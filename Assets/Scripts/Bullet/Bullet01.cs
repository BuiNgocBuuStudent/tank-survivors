using UnityEngine;

public class Bullet01 : BulletBase
{
    // Tier 2: Piercing Rounds — đạn xuyên qua enemy
    private int _pierceCount;

    /// <summary>
    /// Set số lần xuyên. Gọi bởi GunController01 trước khi Init().
    /// 0 = không xuyên (mặc định), 1 = xuyên qua 1 enemy.
    /// </summary>
    public void SetPierceCount(int count)
    {
        _pierceCount = count;
    }

    //private void OnEnable()
    //{
    //    // Reset pierce count mỗi lần bullet được tái sử dụng từ pool
    //    // (nếu không có skill, _pierceCount sẽ vẫn = 0 từ lần Init trước)
    //}

    protected override void Boom(GameObject target)
    {
        _isCanGetHit = target.GetComponent<IGetHit>();
        _isCanGetHit?.GetHit(this._dmg);

        if (_pierceCount > 0)
        {
            // Xuyên qua — giảm pierce count, không tắt bullet
            _pierceCount--;
        }
        else
        {
            // Không còn pierce → tắt bullet
            this.gameObject.SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        this.Boom(collision.gameObject);
    }
}
