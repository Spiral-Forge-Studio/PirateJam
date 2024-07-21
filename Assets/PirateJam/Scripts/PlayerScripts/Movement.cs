using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class Movement : MonoBehaviour
{
    public Rigidbody2D rb;

    [Header("Ground Check")]
    public Transform groundCheck;
    public LayerMask groundLayer;
    public float groundCheckRadius;

    [Header("Horizontal Movement")]
    public float maxMoveSpeed;
    public float accelleration;
    public float flipAccelleration;
    private float horizontal;
    private bool isFacingRight = true;

    [Header("Vertical Movement")]
    public float jumpingPower;
    public float fallGravityMultiplier;
    public int maxFallSpeed;
    public float minFallModVelocityY;
    public float coyoteTime;
    private float originalGravityScale;
    private float coyoteTimecounter;

    [Header("Jump buffering")]
    public float jumpBufferTime;
    private float jumpBufferCounter;
    
    // Start is called before the first frame update
    void Start()
    {
        originalGravityScale = rb.gravityScale;
    }

    // Update is called once per frame
    void Update()
    {
        if (IsGrounded())
        {
            coyoteTimecounter = coyoteTime;
        }
        else
        {
            coyoteTimecounter -= Time.deltaTime;
        }
        {
            
        }
        if (!isFacingRight && horizontal > 0f)
        {
            //Flip();
        }
        else if (isFacingRight && horizontal < 0f)
        {
            //Flip();
        }
    }
    private void FixedUpdate()
    {
        UpdateSpeed();
        AdjustGravity();
    }

    public void Jump(InputAction.CallbackContext context)
    {
        if (context.performed && coyoteTimecounter > 0f)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpingPower);
        }
        if (context.canceled && rb.velocity.y > 0f)
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y*0.5f);

            coyoteTimecounter = 0f;
        }
    }

    public bool IsGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
    }

    public void Flip()
    {
        isFacingRight = !isFacingRight;
        Vector3 localScale = transform.localScale;
        localScale.x *= -1f;
        transform.localScale = localScale;
    }

    public void Move(InputAction.CallbackContext context)
    {
        horizontal = context.ReadValue<Vector2>().x;
    }

    public void UpdateSpeed()
    {
        if (IsGrounded() && horizontal != 0)
        {
            if (rb.velocity.x < maxMoveSpeed)
            {
                if (Mathf.Sign(rb.velocity.x) == Mathf.Sign(horizontal))
                {
                    rb.velocity = new Vector3(Mathf.MoveTowards(
                        rb.velocity.x, maxMoveSpeed * horizontal, accelleration * Time.fixedDeltaTime), rb.velocity.y);
                }
                else
                {
                    rb.velocity = new Vector3(Mathf.MoveTowards(
                        rb.velocity.x, maxMoveSpeed * horizontal, flipAccelleration * Time.fixedDeltaTime), rb.velocity.y);
                }
            }
            else
            {
                rb.velocity = new Vector2(horizontal * maxMoveSpeed, rb.velocity.y);
            }
        }
        else
        {
            rb.velocity = new Vector2(horizontal * maxMoveSpeed, rb.velocity.y);
        }
    }

    public void AdjustGravity()
    {

        if (rb.velocity.y < minFallModVelocityY && !IsGrounded())
        {
            rb.gravityScale = originalGravityScale * fallGravityMultiplier;
            rb.velocity = new Vector2(rb.velocity.x, Mathf.Max(rb.velocity.y, -maxFallSpeed));
        }

        else
        {
            rb.gravityScale = originalGravityScale;
        }
    }


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
    }
}
