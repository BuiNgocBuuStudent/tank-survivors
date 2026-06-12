using System.Collections.Generic;
using UnityEngine;

public class GunController01 : GunControllerBase
{
    [SerializeField] bool _isDoubleBullet;
    [SerializeField] float _angle;

    // Skill: Overcharge Shot (Tier 5) — counter
    private int _shotCounter;

    void Update()
    {
        Fire();
    }

    public override void ApplySkills(List<string> skills)
    {
        base.ApplySkills(skills);

        // Tier 1: Rapid Fire — giảm cooldown 25%
        if (HasSkill("Rapid Fire"))
        {
            _cooldownTime *= 0.75f;
        }

        // Tier 3: Double Barrel — bật chế độ bắn 2 viên
        if (HasSkill("Double Barrel"))
        {
            _isDoubleBullet = true;
        }
    }

    protected override void Fire()
    {
        if (_timer >= 0)
            return;

        _timer = _cooldownTime;

        if (_isDoubleBullet)
        {
            // Bắn 2 viên lệch nhẹ sang 2 bên
            Vector3 offset = this.transform.right * 0.15f;
            SpawnBullet(this.transform.up, this.transform.position + offset);
            SpawnBullet(this.transform.up, this.transform.position - offset);
        }
        else
        {
            SpawnBullet(this.transform.up, this.transform.position);
        }

        _shotCounter++;
    }

    private void SpawnBullet(Vector2 direction, Vector3 spawnPos)
    {
        BulletBase bullet = ObjectPooler.Instance.GetComp(_bulletPrefab);

        // Tier 2: Piercing Rounds — set pierce count
        Bullet01 bullet01 = bullet as Bullet01;
        if (bullet01 != null && HasSkill("Piercing Rounds"))
        {
            bullet01.SetPierceCount(1);
        }

        bullet.Init(_bulletSpeed, direction);
        bullet.transform.SetPositionAndRotation(spawnPos, this.transform.rotation);
        bullet.gameObject.SetActive(true);
    }
}
