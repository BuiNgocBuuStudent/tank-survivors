using System.Collections;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

public class ArtilleryBullet : EnemyBulletBase
{
    [Header("--- Artillery Explosion ---")]
    [SerializeField] private GameObject _explosionEffectPrefab;
    [SerializeField] private float _explosionRadius;

    protected override void Boom(GameObject target)
    {
        TriggerExplosion();
    }

    protected override IEnumerator RepeatLifeTime()
    {
        yield return new WaitForSeconds(_lifeTime);
        TriggerExplosion();
    }

    private void TriggerExplosion()
    {
        this.gameObject.SetActive(false);

        if (_explosionEffectPrefab != null)
        {
            GameObject effect = ObjectPooler.Instance.GetObject(_explosionEffectPrefab);
            effect.transform.position = this.transform.position;
            effect.transform.localScale = Vector3.one * 1.2f;
            effect.SetActive(true);

            EffectManager.Instance.TriggerExplosion(effect, _dmg, _explosionRadius, _targetMask);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        this.Boom(collision.gameObject);
    }

    private void OnDrawGizmosSelected()
    {

        Gizmos.color = new Color(1f, 0f, 0f, 0.2f);
        Gizmos.DrawSphere(transform.position, _explosionRadius);

        Gizmos.color = new Color(1f, 0f, 0f, 0.8f);
        Gizmos.DrawWireSphere(transform.position, _explosionRadius);
    }
}
