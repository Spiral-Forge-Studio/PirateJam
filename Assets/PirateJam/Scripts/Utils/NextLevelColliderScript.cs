using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NextLevelColliderScript : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            SceneController.instance.LoadNextScene();
        }
    }
}
