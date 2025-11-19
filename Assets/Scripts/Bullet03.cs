using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet03 : BulletBase
{
    [Header("-----AOE Config-----")]
    [SerializeField] float _damageRange;
    private static Collider2D[] _buffer = new Collider2D[50];
    public override void Boom(GameObject target)
    {
        this.gameObject.SetActive(false);

        //OverlapCircleNonAlloc chỉ còn hổ trợ đến phiên bản 2022.3
        int hitCount = Physics2D.OverlapCircleNonAlloc(this.transform.position, _damageRange, _buffer);
        for (int i = 0; i < hitCount ; i++)
        {
            IGetHit isCanGetHit = _buffer[i].GetComponent<IGetHit>();
            if (isCanGetHit != null)
            {
                isCanGetHit.GetHit(this._dmg);
            }
        }

    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        this.Boom(collision.gameObject);
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(this.transform.position, _damageRange);
    }
}
