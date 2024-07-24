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
    public bool hittingWallLeft = false;
    public bool hittingWallRight = false;

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
        hittingWallLeft = false;
        hittingWallRight = false;
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
        // wall movement
        if (hittingWallLeft)
        {
            rb.velocity = new Vector2(Mathf.Clamp(horizontal*maxMoveSpeed, 0, maxMoveSpeed), rb.velocity.y);
        }
        else if (hittingWallRight)
        {
            rb.velocity = new Vector2(Mathf.Clamp(horizontal * maxMoveSpeed, -maxMoveSpeed, 0), rb.velocity.y);
        }

        // normal ground movement
        else if (IsGrounded() && horizontal != 0)
        {
            if (rb.velocity.x < maxMoveSpeed)
            {
                if (Mathf.Sign(rb.velocity.x) == Mathf.Sign(horizontal))
                {
                    rb.velocity = new Vector3(Mathf.SmoothStep(
                        rb.velocity.x, maxMoveSpeed * horizontal, accelleration * Time.fixedDeltaTime), rb.velocity.y);
                }
                else
                {
                    rb.velocity = new Vector3(Mathf.SmoothStep(
                        rb.velocity.x, maxMoveSpeed * horizontal, flipAccelleration * Time.fixedDeltaTime), rb.velocity.y);
                }
            }
            else
            {
                rb.velocity = new Vector2(horizontal * maxMoveSpeed, rb.velocity.y);
            }
        }

        // freefall movement
        else if (horizontal != 0)
        {
            rb.velocity = new Vector2(horizontal * maxMoveSpeed, rb.velocity.y);
        }
        //Debug.Log("Velocity: " + rb.velocity);
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
