using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "New Bouncy Tile", menuName = "Tiles/Bouncy Tile")]
public class BouncyTile : Tile
{
    public float bounceForce = 10f; // The force applied to bouncing objects

    public override bool StartUp(Vector3Int position, ITilemap tilemap, GameObject go)
    {
        base.StartUp(position, tilemap, go);
        if (go != null)
        {
            go.AddComponent<BouncyTileBehavior>().bounceForce = bounceForce;
            
        }

        return false;
    }
}

public class BouncyTileBehavior : MonoBehaviour
{
    public float bounceForce;

    private void Update()
    {
        Debug.Log("alive?");
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("Colliding with: " + collision.gameObject.name);

        Rigidbody2D rb = collision.collider.GetComponent<Rigidbody2D>();

        if (rb != null)
        {
            // Apply an upward force to the object that collided with the bouncy tile
            rb.velocity = new Vector2(rb.velocity.x, bounceForce);
        }
    }
}

