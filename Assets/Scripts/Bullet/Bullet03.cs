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

    private void Awake()
    {
        _baseDamageRadius = _damageRadius;
    }

    /// <summary>
    /// Được gọi bởi GunController03 trước mỗi lần bắn.
    /// </summary>
    public void SetSkillFlags(bool biggerBoom, bool clusterBomb, BulletBase clusterPrefab)
    {
        _hasBiggerBoom = biggerBoom;
        _hasClusterBomb = clusterBomb;
        _clusterBulletPrefab = clusterPrefab;

        // Tier 1: Bigger Boom — tăng bán kính nổ 30%
        _damageRadius = _hasBiggerBoom ? _baseDamageRadius * 1.3f : _baseDamageRadius;
    }

    protected override void Boom(GameObject target)
    {
        this.gameObject.SetActive(false);

        GameObject prefab = ObjectPooler.Instance.GetObject(_explosionPrefab);
        prefab.transform.position = this.transform.position;
        prefab.SetActive(true);
        EffectManager.Instance.TriggerExplosion(prefab, _dmg, _damageRadius, _targetMask);

        // Tier 3: Cluster Bomb — spawn 3 mảnh đạn nhỏ bay ra 3 hướng
        if (_hasClusterBomb && _clusterBulletPrefab != null)
        {
            SpawnClusterFragments();
        }
    }

    /// <summary>
    /// Spawn các mảnh đạn nhỏ tỏa đều xung quanh hướng đạn đang bay.
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
            SpawnClusterFragments();
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        this.Boom(collision.gameObject);
    }
}
