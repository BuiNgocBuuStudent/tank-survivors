using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet04 : BulletBase
{
    [Header("------Toxic Config------")]
    [SerializeField] GameObject _burningPrefab;

    [SerializeField] float _burningRange;
    [SerializeField] float _burningDmg;
    [SerializeField] float _burningSpeed;
    [SerializeField] float _burningTime;
    public override void Boom(GameObject target)
    {
        this.gameObject.SetActive(false);

        GameObject prefab = ObjectPooler.Instance.GetObject(_burningPrefab);
        prefab.transform.position = this.transform.position;
        prefab.SetActive(true);
        EffectManager.Instance.StartCoroutine(EffectManager.Instance.Burning(prefab, _burningDmg, _burningRange, _burningTime, _burningSpeed, _targetMask));

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
