using System.Collections.Generic;
using UnityEngine;

public class GunController04 : GunControllerBase
{
    void Update()
    {
        Fire();
    }

    public override void ApplySkills(List<string> skills)
    {
        base.ApplySkills(skills);
        // Tier 1-2 skill effects được xử lý bên Bullet04
        // Tier 3: Corrosive Cloud cũng ở Bullet04 (ảnh hưởng vùng burning)
    }

    protected override void Fire()
    {
        if (_timer >= 0)
            return;

        _timer = _cooldownTime;

        BulletBase bullet = ObjectPooler.Instance.GetComp(_bulletPrefab);
        bullet.Init(_bulletSpeed, this.transform.up);
        bullet.transform.SetPositionAndRotation(this.transform.position, this.transform.rotation);

        // Truyền skill flags cho Bullet04
        Bullet04 bullet04 = bullet as Bullet04;
        if (bullet04 != null)
        {
            bullet04.SetSkillFlags(
                HasSkill("Toxic Expansion"),
                HasSkill("Lingering Fumes"),
                HasSkill("Corrosive Cloud")
            );
        }

        bullet.gameObject.SetActive(true);
    }
}
