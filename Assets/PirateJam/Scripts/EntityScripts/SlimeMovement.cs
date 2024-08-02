using UnityEngine;

public class SlimeMovement : MonoBehaviour
{
    [Header("SlimeTile Settings")]
    private MapManager mapManager;
    public Transform groundCheck; // Reference to the BouncyTile
    public float icd;
    private Coroutine routine;

    private void Awake()
    {
        mapManager = FindObjectOfType<MapManager>();
    }

    private void Start()
    {
        // Start the coroutine to change the tile every 0.1 seconds
        StartCoroutine(ChangeTileRoutine());
    }

    private System.Collections.IEnumerator ChangeTileRoutine()
    {
        while (true)
        {
            // Call the method to change the tile
            if (GetComponent<EntityMovement>().IsGrounded())
            {
                mapManager.ChangeTileToSlimeTile(groundCheck.position);
            }

            // Wait for 0.1 seconds before repeating
            yield return new WaitForSeconds(0.1f);
        }
    }
}
