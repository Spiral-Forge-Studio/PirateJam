using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIMainScript : MonoBehaviour
{

    [Header("Alchemy System References")]
    public PotionManager potionManager;
    public Image[] potionSlots;

    [Header("Property Gauge References")]
    public GameObject AoEActiveSquare;
    public Image AoEGauge;
    public GameObject CatalystActiveSquare;
    public Image CatalystGauge;

    // Start is called before the first frame update
    void Awake()
    {
        potionManager = FindObjectOfType<PotionManager>();
        AoEActiveSquare.SetActive(false);
        CatalystActiveSquare.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        UpdatePropertyActiveSquare();
        UpdatePropertyGauge();
    }

    public void UpdatePotionGauge()
    {
        for (int i = 0; i < potionManager.activeEffectsList.Count; i++)
        {
            if (potionManager.activeEffectsList[i].effectEnum == Effect.EEffect.Burst)
            {
                potionSlots[i].color = Color.red;
            }
            else if (potionManager.activeEffectsList[i].effectEnum == Effect.EEffect.Quicken)
            {
                potionSlots[i].color = Color.blue;
            }
            else if (potionManager.activeEffectsList[i].effectEnum == Effect.EEffect.Shrink)
            {
                potionSlots[i].color = Color.green;
            }
            else
            {
                potionSlots[i].color = Color.gray;
            }
        }
    }

    public void UpdatePropertyActiveSquare()
    {
        if (potionManager.activePropertyForAdjustment == Property.EProperty.AoE)
        {
            AoEActiveSquare.SetActive(true);
            CatalystActiveSquare.SetActive(false);
        }
        else if (potionManager.activePropertyForAdjustment == Property.EProperty.Catalyst)
        {
            AoEActiveSquare.SetActive(false);
            CatalystActiveSquare.SetActive(true);
        }
        else
        {
            AoEActiveSquare.SetActive(false);
            CatalystActiveSquare.SetActive(false);
        }
    }

    public void UpdatePropertyGauge()
    {
        AoEGauge.fillAmount = potionManager.AoEPercentage / 100;
        CatalystGauge.fillAmount = potionManager.CatalystPercentage / 100;
    }

}
