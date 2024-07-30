using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CritterMovement : MonoBehaviour
{
    const string LEFT = "left";
    const string RIGHT = "right";

    [SerializeField] private LayerMask layerMask;

    [SerializeField] private Transform castPos;

    [SerializeField] private float baseCastDist;

    string facingDirection;

    [SerializeField] private float moveSpeed = 5f;

    Rigidbody2D rb;

    Vector3 baseScale;

    void Start()
    {
        baseScale = transform.localScale;
        facingDirection = RIGHT;
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        float directX = moveSpeed;

        if (facingDirection == LEFT)
        {
            directX = -moveSpeed;
        }

        rb.velocity = new Vector2(directX, rb.velocity.y);

        if ( IsHittingWall() || IsNearingEdge())
        {
            if (facingDirection == LEFT)
            {
                ChangeDirection(RIGHT);
            }
            else if (facingDirection == RIGHT)
            {
                ChangeDirection(LEFT);
            }
        }
}
    
    //this function will flip the sprite
    void ChangeDirection(string newDirection)
    {
        Vector3 newScale = baseScale;

        if (newDirection == LEFT)
        {
            newScale.x = -baseScale.x;
        }
        else if(newDirection == RIGHT)
        {
            newScale.x = baseScale.x;
        }

        transform.localScale = newScale;

        facingDirection = newDirection;
    }

    bool IsHittingWall()
    {
        bool val = false;

        float castDist = baseCastDist;

        if (facingDirection == LEFT)
        {
            castDist = -baseCastDist;
        }
        else
        {
            castDist = baseCastDist;
        }

        Vector3 targetPos = castPos.position;
        targetPos.x += castDist;

        Debug.DrawLine(castPos.position, targetPos, Color.red);


        if (Physics2D.Linecast(castPos.position, targetPos, layerMask))
        {
            val= true;
        }
        else
        {
            val= false;
        }

        return val;
    }

    bool IsNearingEdge()
    {
        bool val = true;

        float castDist = baseCastDist;


        Vector3 targetPos = castPos.position;
        targetPos.y -= castDist;

        Debug.DrawLine(castPos.position, targetPos, Color.blue);


        if (Physics2D.Linecast(castPos.position, targetPos, layerMask))
        {
            val = false;
        }
        else
        {
            val = true;
        }

        return val;
    }
}
