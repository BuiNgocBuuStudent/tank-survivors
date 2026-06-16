using System.Collections.Generic;
using UnityEngine;

public class GunController02 : GunControllerBase
{
    [Header("Shotgun config")]
    [SerializeField] int _numberBullet;
    [SerializeField] float _attackRange;

    [Header("Tier 4 — Knockback Blast")]
    [SerializeField] GameObject _knockbackPrefab;
    [SerializeField] LayerMask _enemyLayerMaskForKnockback;
    [SerializeField] float _knockbackRadius = 2f;
    [SerializeField] float _knockbackForce = 400f;

    private int _baseNumberBullet;
    private float _baseAttackRange;
    private bool _hasSlugRound;
    private bool _hasKnockbackBlast;

    public override void Init()
    {
        base.Init();
        _baseNumberBullet = _numberBullet;
        _baseAttackRange = _attackRange;
    }

    void Update()
    {
        Fire();
    }

    public override void ApplySkills(List<string> skills)
    {
        base.ApplySkills(skills);

        // Reset về giá trị gốc trước khi apply
        _numberBullet = _baseNumberBullet;
        _attackRange = _baseAttackRange;
        _hasSlugRound = false;

        // Tier 1: Wider Spread — tăng attackRange 30%
        if (HasSkill("Wider Spread"))
        {
            _attackRange *= 1.3f;
        }

        // Tier 2: Extra Pellets — thêm 3 viên đạn
        if (HasSkill("Extra Pellets"))
        {
            _numberBullet += 3;
        }

        // Tier 3: Slug Round — viên giữa gây ×2 damage
        _hasSlugRound = HasSkill("Slug Round");

        // Tier 4: Knockback Blast
        _hasKnockbackBlast = HasSkill("Knockback Blast");
    }

    protected override void Fire()
    {
        if (_timer >= 0)
            return;

        _timer = _cooldownTime;

        // Tier 4: Knockback Blast — đẩy lùi tất cả enemy gần nơi bắn
        if (_hasKnockbackBlast)
            ApplyKnockback(this.transform.position);

        //góc giữa 2 game object
        float baseAngle = Mathf.Atan2(this.transform.up.y, this.transform.up.x) * Mathf.Rad2Deg;

        float anglePerBullet = _attackRange / (_numberBullet - 1);
        float startSpawnAngle = baseAngle + _attackRange / 2;

        int centerIndex = _numberBullet / 2;

        for (int i = 0; i < _numberBullet; i++)
        {
            float bulletAngle = (startSpawnAngle - anglePerBullet * i) * Mathf.Deg2Rad;
            Vector2 direction = new Vector2(Mathf.Cos(bulletAngle), Mathf.Sin(bulletAngle));

            BulletBase bullet = ObjectPooler.Instance.GetComp(_bulletPrefab);

            // Tier 3: Slug Round — viên giữa bay xa hơn 50%, damage ×2
            if (_hasSlugRound && i == centerIndex)
            {
                bullet.Init(_bulletSpeed * 1.5f, direction);
                bullet.SetDamageMultiplier(2f);
            }
            else
            {
                bullet.Init(_bulletSpeed, direction);
            }

            // Tier 5: Chain Reaction — truyền flag vào bullet
            Bullet02 bullet02 = bullet as Bullet02;
            if (bullet02 != null)
                bullet02.SetChainReaction(HasSkill("Chain Reaction"), _enemyLayerMask);

            bullet.transform.SetPositionAndRotation(this.transform.position, this.transform.rotation);
            bullet.gameObject.SetActive(true);
        }
    }

    /// <summary>
    /// Tier 4 — Knockback Blast
    /// Dùng EffectManager.ApplyKnockback() thay vì AddForce() trực tiếp
    /// vì ChaseTarget() override velocity mỗi FixedUpdate.
    /// </summary>
    private void ApplyKnockback(Vector2 origin)
    {

        Collider2D[] hits = Physics2D.OverlapCircleAll(origin, _knockbackRadius, _enemyLayerMaskForKnockback);
        foreach (Collider2D hit in hits)
        {
            if (hit == null) continue;

            GameObject knockbackEffect = ObjectPooler.Instance.GetObject(_knockbackPrefab);
            knockbackEffect.transform.position = this.transform.position;
            knockbackEffect.SetActive(true);

            // Hướng đẩy = từ tâm ra phía enemy
            Vector2 pushDir = (hit.transform.position - (Vector3)origin).normalized;
            if (pushDir == Vector2.zero)
                pushDir = Random.insideUnitCircle.normalized;

            // Chuyển force thành velocity và giao cho EffectManager quản lý
            Vector2 knockbackVelocity = pushDir * _knockbackForce;
            EffectManager.Instance.ApplyKnockback(hit.gameObject, knockbackVelocity, duration: 0.25f);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(1f, 0.4f, 0f, 0.3f);
        Gizmos.DrawSphere(transform.position, _knockbackRadius);

        Gizmos.color = new Color(1f, 0.4f, 0f, 1f);
        Gizmos.DrawWireSphere(transform.position, _knockbackRadius);
    }
}
