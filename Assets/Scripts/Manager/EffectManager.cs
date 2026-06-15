using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ============================================================
//  EffectManager — Ngày 8: Hệ thống nền dùng chung
//  Tích hợp: DOTEffect, DebuffEffect (Slow), EnemyDeathCallback
// ============================================================

public class EffectManager : Singleton<EffectManager>
{
    /// <summary>
    /// Burning zone: gây DOT mỗi tick cho mọi enemy trong vùng,
    /// dùng cho: Tank04 Toxic Cloud
    /// </summary>
    public IEnumerator Burning(GameObject effect, float dmg, float range, float time, float speed, LayerMask mask)
    {
        float timer = 0f;
        while (timer < time)
        {
            Collider2D[] hits = Physics2D.OverlapCircleAll(effect.transform.position, range, mask);
            foreach (Collider2D hit in hits)
            {
                IGetHit isCanGetHit = hit.GetComponent<IGetHit>();
                isCanGetHit?.GetHit(dmg);
            }
            yield return new WaitForSeconds(speed);
            timer += speed;
        }
        effect.SetActive(false);
    }

    /// <summary>
    /// Nổ tức thì: gây damage 1 lần cho mọi enemy trong bán kính,
    /// dùng cho: Tank03 Cannon, Chain Reaction (T02-T5)
    /// </summary>
    public void TriggerExplosion(GameObject effect, float dmg, float range, LayerMask mask)
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(effect.transform.position, range, mask);
        foreach (Collider2D hit in hits)
        {
            hit.GetComponent<IGetHit>()?.GetHit(dmg);
        }
        // effect tự động disable
    }

    /// <summary>
    /// Burning + Corrosive Cloud: enemy trong vùng bị giảm armor -> nhận thêm damage,
    /// armorReduction = 20 -> enemy mất 20% armor khi ở trong vùng,
    /// thực hiện bằng cách tăng damage lên thay vì sửa armor enemy
    /// </summary>
    public IEnumerator CorrosiveBurning(GameObject effect, float dmg, float range,
        float time, float speed, LayerMask mask, float armorReduction)
    {
        float corrosiveMultiplier = 1f / (1f - armorReduction / 100f);
        float corrosiveDmg = dmg * corrosiveMultiplier;

        float timer = 0f;
        while (timer < time)
        {
            Collider2D[] hits = Physics2D.OverlapCircleAll(effect.transform.position, range, mask);
            foreach (Collider2D hit in hits)
            {
                IGetHit isCanGetHit = hit.GetComponent<IGetHit>();
                isCanGetHit?.GetHit(corrosiveDmg);
            }
            yield return new WaitForSeconds(speed);
            timer += speed;
        }
        effect.SetActive(false);
    }
    #region DOT EFFECT
    //  Dùng cho: Incendiary Ammo (T01-T4),
    //            Napalm Strike (T03-T5),
    //            Toxic Trail (T04-T4)

    /// <summary>
    /// Gắn DOT (Damage Over Time) trực tiếp lên một enemy.
    /// Tick damage mỗi <tickInterval> giây trong <duration> giây.
    /// Nếu enemy đã có DOT thì làm mới thời gian (không stack)
    /// </summary>
    public void ApplyDOT(GameObject target, float dmgPerTick, float tickInterval, float duration)
    {
        if (target == null || !target.activeInHierarchy) return;

        EnemyDOTState dot = target.GetComponent<EnemyDOTState>();
        if (dot == null)
            dot = target.AddComponent<EnemyDOTState>();

        dot.Apply(dmgPerTick, tickInterval, duration);
    }

    /// <summary>
    /// Gắn DOT lên mọi enemy trong bán kính (dùng cho vùng lửa / nổ)
    /// </summary>
    public void ApplyDOTInRadius(Vector2 center, float radius, LayerMask mask,
                                 float dmgPerTick, float tickInterval, float duration)
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(center, radius, mask);
        foreach (Collider2D hit in hits)
            ApplyDOT(hit.gameObject, dmgPerTick, tickInterval, duration);
    }
    #endregion
    //  2. DEBUFF EFFECT — SLOW
    //  Dùng cho: Shockwave (T03-T4)

    /// <summary>
    /// Giảm tốc độ di chuyển của enemy trong một khoảng thời gian;
    /// không stack, chỉ làm mới
    /// </summary>
    /// <param name="slowFactor">Tỉ lệ giảm tốc</param>
    public void ApplySlow(GameObject target, float slowFactor, float duration)
    {
        if (target == null || !target.activeInHierarchy) return;

        EnemySlowState slow = target.GetComponent<EnemySlowState>();
        if (slow == null)
            slow = target.AddComponent<EnemySlowState>();

        slow.Apply(slowFactor, duration);
    }

    /// <summary>
    /// Slow mọi enemy trong bán kính (dùng sau vụ nổ Shockwave)
    /// </summary>
    public void ApplySlowInRadius(Vector2 center, float radius, LayerMask mask,
                                   float slowFactor, float duration)
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(center, radius, mask);
        foreach (Collider2D hit in hits)
            ApplySlow(hit.gameObject, slowFactor, duration);
    }
}

// ============================================================
//  ENEMY STATE COMPONENTS
//  MonoBehaviour nhỏ gắn tạm lên enemy khi cần,
//  tự xóa khi hết thời gian.
// ============================================================

/// <summary>
/// Component DOT: gắn lên enemy, tự tick damage và tự hủy.
/// Nhiều nguồn DOT có thể gọi Apply() để làm mới thay vì stack
/// </summary>
public class EnemyDOTState : MonoBehaviour
{
    private Coroutine _dotCoroutine;

    /// <summary>
    /// Bắt đầu (hoặc làm mới) DOT với thông số mới
    /// </summary>
    public void Apply(float dmgPerTick, float tickInterval, float duration)
    {
        if (_dotCoroutine != null)
            StopCoroutine(_dotCoroutine);

        _dotCoroutine = StartCoroutine(DOTRoutine(dmgPerTick, tickInterval, duration));
    }

    private IEnumerator DOTRoutine(float dmgPerTick, float tickInterval, float duration)
    {
        float elapsed = 0f;
        IGetHit victim = GetComponent<IGetHit>();

        while (elapsed < duration)
        {
            yield return new WaitForSeconds(tickInterval);
            elapsed += tickInterval;

            // Kiểm tra enemy còn active không trước khi gây damage
            if (!gameObject.activeInHierarchy) yield break;

            victim?.GetHit(dmgPerTick);
        }

        Destroy(this);
    }

    private void OnDisable()
    {
        if (_dotCoroutine != null)
        {
            StopCoroutine(_dotCoroutine);
            _dotCoroutine = null;
        }
    }
}

/// <summary>
/// Component Slow: giảm tốc độ enemy, tự restore và tự hủy.
/// Không stack — gọi Apply() nhiều lần sẽ làm mới duration
/// </summary>
public class EnemySlowState : MonoBehaviour
{
    private Coroutine _slowCoroutine;
    private EnemyControllerBase _enemy;

    private void Awake()
    {
        _enemy = GetComponent<EnemyControllerBase>();
    }

    /// <summary>
    /// Áp dụng (hoặc làm mới) slow
    /// </summary>
    public void Apply(float slowFactor, float duration)
    {
        if (_enemy == null) return;

        if (_slowCoroutine != null)
            StopCoroutine(_slowCoroutine);

        _slowCoroutine = StartCoroutine(SlowRoutine(slowFactor, duration));
    }

    private IEnumerator SlowRoutine(float slowFactor, float duration)
    {
        _enemy.SetSpeedMultiplier(1f - slowFactor);
        yield return new WaitForSeconds(duration);

        if (gameObject.activeInHierarchy)
            _enemy.SetSpeedMultiplier(1f); // Restore tốc độ gốc

        Destroy(this);
    }

    private void OnDisable()
    {
        // Restore ngay khi enemy chết/bị disable
        _enemy?.SetSpeedMultiplier(1f);
        if (_slowCoroutine != null)
        {
            StopCoroutine(_slowCoroutine);
            _slowCoroutine = null;
        }
    }
}
