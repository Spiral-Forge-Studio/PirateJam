using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallCheckScript : MonoBehaviour
{
    public Movement movement;
    public bool wallCheckRight;
    public bool wallCheckLeft;

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
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
        if (collision.gameObject.CompareTag("Ground"))
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