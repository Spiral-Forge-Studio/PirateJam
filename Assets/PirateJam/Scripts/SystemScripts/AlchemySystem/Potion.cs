using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Potion : MonoBehaviour
{
    [Header("[DEBUG, READONLY] Potion Info")]
    public GameObject explosionPrefab;
    private List<Effect> effectsList;
    private float areaOfEffect;
    private float catalyst;

    private float burstMultiplier;
    private float quickenMulitplier;
    private float shrinkMultiplier;

    [Header("Timer settings")]
    private float timeAtLaunch;
    private bool startTimer;

    private GameObject effect;
    private Coroutine catalystRoutine;

    // Trajectory Variables and References
    [Header("Trajectory Settings")]
    private Rigidbody2D rb;
    private Vector3 launchDir;
    private float launchSpeed;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        catalystRoutine = null;
        startTimer = false;
    }

    private void Start()
    {
        timeAtLaunch = Time.time;
    }

    private void Update()
    {
        if (startTimer)
        {
            CatalystTimer();
        }
    }

    public void InitializePotion(Vector3 launchDir, float _launchSpeed, 
        List<Effect> _effectsList, 
        Dictionary<Property.EProperty, Property> _propertyDict)
    {
        launchSpeed = _launchSpeed;
        this.launchDir = launchDir;
        effectsList = _effectsList;

        // handle properties
        if (_propertyDict.ContainsKey(Property.EProperty.AoE))
        {
            areaOfEffect = _propertyDict[Property.EProperty.AoE].GetFinalValueAsDecimal();
        }
        if (_propertyDict.ContainsKey(Property.EProperty.Catalyst))
        {
            catalyst = _propertyDict[Property.EProperty.Catalyst].GetFinalValueAsDecimal();
        }

        // Calculate the initial velocity required to reach the target
        CalculateLaunchVelocity();

        startTimer = true;
    }

    private void CalculateLaunchVelocity()
    {
        rb.velocity = launchDir * launchSpeed;
    }

    private void CatalystTimer()
    {
        if (Time.time - timeAtLaunch > catalyst)
        {
            //Debug.Log("Catalyst: " + catalyst + ", AoE: " + areaOfEffect);

            GameObject instantiatedExplosion = Instantiate(explosionPrefab, transform.position, transform.rotation);

            instantiatedExplosion.GetComponent<EffectExplosion>().ApplyAreaOfEffectProperty(areaOfEffect) ;
            instantiatedExplosion.GetComponent<EffectExplosion>().InitializeExplosion(effectsList);
            instantiatedExplosion.GetComponent<EffectExplosion>().Explode();

            Destroy(gameObject);
        }
    }
}
