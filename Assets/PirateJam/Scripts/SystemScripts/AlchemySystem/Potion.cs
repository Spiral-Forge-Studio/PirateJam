using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Potion : MonoBehaviour
{
    [Header("Potion Settings")]
    public GameObject explosionPrefab;
    public float baseCatalystTime;
    private List<Effect> effectsList;

    private float areaOfEffect;
    private float catalyst;

    private float burstMultiplier;
    private float quickenMulitplier;
    private float shrinkMultiplier;

    private Rigidbody2D rb;

    private GameObject effect;
    private Coroutine catalystRoutine;

    // Trajectory Variables and References
    [Header("Trajectory Settings")]
    private Vector3 launchDir;
    public Vector2 maxVelocity;  // Adjust as needed for the desired arc
    //public float launchAngle = 60f;  // Adjust as needed for the desired arc
    private float launchSpeed;  // Adjust as needed for the desired arc

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        catalystRoutine = null;
    }

    public void InitializePotion(Vector3 launchDir, float _launchSpeed, 
        List<Effect> _effectsList, 
        Dictionary<Property.EProperty, Property> _propertyDict)
    {
        launchSpeed = _launchSpeed;
        this.launchDir = launchDir;
        effectsList = _effectsList;

        // handle effects
        foreach (Effect effect in effectsList)
        {
            if (effect.effectEnum == Effect.EEffect.Burst)
            {
                burstMultiplier += effect.value;
            }            
            else if (effect.effectEnum == Effect.EEffect.Quicken)
            {
                quickenMulitplier += effect.value;
            }
            else if (effect.effectEnum == Effect.EEffect.Shrink)
            {
                shrinkMultiplier += effect.value;
            }
        }

        // handle properties
        if (_propertyDict.ContainsKey(Property.EProperty.AoE))
        {
            areaOfEffect = 1 + _propertyDict[Property.EProperty.AoE].value/100;
        }
        if (_propertyDict.ContainsKey(Property.EProperty.Catalyst))
        {
            catalyst = 1+_propertyDict[Property.EProperty.Catalyst].value/100;
        }

        // Calculate the initial velocity required to reach the target
        CalculateLaunchVelocity();
        
        if (catalystRoutine == null)
        {
            catalystRoutine = StartCoroutine(CatalystTimerRoutine());
        }
    }

    /*
    private void CalculateLaunchVelocity()
    {
        Vector3 startPosition = transform.position;

        //Debug.Log("mouse pos: " + target);

        Vector3 displacement = target - startPosition;

        float angle;

        angle = launchAngle * Mathf.Deg2Rad;

        float displacementX = displacement.x;
        float displacementY = displacement.y;

        float dirSign = Mathf.Sign(displacementX);

        float pA = Mathf.Abs(Physics2D.gravity.y) * displacementX * displacementX;
        float pB = (1 + Mathf.Pow(Mathf.Tan(angle),2));
        float pC = Mathf.Clamp(Mathf.Abs(displacementX * Mathf.Tan(angle)) - displacementY, 0f, Mathf.Infinity);
        float pD = 2*Mathf.Pow(Mathf.Cos(angle),2);

        //Debug.Log(pA + ", " + pB + ", " + pC + ", " + pD);

        float velocityX = Mathf.Sqrt(pA / (pB * pC * pD));

        //float velocityX = Mathf.Sqrt(Mathf.Abs(Physics2D.gravity.y) * displacementX * displacementX /
        //    Mathf.Abs(2 * (displacementY - Mathf.Tan(angle) * displacementX))*(Mathf.Pow(Mathf.Cos(angle),2)));

        //Debug.Log("Vx: " + velocityX);

        float velocityY = velocityX * Mathf.Tan(angle);

       // Debug.Log("Vy: " + velocityY);

        Vector2 velocity = new Vector2(Mathf.Clamp(dirSign * velocityX, -maxVelocity.x, maxVelocity.x), 
            Mathf.Clamp(velocityY, -maxVelocity.y, maxVelocity.y));

        //Debug.Log("resulting velocity: " + velocity);
        rb.velocity = velocity;
    }
    */

    private void CalculateLaunchVelocity()
    {
        rb.velocity = launchDir * launchSpeed;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //Debug.Log("hit: " + collision.gameObject.name);


    }

    private IEnumerator CatalystTimerRoutine()
    {
        float totalDelay = baseCatalystTime * catalyst;
        Debug.Log("delay: " + totalDelay);
        yield return new WaitForSeconds(totalDelay);

        GameObject instantiatedExplosion = Instantiate(explosionPrefab, transform.position, transform.rotation);

        instantiatedExplosion.GetComponent<EffectExplosion>().ExplosionRadius *= areaOfEffect;
        instantiatedExplosion.GetComponent<EffectExplosion>().ExplosionForceMulti *= burstMultiplier;
        instantiatedExplosion.GetComponent<EffectExplosion>().Explode();

        Destroy(gameObject);
    }
}
