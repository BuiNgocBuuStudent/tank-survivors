using System.Collections.Generic;
using UnityEngine;

public class GunController01 : GunControllerBase
{
    [SerializeField] bool _isDoubleBullet;

    // Tier 4: Incendiary Ammo
    private bool _hasIncendiaryAmmo;

    // Tier 5: Overcharge Shot
    private int _shotCounter;
    private bool _hasOverchargeShot;
    private const int OverchargeInterval = 5;

    void Update()
    {
        Fire();
    }

    public override void ApplySkills(List<string> skills)
    {
        base.ApplySkills(skills);

        // Tier 1: Rapid Fire
        if (HasSkill("Rapid Fire"))
        {
            _cooldownTime *= 0.75f;
        }

        // Tier 3: Double Barrel
        if (HasSkill("Double Barrel"))
        {
            _isDoubleBullet = true;
        }

        // Tier 4: Incendiary Ammo
        _hasIncendiaryAmmo = HasSkill("Incendiary Ammo");

        // Tier 5: Overcharge Shot
        _hasOverchargeShot = HasSkill("Overcharge Shot");

    }

    protected override void Fire()
    {
        if (_timer >= 0)
            return;

        _timer = _cooldownTime;
        _shotCounter++;

        // Tier 5: Overcharge Shot
        if (_hasOverchargeShot && _shotCounter % OverchargeInterval == 0)
            SpawnOverchargeBullet(this.transform.up, this.transform.position);

        if (_isDoubleBullet)
        {
            // Bắn 2 viên lệch nhẹ sang 2 bên
            Vector3 offset = this.transform.right * 0.15f;
            SpawnBaseBullet(this.transform.up, this.transform.position + offset);
            SpawnBaseBullet(this.transform.up, this.transform.position - offset);
        }
        else
            SpawnBaseBullet(this.transform.up, this.transform.position);
    }

    private void SpawnBaseBullet(Vector2 direction, Vector3 spawnPos)
    {
        BulletBase bullet = ObjectPooler.Instance.GetComp(_bulletPrefab);

        Bullet01 bullet01 = bullet as Bullet01;
        if (bullet01 != null)
        {
            // Tier 2: Piercing Rounds
            if (HasSkill("Piercing Rounds"))
                bullet01.SetPierceCount(1);

            // Tier 4: Incendiary Ammo
            bullet01.SetIncendiaryAmmo(_hasIncendiaryAmmo);
        }

        bullet.Init(_bulletSpeed, direction);
        bullet.transform.SetPositionAndRotation(spawnPos, this.transform.rotation);
        bullet.gameObject.SetActive(true);
    }

    // Tier 5 — Overcharge Shot
    private void SpawnOverchargeBullet(Vector2 direction, Vector3 spawnPos)
    {
        BulletBase bullet = ObjectPooler.Instance.GetComp(_bulletPrefab);

        Bullet01 bullet01 = bullet as Bullet01;
        if (bullet01 != null)
        {
            if (HasSkill("Piercing Rounds"))
                bullet01.SetPierceCount(1);

            bullet01.SetIncendiaryAmmo(_hasIncendiaryAmmo);
        }

        bullet.Init(_bulletSpeed, direction);
        bullet.transform.SetPositionAndRotation(spawnPos, this.transform.rotation);

        bullet.SetDamageMultiplier(3f);
        bullet.transform.localScale = Vector3.one * 2;

        bullet.gameObject.SetActive(true);
    }
}
