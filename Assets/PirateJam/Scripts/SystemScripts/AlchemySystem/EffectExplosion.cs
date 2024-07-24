using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectExplosion : MonoBehaviour
{
    [Header("Burst Explosion Settings")]
    public float BaseExplosionValue;

    Collider2D[] inExplosionRadius = null;
    List<Effect> effectsList;
    private float explosionRadius;
    private float burstPercent;
    private float quickenPercent;
    private float shrinkPercent;


    public void ApplyAreaOfEffectProperty(float areaOfEffect)
    {
        explosionRadius = areaOfEffect;
        transform.localScale = new Vector3(2 * explosionRadius, 2 * explosionRadius, 2 * explosionRadius);
    }

    public void InitializeExplosion(List<Effect> _effectsList)
    {
        effectsList = _effectsList;

        burstPercent = 0;
        quickenPercent = 0;
        shrinkPercent = 0;

        foreach (Effect effect in effectsList)
        {
            if (effect.effectEnum == Effect.EEffect.Burst)
            {
                burstPercent += effect.percentValue;
            }
        }
    }

    public void Explode()
    {
        inExplosionRadius = Physics2D.OverlapCircleAll(transform.position, explosionRadius);

        foreach (Collider2D hitObject in inExplosionRadius)
        {
            Rigidbody2D hitObjectRB = hitObject.GetComponent<Rigidbody2D>();

            if (hitObjectRB != null)
            {
                if (hitObjectRB.gameObject.CompareTag("Ground"))
                {
                    continue;
                }

                Vector2 distanceVec = hitObject.transform.position - transform.position;

                RaycastHit2D ray = Physics2D.Raycast(transform.position, distanceVec.normalized, explosionRadius);

                Debug.DrawLine(transform.position, hitObjectRB.position, Color.red, 2f);

                if (ray.collider != null)
                {
                    if (distanceVec.magnitude > 0 && !ray.collider.gameObject.CompareTag("Ground"))
                    {
                        EntityEffectsHandler effectsHandler =  hitObjectRB.GetComponent<EntityEffectsHandler>();

                        ApplyEffects(effectsHandler);

                        if (burstPercent > 0)
                        {
                            if (hitObjectRB.gameObject.CompareTag("Player"))
                            {
                                hitObjectRB.gameObject.GetComponent<Movement>().hitByExplosion = true;
                            }

                            float burstForce = BaseExplosionValue * (1+burstPercent/100);
                            hitObjectRB.AddForce(distanceVec.normalized * burstForce);
                        }
                        
                    }
                }
            }
        }

        Destroy(gameObject, 0.04f);
    }

    public void ApplyEffects(EntityEffectsHandler effectsHandler)
    {
        // handle effects
        float quickenDuration = 0;
        float shrinkDuration = 0;

        quickenPercent = 0;
        shrinkPercent = 0;

        foreach (Effect effect in effectsList)
        {
            if (effect.effectEnum == Effect.EEffect.Quicken)
            {
                quickenPercent += effect.percentValue;
                quickenDuration = effect.duration;
            }
            else if (effect.effectEnum == Effect.EEffect.Shrink)
            {
                shrinkPercent += effect.percentValue;
                shrinkDuration = effect.duration;
            }
        }

        if (quickenPercent > 0)
        {
            effectsHandler.ApplyQuicken(quickenPercent, quickenDuration);
        }
        if (shrinkPercent > 0)
        {
            effectsHandler.ApplyShrink(shrinkPercent, shrinkDuration);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }

}
