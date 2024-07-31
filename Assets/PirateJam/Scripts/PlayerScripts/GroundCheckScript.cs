using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundCheckScript : MonoBehaviour
{
    private Movement movement;

    void Awake()
    {
        movement = GetComponentInParent<Movement>();
    }


    private void OnTriggerStay2D(Collider2D collision)
    {
        if (movement != null)
        {
            movement.isGrounded = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (movement != null)
        {
            movement.isGrounded = false;
        }
    }
}
