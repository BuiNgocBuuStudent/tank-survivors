using System.Collections.Generic;
using UnityEngine;

public class GunController03 : GunControllerBase
{
    [Header("-----Cluster Bomb Config-----")]
    [Tooltip("Prefab cho mảnh đạn nhỏ (Cluster Bomb Tier 3). Dùng Bullet01 prefab.")]
    [SerializeField] BulletBase _clusterBulletPrefab;

    private bool _hasClusterBomb;
    private bool _hasShockwave;     // Tier 4
    private bool _hasNapalmStrike;  // Tier 5

    void Update()
    {
        Fire();
    }

    public override void ApplySkills(List<string> skills)
    {
        base.ApplySkills(skills);

        // Tier 2: Faster Reload — giảm cooldown 20%
        if (HasSkill("Faster Reload"))
        {
            _cooldownTime *= 0.8f;
        }

        // Tier 3: Cluster Bomb — flag để Bullet03 biết spawn mảnh đạn
        _hasClusterBomb = HasSkill("Cluster Bomb");

        // Tier 4: Shockwave — slow enemy sau khi nổ
        _hasShockwave = HasSkill("Shockwave");

        // Tier 5: Napalm Strike — spawn vùng lửa tại điểm nổ
        _hasNapalmStrike = HasSkill("Napalm Strike");
    }

    protected override void Fire()
    {
        if (_timer >= 0)
            return;

        _timer = _cooldownTime;

        BulletBase bullet = ObjectPooler.Instance.GetComp(_bulletPrefab);
        bullet.Init(_bulletSpeed, this.transform.up);
        bullet.transform.SetPositionAndRotation(this.transform.position, this.transform.rotation);

        // Tier 1: Bigger Boom — truyền flag vào Bullet03
        Bullet03 bullet03 = bullet as Bullet03;
        if (bullet03 != null)
        {
            bullet03.SetSkillFlags(
                HasSkill("Bigger Boom"),
                _hasClusterBomb,
                _clusterBulletPrefab,
                _hasShockwave,
                _hasNapalmStrike
            );
        }

        bullet.gameObject.SetActive(true);
    }
}
