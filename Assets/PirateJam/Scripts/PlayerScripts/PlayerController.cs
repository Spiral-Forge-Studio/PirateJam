using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    PlayerControls playerControls;
    private Movement movement;

    [Header("Property and Effect Adjusting")]
    public PotionManager potionHandler;
    public float adjustmentValue;
    public Property.EProperty activePropertyToAdjust;

    private float mouseScrollY;


    private void Awake()
    {
        playerControls = new PlayerControls();

        playerControls.Player.Adjust.performed += x => mouseScrollY = x.ReadValue<float>();
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        potionHandler.AdjustPropertyByStep(mouseScrollY);
    }

    #region -- Enable/Disable -- 
    private void OnEnable()
    {
        playerControls.Enable();
    }

    private void OnDisable()
    {
        playerControls.Disable();
    }
    #endregion

    public void ToggleAoEProperty(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (potionHandler.activePropertyForAdjustment == Property.EProperty.AoE)
            {
                potionHandler.activePropertyForAdjustment = Property.EProperty.None;
            }
            else
            {
                potionHandler.activePropertyForAdjustment = Property.EProperty.AoE;
            }
        }
    }

    public void ToggleCatalystProperty(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (potionHandler.activePropertyForAdjustment == Property.EProperty.Catalyst)
            {
                potionHandler.activePropertyForAdjustment = Property.EProperty.None;
            }
            else
            {
                potionHandler.activePropertyForAdjustment = Property.EProperty.Catalyst;
            }
        }
    }

    public void ThrowPotion(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            potionHandler.ThrowPotionObject();
        }
    }
    
}
