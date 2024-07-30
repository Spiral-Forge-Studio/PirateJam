using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEditor.Timeline.TimelinePlaybackControls;

public class PlayerController : MonoBehaviour
{
    PlayerControls playerControls;
    private Movement movement;

    [Header("[DEBUG]")]
    [SerializeField] private PotionManager potionManager;
    [SerializeField] private float mouseScrollY;


    private void Awake()
    {
        potionManager = GetComponentInChildren<PotionManager>();

        playerControls = new PlayerControls();
        playerControls.Player.Adjust.performed += x => mouseScrollY = x.ReadValue<float>();
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        potionManager.AdjustPropertyByStep(mouseScrollY);
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

    #region -- Potion Effects -- 
    public void QueueBurstEffect(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            potionManager.AddEffect(Effect.EEffect.Burst);
        }
    }
    public void QueueQuickenEffect(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            potionManager.AddEffect(Effect.EEffect.Quicken);
        }
    }
    public void QueueShrinkEffect(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            potionManager.AddEffect(Effect.EEffect.Shrink);
        }
    }

    #endregion

    #region -- Property and Potion Throw --
    public void ToggleAoEProperty(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (potionManager.activePropertyForAdjustment == Property.EProperty.AoE)
            {
                potionManager.activePropertyForAdjustment = Property.EProperty.None;
            }
            else
            {
                potionManager.activePropertyForAdjustment = Property.EProperty.AoE;
            }
        }
    }

    public void ToggleCatalystProperty(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (potionManager.activePropertyForAdjustment == Property.EProperty.Catalyst)
            {
                potionManager.activePropertyForAdjustment = Property.EProperty.None;
            }
            else
            {
                potionManager.activePropertyForAdjustment = Property.EProperty.Catalyst;
            }
        }
    }

    public void ThrowPotion(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            potionManager.ThrowPotionObject();
        }
    }

    public void DiscardPotions(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            potionManager.FlushPotionGauge();
        }
    }
    #endregion
}
