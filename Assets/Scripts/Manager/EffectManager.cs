using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectManager : Singleton<EffectManager>
{
    public IEnumerator Burning(GameObject effect, float dmg, float range, float time, float speed,LayerMask mask)
    {
        float timer = 0f;
        while(timer < time)
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

    public void TriggerExplosion(GameObject effect,float dmg, float range, LayerMask mask)
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(effect.transform.position, range, mask);
        foreach (Collider2D hit in hits)
        {
           hit.GetComponent<IGetHit>()?.GetHit(dmg);
        }
        //effect tự động disable
    }
}
