using UnityEngine;

public class SlimeMovement : MonoBehaviour
{
    [Header("SlimeTile Settings")]
    private MapManager mapManager;
    public Transform groundCheck; // Reference to the BouncyTile

    private void Awake()
    {
        mapManager = FindObjectOfType<MapManager>();
    }


    private void Update()
    {
        mapManager.ChangeTileToSlimeTile(groundCheck.position);
    }
}
