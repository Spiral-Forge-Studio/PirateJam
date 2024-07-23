using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

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
    public Property(EProperty propertyEnum, float baseValue, float stepPercent)
    {
        this.propertyEnum = propertyEnum;
        this.baseValue = baseValue;
        this.stepPercentage = stepPercent;
        value = baseValue;
    }

    public enum EProperty
    {
        AoE,
        Catalyst,
        None
    }

    public EProperty propertyEnum;
    public float baseValue;
    public float value;
    public float stepPercentage;
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
    public Vector2 throwForce;
    public GameObject potionPrefab;

    [Header("Potion Effect Parameters")]
    public float burstBaseMult;
    public float quickenBaseMult;
    public float shrinkBaseMult;

    [Header("Potion Property Parameters")]
    public float baseAreaOfEffect;
    public float stepPercent_areaOfEffect;
    public float baseCatalyst;
    public float stepPercent_catalyst;

    [Header("For UI Reference")]
    public float AoEPercentage;
    public float CatalystPercentage;


    private void Awake()
    {
        OnPotionEffectsUpdated.AddListener(FindObjectOfType<UIMainScript>().UpdatePotionGauge);
    }
    // Start is called before the first frame update
    void Start()
    {
        propertyDict.Add(Property.EProperty.AoE, new Property(Property.EProperty.AoE, baseAreaOfEffect, stepPercent_areaOfEffect));
        propertyDict.Add(Property.EProperty.Catalyst, new Property(Property.EProperty.Catalyst, baseCatalyst, stepPercent_catalyst));
        FlushPotionGauge();
    }

    // Update is called once per frame
    void Update()
    {
        AoEPercentage = propertyDict[Property.EProperty.AoE].value;
        CatalystPercentage = propertyDict[Property.EProperty.Catalyst].value;
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
            activeProperty.value = Mathf.Clamp(activeProperty.value + activeProperty.stepPercentage*Mathf.Sign(scrollStep),0, 100);
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



    public void ThrowPotionObject()
    {
        Vector3 targetPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        GameObject potionObject = Instantiate(potionPrefab, transform.position, transform.rotation);
        Potion potion = potionObject.GetComponent<Potion>();

        potion.InitializePotion(targetPosition, activeEffectsList, propertyDict);

        //FlushPotionGauge();
    }
}
