using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet02 : BulletBase
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public override void Boom(GameObject target)
    {
        this.gameObject.SetActive(false);
        IGetHit isCanGetHit = target.GetComponent<IGetHit>();

        if (isCanGetHit != null)
        {
            isCanGetHit.GetHit(this._dmg);
        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        this.Boom(collision.gameObject);
    }
}
