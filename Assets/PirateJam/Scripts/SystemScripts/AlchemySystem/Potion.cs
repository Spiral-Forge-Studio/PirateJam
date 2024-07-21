using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Potion : MonoBehaviour
{
    private Vector3 target;
    private List<PotionManager.EEffect> effectsList;

    private Rigidbody2D rb;
    public float launchAngle = 60f;  // Adjust as needed for the desired arc
    public float launchForce;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        // No need to manually update the position, physics will handle it
    }

    public void InitializePotion(Vector3 _target, List<PotionManager.EEffect> _effectsList)
    {
        target = _target;
        effectsList = _effectsList;

        // Calculate the initial velocity required to reach the target
        CalculateLaunchVelocity();
    }

    private void CalculateLaunchVelocity()
    {
        // Get the starting position
        Vector3 startPosition = transform.position;

        Debug.Log("mouse pos: " + target);

        

        // Calculate the displacement to the target
        Vector3 displacement = target - startPosition;
        
        // Calculate the angle in radians
        float angle;

        angle = launchAngle * Mathf.Deg2Rad;

        // Calculate the velocity needed
        float displacementX = displacement.x;
        float displacementY = displacement.y;

        float dirSign = Mathf.Sign(displacementX);

        float pA = Mathf.Abs(Physics2D.gravity.y) * displacementX * displacementX;
        float pB = (1 + Mathf.Pow(Mathf.Tan(angle),2));
        float pC = Mathf.Clamp(Mathf.Abs(displacementX * Mathf.Tan(angle)) - displacementY, 0.5f, Mathf.Infinity);
        float pD = 2*Mathf.Pow(Mathf.Cos(angle),2);

        Debug.Log(pA + ", " + pB + ", " + pC + ", " + pD);

        float velocityX = Mathf.Sqrt(pA / (pB * pC * pD));

        //float velocityX = Mathf.Sqrt(Mathf.Abs(Physics2D.gravity.y) * displacementX * displacementX /
        //    Mathf.Abs(2 * (displacementY - Mathf.Tan(angle) * displacementX))*(Mathf.Pow(Mathf.Cos(angle),2)));

        Debug.Log("Vx: " + velocityX);

        float velocityY = velocityX * Mathf.Tan(angle);

        Debug.Log("Vy: " + velocityY);

        // Apply the velocity to the Rigidbody
        Vector2 velocity = new Vector2( dirSign * velocityX, velocityY);
        rb.velocity = velocity;
    }

    private void OnReachTarget()
    {
        // Handle what happens when the projectile reaches the target
        // For example, apply effects, play an animation, etc.
        Debug.Log("Potion reached the target!");

        // Destroy the potion object (optional)
        Destroy(gameObject);
    }
}
