using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallCheckScript : MonoBehaviour
{
    public Movement movement;
    public bool wallCheckRight;
    public bool wallCheckLeft;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Debug.Log(collision.gameObject.name);

        if (collision.gameObject.CompareTag("Ground") || collision.gameObject.CompareTag("Turtle"))
        {
            if (wallCheckRight)
            {
                movement.hittingWallRight = true;
            }
            else
            {
                movement.hittingWallLeft = true;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Ground") || collision.gameObject.CompareTag("Turtle"))
        {
            if (wallCheckRight)
            {
                movement.hittingWallRight = false;
            }
            else
            {
                movement.hittingWallLeft = false;
            }
        }
    }
}
