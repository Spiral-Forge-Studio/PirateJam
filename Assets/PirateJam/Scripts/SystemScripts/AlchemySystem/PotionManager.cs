using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using static UnityEngine.GraphicsBuffer;

public class Effect
{
    public Effect(EEffect effectEnum, float value)
    {
        this.effectEnum = effectEnum;
        this.value = value;
    }

    public enum EEffect
    {
        Burst,
        Quicken,
        Shrink,
        Empty
    }

    public EEffect effectEnum;
    public float value;
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
    [Header("Potion Gauge Info")]
    public int maxPotionSlots;
    public List<Effect> activeEffectsList = new List<Effect>();
    public Dictionary<Property.EProperty,Property> propertyDict = new Dictionary<Property.EProperty, Property>();
    public Property.EProperty activePropertyForAdjustment;
    public UnityEvent OnPotionEffectsUpdated;

    [Header("Potion Throwing Related")]
    public float maxThrowHeight;
    public float throwHeightOffest;
    public float maxThrowDistance;
    private float throwHeight;
    private float throwDistance;
    private float Px;
    private float Py;
    private float TermVxA;
    private float TermVxB;
    private float TermVyA;

    public float launchSpeed;
    private Vector3 launchDir;
    public GameObject potionPrefab;

    [Header("Potion Effect Parameters")]
    public float burstBaseMult;
    public float quickenBaseMult;
    public float shrinkBaseMult;

    [Header("Potion Property Parameters")]
    public float baseAreaOfEffect; // in units of space (idk what unity calls them)
    public float baseAreaOfEffectPercent;
    public float stepAreaOfEffectPercent;
    public float maxAreaOfEffectPercent;
    public float baseCatalyst; // in seconds
    public float baseCatalystPercent;
    public float stepCatalystPercent;
    public float maxCatalystPercent;

    [Header("Line Renderer settings")]
    public int linePoints;  // Adjust as needed for the desired arc
    public float timeIntervalInPoints;
    private LineRenderer lineRenderer;

    [Header("For UI Reference")]
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
        float value;

        if (effectToAdd == Effect.EEffect.Burst)
        {
            value = burstBaseMult;
        }
        else if (effectToAdd == Effect.EEffect.Quicken)
        {
            value = quickenBaseMult;
        }
        else if (effectToAdd == Effect.EEffect.Shrink)
        {
            value = shrinkBaseMult;
        }
        else
        {
            Debug.Log("empty effect");
            return;
        }

        Effect newEffect = new Effect(effectToAdd, value);

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
            activeEffectsList.Add(new Effect(Effect.EEffect.Empty, 0));
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
