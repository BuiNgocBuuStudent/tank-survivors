using System.Collections.Generic;
using UnityEngine;

public class Bullet02 : BulletBase
{
    // Tier 5: Chain Reaction — bật/tắt bởi GunController02
    private bool _hasChainReaction;
    private LayerMask _enemyMask;

    // Mỗi enemy chỉ spawn đúng 1 explosion dù có nhiều bullet cùng trúng
    // Key = (frameCount, deathPos) — các handler trong cùng 1 event 
    // sẽ thấy key đã tồn tại và bỏ qua
    private static readonly HashSet<(int, Vector3)> _processedDeaths = new HashSet<(int, Vector3)>();

    [Header("---------Tier 5: Chain Reaction Config---------")]
    [SerializeField] GameObject _explosionPrefab;
    [SerializeField] private float _chainDamageMultiplier;
    [SerializeField] private float _chainRadius;

    /// <summary>
    /// Bật Chain Reaction. Gọi bởi GunController02 sau khi ApplySkills()
    /// </summary>
    public void SetChainReaction(bool active, LayerMask enemyMask)
    {
        _hasChainReaction = active;
        _enemyMask = enemyMask;
    }

    protected override void Boom(GameObject target)
    {
        this.gameObject.SetActive(false);

        _isCanGetHit = target.GetComponent<IGetHit>();
        _isCanGetHit?.GetHit(this._dmg);

        // Tier 5: Chain Reaction — chỉ subscribe SAU KHI bullet trúng enemy
        if (_hasChainReaction)
            EnemyControllerBase.OnEnemyDeath += OnEnemyDeathHandler;
    }

    /// <summary>
    /// Handler nhận event khi BẤT KỲ enemy nào chết.
    /// Chỉ spawn explosion tại đúng vị trí enemy vừa chết
    /// rồi tự unsubscribe ngay để không lắng nghe tiếp.
    /// </summary>
    private void OnEnemyDeathHandler(Vector3 deathPos, float lastDmg)
    {
        EnemyControllerBase.OnEnemyDeath -= OnEnemyDeathHandler;

        // nếu bullet khác đã xử lý cái chết này trong frame này thì bỏ qua
        // dùng frameCount vì nếu 2 enemy chết cùng 1 vị trí (deathPos) ở 2 frame ở khác nhau
        // => vẫn trigger chain explosion
        var key = (Time.frameCount, deathPos);
        if (_processedDeaths.Contains(key)) return;
        _processedDeaths.Add(key);

        SpawnChainExplosion(deathPos, lastDmg);
    }

    private void SpawnChainExplosion(Vector3 deathPos, float dmg)
    {
        if (_explosionPrefab == null) return;

        GameObject explosionEffect = ObjectPooler.Instance.GetObject(_explosionPrefab);
        explosionEffect.transform.position = deathPos;
        explosionEffect.SetActive(true);

        float chainDmg = dmg * _chainDamageMultiplier;
        EffectManager.Instance.TriggerExplosion(explosionEffect, chainDmg, _chainRadius, _enemyMask);
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        EnemyControllerBase.OnEnemyDeath -= OnEnemyDeathHandler;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        this.Boom(collision.gameObject);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(1f, 0.4f, 0f, 0.3f);
        Gizmos.DrawSphere(transform.position, _chainRadius);

        Gizmos.color = new Color(1f, 0.4f, 0f, 1f);
        Gizmos.DrawWireSphere(transform.position, _chainRadius);
    }
}
