using UnityEngine;

public class SlimeMovement : MonoBehaviour
{
    [Header("SlimeTile Settings")]
    private MapManager mapManager;
    public Transform groundCheck; // Reference to the BouncyTile

    [Header("Slime Movement Settings")]
    public float moveSpeed;
    public float patrolInterval; // Time in seconds for each direction change
    private bool movingRight = true;
    private float lastPatrolTime;

    private void Awake()
    {
        mapManager = FindObjectOfType<MapManager>();
    }

    private void Start()
    {
        lastPatrolTime = Time.time;
    }

    private void Update()
    {
        mapManager.ChangeTileToSlimeTile(groundCheck.position);
    }

    private void FixedUpdate()
    {
        Patrol();
    }

    void Patrol()
    {
        // Check if it's time to switch direction
        if (Time.time - lastPatrolTime >= patrolInterval)
        {
            movingRight = !movingRight;
            lastPatrolTime = Time.time;
            Flip();
        }

        // Move the slime
        Vector2 targetVelocity = new Vector2((movingRight ? 1 : -1) * moveSpeed * Time.fixedDeltaTime, 0);
        transform.Translate(targetVelocity);
    }

    void Flip()
    {
        Vector3 scaler = transform.localScale;
        scaler.x *= -1;
        transform.localScale = scaler;
    }
}
