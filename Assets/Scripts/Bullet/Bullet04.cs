using System.Collections;
using UnityEngine;

public class Bullet04 : BulletBase
{
    [Header("------Toxic Config------")]
    [SerializeField] GameObject _toxicZonePrefab;

    [SerializeField] float _burningRange;
    [SerializeField] float _burningSpeed;
    [SerializeField] float _burningTime;

    private float _baseBurningRange;
    private float _baseBurningTime;

    // Skill flags
    private bool _hasToxicExpansion;
    private bool _hasLingeringFumes;
    private bool _hasCorrosiveCloud;

    // Tier 4: Toxic Trail
    private bool _hasToxicTrail;

    // Tier 5: Plague Carrier
    private bool _hasPlagueCarrier;

    [Header("-----Tier 4: Toxic Trail Config-----")]
    [SerializeField] private float _trailInterval;
    [SerializeField] private float _miniZoneRange;
    [SerializeField] private float _miniZoneDmgMultiplier;
    [SerializeField] private float _miniZoneDuration;
    [SerializeField] private float _miniZoneTickInterval;

    [Header("-----Tier 5: Plague Carrier Config-----")]
    [SerializeField] private float _plagueDetectRadius;

    // Timer nội bộ — tích lũy delta time trong Move()
    private float _trailTimer;

    // Vị trí vùng độc đang active (để Plague Carrier check khoảng cách)
    private Vector2 _lastBoomPos;
    private bool _hasBoomOccurred;

    private void Awake()
    {
        _baseBurningRange = _burningRange;
        _baseBurningTime = _burningTime;
    }

    /// <summary>
    /// Được gọi bởi GunController04 trước mỗi lần bắn.
    /// </summary>
    public void SetSkillFlags(bool toxicExpansion, bool lingeringFumes, bool corrosiveCloud,
                               bool toxicTrail, bool plagueCarrier)
    {
        _hasToxicExpansion = toxicExpansion;
        _hasLingeringFumes = lingeringFumes;
        _hasCorrosiveCloud = corrosiveCloud;
        _hasToxicTrail = toxicTrail;
        _hasPlagueCarrier = plagueCarrier;

        // Tier 1: Toxic Expansion — tăng vùng độc 35%
        if (_hasToxicExpansion)
        {
            _burningRange = _baseBurningRange * 1.35f;
            _toxicZonePrefab.transform.localScale = Vector3.one * 1.3f;
        }
        else
        {
            _burningRange = _baseBurningRange;
        }

        // Tier 2: Lingering Fumes — đám mây tồn tại lâu hơn 50%
        _burningTime = _hasLingeringFumes ? _baseBurningTime * 1.5f : _baseBurningTime;
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        _trailTimer = 0f;
        _hasBoomOccurred = false;

        if (_hasPlagueCarrier)
            EnemyControllerBase.OnEnemyDeath += OnEnemyDeathHandler;
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        EnemyControllerBase.OnEnemyDeath -= OnEnemyDeathHandler;
    }

    protected override void Move()
    {
        base.Move();

        // Tier 4: Toxic Trail — spawn MiniToxicZone dọc đường bay
        if (!_hasToxicTrail || _toxicZonePrefab == null) return;

        _trailTimer += Time.fixedDeltaTime;
        if (_trailTimer >= _trailInterval)
        {
            _trailTimer = 0f;
            SpawnMiniToxicZone(this.transform.position);
        }
    }

    private void SpawnMiniToxicZone(Vector2 pos)
    {
        GameObject zone = ObjectPooler.Instance.GetObject(_toxicZonePrefab);
        zone.transform.localScale = Vector3.one * 0.3f;
        zone.transform.position = pos;
        zone.SetActive(true);

        float miniDmg = _dmg * _miniZoneDmgMultiplier;
        EffectManager.Instance.StartCoroutine(
            EffectManager.Instance.Burning(
                zone, miniDmg, _miniZoneRange,
                _miniZoneDuration, _miniZoneTickInterval, _targetMask
            )
        );
    }

    protected override void Boom(GameObject target)
    {
        _lastBoomPos = this.transform.position;
        _hasBoomOccurred = true;

        this.gameObject.SetActive(false);

        GameObject prefab = ObjectPooler.Instance.GetObject(_toxicZonePrefab);
        prefab.transform.position = _lastBoomPos;
        prefab.SetActive(true);

        // Tier 3: Corrosive Cloud: Enemy trong vùng độc bị nhận thêm 20% damage
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
            EffectManager.Instance.StartCoroutine(
                EffectManager.Instance.Burning(
                    prefab, _dmg, _burningRange, _burningTime, _burningSpeed, _targetMask
                )
            );
        }
    }

    private void OnEnemyDeathHandler(Vector3 deathPos, float lastDmg)
    {
        // Chỉ kích hoạt sau khi đạn đã nổ (vùng độc đã xuất hiện)
        if (!_hasBoomOccurred) return;

        // kiểm tra xem chúng có trong vùng độc hiện tại không
        float dist = Vector2.Distance(deathPos, _lastBoomPos);
        if (dist > _plagueDetectRadius) return;

        SpawnPlagueZone(deathPos);
    }

    private void SpawnPlagueZone(Vector2 pos)
    {
        if (_toxicZonePrefab == null) return;

        GameObject miniZone = ObjectPooler.Instance.GetObject(_toxicZonePrefab);
        miniZone.transform.position = pos;
        miniZone.transform.localScale = Vector3.one * 0.5f;
        miniZone.SetActive(true);

        float miniDmg = _dmg * 0.5f;
        float miniRange = _burningRange * 0.5f;
        float miniTime = _burningTime * 0.5f;

        EffectManager.Instance.StartCoroutine(
            EffectManager.Instance.Burning(
                miniZone, miniDmg, miniRange, miniTime, _burningSpeed, _targetMask
            )
        );
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
