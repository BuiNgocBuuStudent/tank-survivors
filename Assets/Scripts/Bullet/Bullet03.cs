using System.Collections;
using UnityEngine;

public class Bullet03 : BulletBase
{
    [Header("-----AOE Config-----")]
    [SerializeField] GameObject _explosionPrefab;

    [SerializeField] float _damageRadius;
    private float _baseDamageRadius;

    // Skill flags — set bởi GunController03 trước khi SetActive
    private bool _hasBiggerBoom;

    private bool _hasClusterBomb;
    private BulletBase _clusterBulletPrefab;

    private bool _hasShockwave;
    [Header("-----Tier 4: Shockwave Config-----")]
    [SerializeField] private float _shockwaveSlowFactor;
    [SerializeField] private float _shockwaveSlowDuration;

    private bool _hasNapalmStrike;
    [Header("-----Tier 5: Napalm Strike Config-----")]
    [SerializeField] private GameObject _burningZonePrefab;
    [SerializeField] private float _napalmRange;
    [SerializeField] private float _napalmTickInterval;
    [SerializeField] private float _napalmDuration;
    [SerializeField] private float _napalmDmgMultiplier;

    private void Awake()
    {
        _baseDamageRadius = _damageRadius;
    }

    /// <summary>
    /// Được gọi bởi GunController03 trước mỗi lần bắn.
    /// </summary>
    public void SetSkillFlags(bool biggerBoom, bool clusterBomb, BulletBase clusterPrefab,
                               bool shockwave, bool napalmStrike)
    {
        _hasBiggerBoom = biggerBoom;
        _hasClusterBomb = clusterBomb;
        _clusterBulletPrefab = clusterPrefab;
        _hasShockwave = shockwave;
        _hasNapalmStrike = napalmStrike;

        // Tier 1: Bigger Boom — tăng bán kính nổ 30%
        if (_hasBiggerBoom)
        {
            _damageRadius = _baseDamageRadius * 1.3f;
            _explosionPrefab.transform.localScale = Vector3.one * 2.6f;
        }
    }

    protected override void Boom(GameObject target)
    {
        this.gameObject.SetActive(false);

        Vector2 explosionPos = this.transform.position;

        GameObject prefab = ObjectPooler.Instance.GetObject(_explosionPrefab);
        prefab.transform.position = explosionPos;
        prefab.SetActive(true);
        EffectManager.Instance.TriggerExplosion(prefab, _dmg, _damageRadius, _targetMask);

        // Tier 3: Cluster Bomb — spawn 5 mảnh đạn nhỏ bay ra 5 hướng
        if (_hasClusterBomb && _clusterBulletPrefab != null)
        {
            SpawnClusterFragments();
        }

        // Tier 4: Shockwave — slow tất cả enemy trong bán kính sau khi nổ
        if (_hasShockwave)
        {
            EffectManager.Instance.ApplySlowInRadius(
                explosionPos, _damageRadius, _targetMask,
                _shockwaveSlowFactor, _shockwaveSlowDuration
            );
        }

        // Tier 5: Napalm Strike — spawn vùng lửa tại vị trí nổ
        if (_hasNapalmStrike && _burningZonePrefab != null)
        {
            GameObject burningZone = ObjectPooler.Instance.GetObject(_burningZonePrefab);
            burningZone.transform.position = explosionPos;
            burningZone.SetActive(true);

            float napalmDmg = _dmg * _napalmDmgMultiplier;
            EffectManager.Instance.StartCoroutine(
                EffectManager.Instance.Burning(
                    burningZone, napalmDmg, _napalmRange,
                    _napalmDuration, _napalmTickInterval, _targetMask
                )
            );
        }
    }

    /// <summary>
    /// Spawn các mảnh đạn nhỏ tỏa đều xung quanh hướng đạn đang bay
    /// Mỗi mảnh gây 30% damage gốc.
    /// </summary>
    private void SpawnClusterFragments()
    {
        float fragmentDmgMultiplier = 0.3f;
        float fragmentSpeed = _speed * 0.7f;
        int fragmentCount = 5;

        // Lấy góc hiện tại của đạn làm gốc để tỏa ra xung quanh
        float baseAngle = Mathf.Atan2(_movement.y, _movement.x) * Mathf.Rad2Deg;

        for (int i = 0; i < fragmentCount; i++)
        {
            float spreadAngle = baseAngle + (360f / fragmentCount) * i;
            float spreadRad = spreadAngle * Mathf.Deg2Rad;
            Vector2 direction = new Vector2(Mathf.Cos(spreadRad), Mathf.Sin(spreadRad));

            // Xoay sprite theo hướng di chuyển (trừ 90° vì sprite mặc định trỏ lên)
            Quaternion rotation = Quaternion.Euler(0f, 0f, spreadAngle - 90);

            BulletBase fragment = ObjectPooler.Instance.GetComp(_clusterBulletPrefab);
            fragment.Init(fragmentSpeed, direction);
            fragment.SetDamageMultiplier(fragmentDmgMultiplier);
            fragment.transform.SetPositionAndRotation(this.transform.position, rotation);
            fragment.gameObject.SetActive(true);
        }
    }
    protected override IEnumerator RepeatLifeTime()
    {
        while (true)
        {
            yield return new WaitForSeconds(_lifeTime);
            this.gameObject.SetActive(false);
            if (_hasClusterBomb)
                SpawnClusterFragments();
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        this.Boom(collision.gameObject);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(1f, 0.4f, 0f, 0.3f);
        Gizmos.DrawSphere(transform.position, _damageRadius);

        Gizmos.color = new Color(1f, 0.4f, 0f, 1f);
        Gizmos.DrawWireSphere(transform.position, _damageRadius);
    }
}
