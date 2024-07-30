using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityWallCheck : MonoBehaviour
{
    private EntityMovement EntityMovement;

    private void Awake()
    {
        EntityMovement = GetComponentInParent<EntityMovement>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            EntityMovement.Flip();
        }
    }
}
