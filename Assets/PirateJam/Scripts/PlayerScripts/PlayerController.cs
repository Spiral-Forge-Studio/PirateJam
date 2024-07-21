using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    PlayerControls playerControls;
    private Movement movement;
    public PotionManager potionHandler;
    public float mouseScrollY;
    public float adjustmentValue;


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
        if (mouseScrollY > 0)
        {
            adjustmentValue += 1;
            Debug.Log("Scrolled Up");
        }        
        if (mouseScrollY < 0)
        {
            adjustmentValue -= 1;
            Debug.Log("Scrolled Down");
        }

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

    public void ThrowPotion(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            potionHandler.ThrowPotion();
        }
    }
    #endregion
}
