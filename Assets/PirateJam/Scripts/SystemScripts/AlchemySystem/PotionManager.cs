using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PotionManager : MonoBehaviour
{
    public enum EEffect
    {
        Burst,
        Quicken,
        Shrink,
        Empty
    }


    public GameObject potionPrefab;
    public int maxPotionSlots;
    public Vector2 throwForce;
    public List<EEffect> activeEffectsList = new List<EEffect>();

    private float areaOfEffect;
    private float reactionTime;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddEffect(EEffect effectToAdd)
    {
        activeEffectsList.Add(effectToAdd);

        if (activeEffectsList.Count > maxPotionSlots)
        {
            activeEffectsList.RemoveAt(0);
        }
    }

    public void FlushPotionGauge()
    {
        activeEffectsList.Clear();
    }

    public void ThrowPotion()
    {
        Vector3 targetPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        GameObject potionObject = Instantiate(potionPrefab, transform.position, transform.rotation);
        Potion potion = potionObject.GetComponent<Potion>();
        potion.InitializePotion(targetPosition, activeEffectsList);
        FlushPotionGauge();
    }
}
