using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityEffectsHandler : MonoBehaviour
{
    public Movement movement;
    public EntityMovement entityMovement;
    // public EntityMovement entityMovement; --Brandon--
    //      - note: reference to your enemy movement script (make movespeed accessible)

    public bool isPlayer; // make sure to toggle this off when you attach the script

    [Header("[DEBUG, READONLY] Quicken Effect Info")]
    private float quickenEffectTimeHit;
    private float quickenEffectDuration;
    private float quickenAmountPercent;
    private float originalMoveSpeed;
    private bool underQuickenEffect;

    [Header("[DEBUG, READONLY] Shrink Effect Info")]
    private float shrinkEffectTimeHit;
    private float shrinkEffectDuration;
    private float shrinkAmountPercent;
    private Vector3 originalScale;
    private bool underShrinkEffect;

    // Update is called once per frame
    private void Awake()
    {
        underQuickenEffect = false;
        underShrinkEffect = false;

        // set original values
        if (isPlayer)
        {
            movement = GetComponent<Movement>();
            originalMoveSpeed = movement.maxMoveSpeed;
            originalScale = transform.localScale;
        }
        else
        {
            //originalMoveSpeed = entityMovement.moveSpeed; --Brandon--
            entityMovement = GetComponent<EntityMovement>();
            originalMoveSpeed = entityMovement.moveSpeed;
            originalScale = transform.localScale;
        }


    }
    void Update()
    {
        if (underQuickenEffect)
        {
            CheckQuickenEffectTimer();
        }
        if (underShrinkEffect)
        {
            CheckShrinkEffectTimer();
        }
    }

    public void ApplyQuicken(float amountPercent, float effectDuration)
    {
        quickenEffectTimeHit = Time.time;
        quickenAmountPercent = amountPercent;
        quickenEffectDuration = effectDuration;

        if (isPlayer)
        {
            movement.maxMoveSpeed = originalMoveSpeed;
            movement.maxMoveSpeed *= (1+quickenAmountPercent/100);
        }
        else
        {
            entityMovement.moveSpeed = originalMoveSpeed;
            entityMovement.moveSpeed *= (1+quickenAmountPercent/100);
        }

        underQuickenEffect = true;
    }

    public void ApplyShrink(float amountPercent, float effectDuration)
    {
        shrinkEffectTimeHit = Time.time;
        shrinkAmountPercent = amountPercent;
        shrinkEffectDuration = effectDuration;

        if (isPlayer)
        {
            transform.localScale = originalScale;
            transform.localScale *= (1 - shrinkAmountPercent/100);
        }
        else
        {
            transform.localScale = originalScale;
            transform.localScale *= (1 - shrinkAmountPercent/100);
        }

        underShrinkEffect = true;
    }

    public void CheckQuickenEffectTimer()
    {
        if (Time.time - quickenEffectTimeHit > quickenEffectDuration)
        {
            if (isPlayer)
            {
                movement.maxMoveSpeed = originalMoveSpeed;
            }
            else
            {
                entityMovement.moveSpeed = originalMoveSpeed;
            }
            underQuickenEffect = false;
        }
    }

    public void CheckShrinkEffectTimer()
    {
        if (Time.time - shrinkEffectTimeHit > shrinkEffectDuration)
        {
            if (isPlayer)
            {
                transform.localScale = originalScale;
            }
            else
            {
                transform.localScale = originalScale;
            }
            underShrinkEffect = false;
        }
    }


}
