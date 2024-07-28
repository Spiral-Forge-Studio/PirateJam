using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurtlePlatformScript : MonoBehaviour
{

    private Vector3 previousPosition;
    private Transform player;
    private EntityMovement EntityMovement;

    public bool playerOnPlatform;

    private void Start()
    {
        EntityMovement = GetComponentInParent<EntityMovement>();
        playerOnPlatform = false;
    }

    private void Update()
    {
        //previousPosition = transform.position;

        //if (playerOnPlatform && player != null)
        //{
        //    //player.SetParent(this.transform);
        //    //player.transform.position = new Vector3(transform.position.x, player.position.y, player.position.z);
            
        //    Debug.Log("movdelta: " + EntityMovement.GetMovementDelta());
        //    Debug.Log("onplatform " + (transform.position - previousPosition));
        //}
    }

    private void FixedUpdate()
    {
        previousPosition = transform.position;
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            //player = collision.gameObject.GetComponent<Transform>();
            collision.gameObject.transform.position += EntityMovement.GetMovementDelta();
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            //player.SetParent(null);
            collision.gameObject.GetComponent<Rigidbody2D>().isKinematic = false;
            player = null;
            //collision.gameObject.transform.SetParent(transform);
            playerOnPlatform = false;
            
        }
    }
}
