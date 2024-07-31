using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LavaScript : MonoBehaviour
{

    public GameObject lavaDeathSound;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("entered lava: " + collision.gameObject.name);
        if (collision.gameObject.CompareTag("Player"))
        {
            Instantiate(lavaDeathSound, collision.ClosestPoint(collision.gameObject.transform.position), Quaternion.identity);
            SceneController.instance.ReloadCurrentScene();
        }
        if (collision.gameObject.CompareTag("Slime"))
        {
            Instantiate(lavaDeathSound, collision.ClosestPoint(collision.gameObject.transform.position), Quaternion.identity);
            Destroy(collision.gameObject);
        }
    }
}
