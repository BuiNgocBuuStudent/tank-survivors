using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public abstract class EnemyControllerBase : MonoBehaviour, IGetHit
{
    private PlayerControllerBase _player;
    private EnemyManager _enemyManager;
    [SerializeField] private Rigidbody2D _rb;
    [SerializeField] FlashEffect _flashEffect;
    [SerializeField] EnemyConfigBase _enemyDataBase;

    [SerializeField] float _currentHealth;
    /// <summary>EnemySlowState sẽ điều chỉnh field này</summary>
    [SerializeField] float _speedMultiplier = 1f;

    /// <summary>Khi true: ChaseTarget() bị bỏ qua, nhường cho knockback velocity</summary>
    private bool _isKnockedBack;

    protected Rigidbody2D Rb => _rb;
    protected PlayerControllerBase Player => _player;
    protected EnemyConfigBase EnemyData => _enemyDataBase;

    /// <summary>
    /// Phát ra khi enemy chết. Truyền vị trí và damage của đòn cuối
    /// Bullet/Skill subscribe vào event này để trigger chain effects
    /// </summary>
    public static event Action<Vector3, float> OnEnemyDeath;

    public void Init(Vector2 randomSpawnPos)
    {
        if (_rb == null)
            _rb = this.GetComponent<Rigidbody2D>();
        _flashEffect = this.GetComponentInChildren<FlashEffect>();
        _player = GameManager.Instance.Player;
        _enemyManager = GameManager.Instance.EnemyManager;

        _currentHealth = _enemyDataBase.intialHealth;
        this.transform.position = randomSpawnPos;

        OnInit();
    }

    /// <summary>
    /// Hook cho subclass khởi tạo thêm sau khi Init() chạy xong.
    /// Override để reset state machine, timer... mỗi lần enemy được re-spawn.
    /// </summary>
    protected virtual void OnInit() { }

    private void FixedUpdate()
    {
        ChaseTarget();
        UpdateBehavior();
    }

    /// <summary>
    /// Di chuyển về phía player. Override trong subclass để thay đổi hoàn toàn
    /// cách di chuyển (ví dụ: Shooter dừng lại khi trong range).
    /// Nhớ gọi base.ChaseTarget() nếu muốn giữ nguyên hành vi chase gốc.
    /// </summary>
    protected virtual void ChaseTarget()
    {
        // Khi đang bị knockback: không ghi đè velocity, để physics tự xử lý
        if (_isKnockedBack) return;

        Vector2 movement = _player.transform.position - this.transform.position;

        float angle = Mathf.Atan2(movement.y, movement.x) * Mathf.Rad2Deg + 90;
        Quaternion quaternion = this.transform.rotation;
        quaternion.eulerAngles = new Vector3(0, 0, angle);
        this.transform.rotation = quaternion;

        _rb.velocity = movement.normalized * (_enemyDataBase.moveSpeed * _speedMultiplier);
    }

    /// <summary>
    /// Hook cho subclass thêm logic Update mỗi FixedUpdate (timer, state, bắn đạn...).
    /// Chạy sau ChaseTarget(). Mặc định không làm gì.
    /// </summary>
    protected virtual void UpdateBehavior() { }

    /// <summary>
    /// Inject knockback velocity trực tiếp vào Rigidbody.
    /// Gọi bởi EnemyKnockbackState — thay thế AddForce() vì ChaseTarget overwrite velocity.
    /// </summary>
    public void SetKnockbackVelocity(Vector2 velocity)
    {
        _isKnockedBack = true;
        _rb.velocity = velocity;
    }

    /// <summary>Restore chase sau khi knockback kết thúc</summary>
    public void ClearKnockback()
    {
        _isKnockedBack = false;
    }

    public virtual void GetHit(float dmg)
    {
        if (gameObject.activeSelf)
            _flashEffect.Flash();

        _currentHealth -= dmg;
        if (_currentHealth <= 0)
        {
            OnDie(dmg);
        }
    }

    protected virtual void OnDie(float lastDmg)
    {
        OnEnemyDeath?.Invoke(this.transform.position, lastDmg);

        this.gameObject.SetActive(false);
        _flashEffect.ResetMaterial();
        _enemyManager.SpawnExpGem(this.transform.position);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        IGetHit isCanGetHit = collision.gameObject.GetComponent<IGetHit>();
        if (isCanGetHit == null)
            return;
        isCanGetHit.GetHit(_enemyDataBase.damage);
    }

    /// <summary>
    /// Được gọi bởi EnemySlowState để giảm/restore tốc độ
    /// </summary>
    public void SetSpeedMultiplier(float multiplier)
    {
        _speedMultiplier = Mathf.Clamp(multiplier, 0.1f, 2f);
    }
}
