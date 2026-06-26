using UnityEngine;

public class Bullet01 : BulletBase
{
    // Tier 2: Piercing Rounds — đạn xuyên qua enemy
    private int _pierceCount;

    // Tier 4: Incendiary Ammo — bật/tắt bởi GunController01
    private bool _hasIncendiaryAmmo;

    /// <summary>
    /// Set số lần xuyên. Gọi bởi GunController01 trước khi Init().
    /// 0 = không xuyên (mặc định), 1 = xuyên qua 1 enemy.
    /// </summary>
    public void SetPierceCount(int count)
    {
        _pierceCount = count;
    }

    public void SetIncendiaryAmmo(bool active)
    {
        _hasIncendiaryAmmo = active;
    }

    protected override void Boom(GameObject target)
    {
        _isCanGetHit = target.GetComponent<IGetHit>();
        _isCanGetHit?.GetHit(this._dmg);

        // Tier 4: Incendiary Ammo
        if (_hasIncendiaryAmmo)
        {
            float dotDmgPerTick = _dmg * 0.12f;
            EffectManager.Instance.ApplyDOT(target, dotDmgPerTick, tickInterval: 0.5f, duration: 2f);
        }

        //Tier 2: Piercing Rounds
        if (_pierceCount > 0)
        {
            _pierceCount--;
        }
        else
        {
            this.gameObject.SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        this.Boom(collision.gameObject);
    }
}
