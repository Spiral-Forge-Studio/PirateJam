using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Effect
{
    public Effect(EEffect effectEnum, float percentValue, float duration)
    {
        this.effectEnum = effectEnum;
        this.percentValue = percentValue;
        this.duration = duration;
    }

    public enum EEffect
    {
        Burst,
        Quicken,
        Shrink,
        Empty
    }

    public EEffect effectEnum;
    public float percentValue;
    public float duration;
}

public class Property
{
    public EProperty propertyEnum;
    public float baseValue;
    public float basePercent;
    public float currentPercent;
    public float stepPercentage;
    public float maxPercent;


    public enum EProperty
    {
        AoE,
        Catalyst,
        None
    }

    public Property(EProperty propertyEnum, float baseValue, float basePercent, float stepPercentage, float maxPercent)
    {
        this.propertyEnum = propertyEnum;
        this.baseValue = baseValue;
        this.basePercent = basePercent;
        this.stepPercentage = stepPercentage;
        this.maxPercent = maxPercent;
        currentPercent = basePercent;
    }

    public void AdjustProperty(float step)
    {
        currentPercent = Mathf.Clamp(currentPercent + stepPercentage * Mathf.Sign(step), 0, maxPercent);
    }

    public float GetCurrentPercentAsDecimal()
    {
        return currentPercent / maxPercent;
    }

    public float GetFinalValueAsDecimal()
    {
        return baseValue*(currentPercent/100);
    }

}

public class PotionManager : MonoBehaviour
{
    [Header("[DEBUG, READONLY] Potion Gauge Info")]
    [SerializeField] public List<Effect> activeEffectsList = new List<Effect>();
    [SerializeField] public Dictionary<Property.EProperty,Property> propertyDict = new Dictionary<Property.EProperty, Property>();
    [SerializeField] public Property.EProperty activePropertyForAdjustment;
    [SerializeField] public UnityEvent OnPotionEffectsUpdated;

    [Header("Potion Gauge Settings")]
    public int maxPotionSlots;

    [Header("Potion Throwing Settings")]
    public float maxThrowHeight;
    public float throwHeightOffest;
    public float maxThrowDistance;
    public GameObject potionPrefab;

    [Header("[DEBUG, READONLY] Potion Throwing Info")]
    [SerializeField] private Vector3 launchDir;
    [SerializeField] private float throwHeight;
    [SerializeField] private float Px;
    [SerializeField] private float Py;
    [SerializeField] private float TermVxA;
    [SerializeField] private float TermVyA;


    [Header("Potion Effect Settings")]
    public float burstBasePercent;
    public float burstBaseDuration;
    public float quickenBasePercent;
    public float quickenBaseDuration;
    public float shrinkBasePercent;
    public float shrinkBaseDuration;

    [Header("Potion Property Settings")]
    public float baseAreaOfEffect; // in units of space (idk what unity calls them)
    public float baseAreaOfEffectPercent;
    public float stepAreaOfEffectPercent;
    public float maxAreaOfEffectPercent;
    public float baseCatalyst; // in seconds
    public float baseCatalystPercent;
    public float stepCatalystPercent;
    public float maxCatalystPercent;

    [Header("Line Renderer settings")]
    private int linePoints;  // Adjust as needed for the desired arc
    public float timeIntervalInPoints;
    private LineRenderer lineRenderer;

    [Header("[DEBUG, READONLY] UI References")]
    public float AoEPercentage;
    public float CatalystPercentage;


    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
        OnPotionEffectsUpdated.AddListener(FindObjectOfType<UIMainScript>().UpdatePotionGauge);
    }
    // Start is called before the first frame update
    void Start()
    {
        propertyDict.Add(Property.EProperty.AoE, new Property(
            Property.EProperty.AoE,
            baseAreaOfEffect,
            baseAreaOfEffectPercent, 
            stepAreaOfEffectPercent, 
            maxAreaOfEffectPercent));

        propertyDict.Add(Property.EProperty.Catalyst, new Property(
            Property.EProperty.Catalyst,
            baseCatalyst,
            baseCatalystPercent, 
            stepCatalystPercent, 
            maxCatalystPercent));
        
        FlushPotionGauge();

        lineRenderer.enabled = true;
    }

    // Update is called once per frame
    void Update()
    {
        // update UI references to property gauges
        AoEPercentage = propertyDict[Property.EProperty.AoE].GetCurrentPercentAsDecimal();
        CatalystPercentage = propertyDict[Property.EProperty.Catalyst].GetCurrentPercentAsDecimal();
    }

    private void FixedUpdate()
    {
        launchDir = CalculateThrowVelocity();
        DrawTrajectory();
    }

    public void AddEffect(Effect.EEffect effectToAdd)
    {
        float percentValue;
        float duration;

        if (effectToAdd == Effect.EEffect.Burst)
        {
            percentValue = burstBasePercent;
            duration = burstBaseDuration;
        }
        else if (effectToAdd == Effect.EEffect.Quicken)
        {
            percentValue = quickenBasePercent;
            duration = quickenBaseDuration;
        }
        else if (effectToAdd == Effect.EEffect.Shrink)
        {
            percentValue = shrinkBasePercent;
            duration = shrinkBaseDuration;
        }
        else
        {
            Debug.Log("empty effect");
            return;
        }

        Effect newEffect = new Effect(effectToAdd, percentValue, duration);

        activeEffectsList.Add(newEffect);

        if (activeEffectsList.Count > maxPotionSlots)
        {
            activeEffectsList.RemoveAt(0);
        }

        OnPotionEffectsUpdated?.Invoke();
    }

    public void AdjustPropertyByStep(float scrollStep)
    {
        if (activePropertyForAdjustment == Property.EProperty.None || scrollStep == 0)
        {
            return;
        }
        else
        {
            Property activeProperty = propertyDict[activePropertyForAdjustment];
            activeProperty.AdjustProperty(scrollStep);
        }
    }

    public void FlushPotionGauge()
    {
        activeEffectsList.Clear();

        for (int i = 0; i < maxPotionSlots; ++i)
        {
            activeEffectsList.Add(new Effect(Effect.EEffect.Empty, 0, 0));
        }

        OnPotionEffectsUpdated?.Invoke();
    }

    public Vector3 CalculateThrowVelocity()
    {
        Px = Camera.main.ScreenToWorldPoint(Input.mousePosition).x - transform.position.x;
        Px = Mathf.Clamp(Px, -maxThrowDistance, maxThrowDistance);

        Py = Camera.main.ScreenToWorldPoint(Input.mousePosition).y - transform.position.y;
        Py = Mathf.Clamp(Py, -(maxThrowHeight - throwHeightOffest), (maxThrowHeight - throwHeightOffest));
        throwHeight = Mathf.Clamp(Py + throwHeightOffest, 0, maxThrowHeight);

        float g = Physics2D.gravity.y;

        TermVxA = Mathf.Sqrt((2 * throwHeight) / Mathf.Abs(Physics2D.gravity.y));
        TermVyA = Mathf.Sqrt(-2 * Physics2D.gravity.y * throwHeight);

        float Vx = Px / (TermVxA +  Mathf.Sqrt(Mathf.Abs((2/g) * (Py - throwHeight))));
        float Vy = TermVyA;
        Vector3 velocityY = Vector2.up * Vy;
        Vector3 velocityX = new Vector3(Vx, 0, 0);

        return velocityX + velocityY;
    }

    public void DrawTrajectory()
    {
        linePoints = Mathf.RoundToInt(propertyDict[Property.EProperty.Catalyst].GetFinalValueAsDecimal()*100);

        Vector3 origin = transform.position;
        Vector3 startVelocity = 1 * launchDir;

        lineRenderer.positionCount = linePoints;

        float time = 0;

        for (int i = 0; i < linePoints; i++)
        {
            var x = (startVelocity.x * time) + (Physics.gravity.x / 2 * time * time);
            var y = (startVelocity.y * time) + (Physics.gravity.y / 2 * time * time);

            Vector3 point = new Vector3(x, y, 0);
            lineRenderer.SetPosition(i, origin + point);
            time += timeIntervalInPoints;
        }
    }

    public void ThrowPotionObject()
    {
        GameObject potionObject = Instantiate(potionPrefab, transform.position, transform.rotation);
        Potion potion = potionObject.GetComponent<Potion>();

        potion.InitializePotion(launchDir, 1, activeEffectsList, propertyDict);

        //FlushPotionGauge();
    }
}
