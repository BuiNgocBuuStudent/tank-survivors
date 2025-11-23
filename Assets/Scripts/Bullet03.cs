using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet03 : BulletBase
{
    [Header("-----AOE Config-----")]
    [SerializeField] float _damageRadius;
    [SerializeField] LayerMask _targetMask;

    public override void Boom(GameObject target)
    {
        this.gameObject.SetActive(false);

        Collider2D[] hits = Physics2D.OverlapCircleAll(this.transform.position, _damageRadius, _targetMask);
        foreach (Collider2D hit in hits)
        {
            IGetHit isCanGetHit = hit.GetComponent<IGetHit>();
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
        Gizmos.DrawWireSphere(this.transform.position, _damageRadius);
    }
}
