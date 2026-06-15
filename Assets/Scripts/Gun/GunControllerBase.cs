using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public abstract class GunControllerBase : MonoBehaviour
{
    [Header("-----Base config-----")]
    [SerializeField] protected BulletBase _bulletPrefab;
    protected PlayerControllerBase _player;
    [SerializeField] protected LayerMask _enemyLayerMask;

    [SerializeField] protected float _cooldownTime;
    protected float _baseCooldownTime;
    protected float _timer;

    [Header("-----Bullet config-----")]
    [SerializeField] protected float _bulletSpeed;

    // Skill system
    [SerializeField] protected List<string> _activeSkills = new List<string>();

    public virtual void Init()
    {
        _player = GameManager.Instance.Player;
        _timer = 0;
        _baseCooldownTime = _cooldownTime;
    }

    public virtual void ApplySkills(List<string> skills)
    {
        _activeSkills = skills ?? new List<string>();
        // Reset cooldown về giá trị gốc trước khi apply skill
        _cooldownTime = _baseCooldownTime;
    }

    public bool HasSkill(string skillName)
    {
        return _activeSkills.Contains(skillName);
    }

    private void FixedUpdate()
    {
        _timer -= Time.deltaTime;

    }
    protected abstract void Fire();
}
