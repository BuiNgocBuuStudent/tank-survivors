using System.Collections;
using UnityEngine;

public class EliteArtillery : EnemyControllerBase
{
    [Header("--- Enemy Elite Artillery Config ---")]
    [SerializeField] private GameObject _warningCircle;
    [SerializeField] private ArtilleryBullet _artilleryPrefab;

    private EliteArtilleryConfig _config => (EliteArtilleryConfig)EnemyData;

    private bool _isFiring;
    private float _fireCooldownTimer;

    protected override void OnInit()
    {
        _isFiring = false;
        _fireCooldownTimer = 0f;
    }

    protected override void ChaseTarget()
    {
        if (_isFiring) return;

        float distance = (Player.transform.position - this.transform.position).magnitude;
        if (distance <= _config.stopRange)
        {
            Rb.velocity = Vector2.zero;
            FacePlayer();
        }
        else
        {
            base.ChaseTarget();
        }
    }

    protected override void UpdateBehavior()
    {
        if (_isFiring) return;

        _fireCooldownTimer -= Time.deltaTime;

        float distance = (Player.transform.position - this.transform.position).magnitude;
        if (distance <= _config.stopRange && _fireCooldownTimer <= 0f)
        {
            StartCoroutine(FireSequence());
        }
    }

    private IEnumerator FireSequence()
    {
        _isFiring = true;

        Vector3 targetPos = Player.transform.position;
        GameObject warningCircle = ObjectPooler.Instance.GetObject(_warningCircle);
        if (warningCircle != null)
        {
            warningCircle.transform.position = targetPos;
            warningCircle.transform.localScale = Vector3.zero;
            warningCircle.SetActive(true);
        }

        // Animate scale WarningCircle 0 → 6 trong aimDuration
        float elapsed = 0f;
        float aimDuration = _config.aimDuration;

        while (elapsed < aimDuration)
        {
            if (!gameObject.activeInHierarchy) yield break;

            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / aimDuration);
            float scale = Mathf.Lerp(0f, 6f, t);

            if (warningCircle != null)
                warningCircle.transform.localScale = Vector3.one * scale;

            yield return null;
        }

        if (!gameObject.activeInHierarchy) yield break;

        if (warningCircle != null)
            warningCircle.SetActive(false);

        Shoot(targetPos);

        _fireCooldownTimer = _config.fireCooldown;
        _isFiring = false;
    }

    private void Shoot(Vector3 targetPos)
    {
        ArtilleryBullet bullet = ObjectPooler.Instance.GetComp(_artilleryPrefab);

        Vector2 dir = (targetPos - this.transform.position).normalized;

        bullet.Init(_config.shellSpeed, dir);
        bullet.transform.SetPositionAndRotation(this.transform.position, this.transform.rotation);
        bullet.gameObject.SetActive(true);
    }

    protected override void OnDie(float lastDmg)
    {
        if (_warningCircle != null)
            _warningCircle.SetActive(false);

        _isFiring = false;
        StopAllCoroutines();

        base.OnDie(lastDmg);
    }

    private void FacePlayer()
    {
        Vector2 dir = Player.transform.position - this.transform.position;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg + 90f;
        Quaternion quaternion = this.transform.rotation;
        quaternion.eulerAngles = new Vector3(0f, 0f, angle);
        this.transform.rotation = quaternion;
    }

    private void OnDrawGizmosSelected()
    {
        if (_config == null) return;

        // Vùng dừng và bắn — màu đỏ
        Gizmos.color = new Color(1f, 0f, 0f, 0.2f);
        Gizmos.DrawSphere(transform.position, _config.stopRange);

        Gizmos.color = new Color(1f, 0f, 0f, 0.8f);
        Gizmos.DrawWireSphere(transform.position, _config.stopRange);
    }
}
