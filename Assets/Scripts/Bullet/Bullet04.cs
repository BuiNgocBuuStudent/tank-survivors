using UnityEngine;

public class Bullet04 : BulletBase
{
    [Header("------Toxic Config------")]
    [SerializeField] GameObject _burningPrefab;

    [SerializeField] float _burningRange;
    [SerializeField] float _burningSpeed;
    [SerializeField] float _burningTime;

    private float _baseBurningRange;
    private float _baseBurningTime;

    // Skill flags
    private bool _hasToxicExpansion;
    private bool _hasLingeringFumes;
    private bool _hasCorrosiveCloud;

    private void Awake()
    {
        _baseBurningRange = _burningRange;
        _baseBurningTime = _burningTime;
    }

    /// <summary>
    /// Được gọi bởi GunController04 trước mỗi lần bắn.
    /// </summary>
    public void SetSkillFlags(bool toxicExpansion, bool lingeringFumes, bool corrosiveCloud)
    {
        _hasToxicExpansion = toxicExpansion;
        _hasLingeringFumes = lingeringFumes;
        _hasCorrosiveCloud = corrosiveCloud;

        // Tier 1: Toxic Expansion — tăng vùng độc 35%
        _burningRange = _hasToxicExpansion ? _baseBurningRange * 1.35f : _baseBurningRange;

        // Tier 2: Lingering Fumes — đám mây tồn tại lâu hơn 50%
        _burningTime = _hasLingeringFumes ? _baseBurningTime * 1.5f : _baseBurningTime;
    }

    protected override void Boom(GameObject target)
    {
        this.gameObject.SetActive(false);

        GameObject prefab = ObjectPooler.Instance.GetObject(_burningPrefab);
        prefab.transform.position = this.transform.position;
        prefab.SetActive(true);

        // Tier 3: Corrosive Cloud — dùng CorrosiveBurning thay vì Burning bình thường
        if (_hasCorrosiveCloud)
        {
            EffectManager.Instance.StartCoroutine(
                EffectManager.Instance.CorrosiveBurning(
                    prefab, _dmg, _burningRange, _burningTime, _burningSpeed, _targetMask, 20f
                )
            );
        }
        else
        {
            //Đạn bình thường chưa có skill
            EffectManager.Instance.StartCoroutine(
                EffectManager.Instance.Burning(
                    prefab, _dmg, _burningRange, _burningTime, _burningSpeed, _targetMask
                )
            );
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        this.Boom(collision.gameObject);
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(this.transform.position, _burningRange);
    }
}
