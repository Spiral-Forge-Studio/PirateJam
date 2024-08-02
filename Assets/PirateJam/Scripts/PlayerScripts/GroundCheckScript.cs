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

            if (collision.gameObject.name == "SlimeTiles")
            {
                movement.takeFallDamage = false;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (movement != null)
        {
            movement.isGrounded = true;

            if (collision.gameObject.name == "SlimeTiles")
            {
                movement.takeFallDamage = false;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (movement != null)
        {
            movement.isGrounded = false;
        }

        if (collision.gameObject.name == "SlimeTiles")
        {
            movement.takeFallDamage = true;
        }
    }
}
