using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIMainScript : MonoBehaviour
{

    [Header("Alchemy System References")]
    public PotionManager potionManager;

    [Header("UI Child References")]
    public GameObject AoEActiveSquare;
    public Image AoEGauge;
    public GameObject CatalystctiveSquare;
    public Image CatalystGauge;

    // Start is called before the first frame update
    void Awake()
    {
        potionManager = FindObjectOfType<PotionManager>();
        AoEActiveSquare.SetActive(false);
        CatalystctiveSquare.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        UpdatePropertyActiveSquare();
        UpdatePropertyGauge();
    }

    public void UpdatePropertyActiveSquare()
    {
        if (potionManager.activePropertyForAdjustment == Property.EProperty.AoE)
        {
            AoEActiveSquare.SetActive(true);
            CatalystctiveSquare.SetActive(false);
        }
        else if (potionManager.activePropertyForAdjustment == Property.EProperty.Catalyst)
        {
            AoEActiveSquare.SetActive(false);
            CatalystctiveSquare.SetActive(true);
        }
        else
        {
            AoEActiveSquare.SetActive(false);
            CatalystctiveSquare.SetActive(false);
        }
    }

    public void UpdatePropertyGauge()
    {
        AoEGauge.fillAmount = potionManager.AoEPercentage / 100;
        CatalystGauge.fillAmount = potionManager.CatalystPercentage / 100;
    }

}
