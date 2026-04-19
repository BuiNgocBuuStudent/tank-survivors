using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet03 : BulletBase
{
    [Header("-----AOE Config-----")]
    [SerializeField] GameObject _explosionPrefab;

    [SerializeField] float _damageRadius;

    protected override void Boom(GameObject target)
    {
        this.gameObject.SetActive(false);

        GameObject prefab = ObjectPooler.Instance.GetObject(_explosionPrefab);
        prefab.transform.position = this.transform.position;
        prefab.SetActive(true);
        EffectManager.Instance.TriggerExplosion(prefab, _dmg, _damageRadius, _targetMask);
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
