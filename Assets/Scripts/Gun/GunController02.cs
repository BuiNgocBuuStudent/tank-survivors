using System.Collections.Generic;
using UnityEngine;

public class GunController02 : GunControllerBase
{
    [Header("Shotgun config")]
    [SerializeField] int _numberBullet;
    [SerializeField] float _attackRange;

    private int _baseNumberBullet;
    private float _baseAttackRange;
    private bool _hasSlugRound;

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
    }

    protected override void Fire()
    {
        if (_timer >= 0)
            return;

        _timer = _cooldownTime;

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

            bullet.transform.SetPositionAndRotation(this.transform.position, this.transform.rotation);
            bullet.gameObject.SetActive(true);
        }

    }
}
