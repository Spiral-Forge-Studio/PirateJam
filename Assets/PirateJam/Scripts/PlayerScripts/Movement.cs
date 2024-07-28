using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class Movement : MonoBehaviour
{
    public Rigidbody2D rb;

    [Header("Animation")]
    public Animator animator;
    public AnimationScript animScript;
    public string IDLE_ANIM;
    public string RUN_ANIM;
    public string AIR_ANIM;

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


    [Header("Fall Damage")]
    public float maxFallHeight;
    private bool isFalling;
    private bool firstTime;
    private Vector3 previousPosition;
    private float highestPosition;

    [Header("[DEBUG, READONLY] Explosion Hit Info")]
    public bool hitByExplosion;
    private bool onPlatform;

    // Start is called before the first frame update
    void Start()
    {
        
        onPlatform = false;
        firstTime = true;
        isFalling = false;
        hitByExplosion = false;
        hittingWallLeft = false;
        hittingWallRight = false;
        originalGravityScale = rb.gravityScale;
        animScript.ChangeAnimationsState(animator, IDLE_ANIM);
        
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

        if (!isFacingRight && Camera.main.ScreenToWorldPoint(Input.mousePosition).x > transform.position.x)
        {
            Flip();
        }

        else if (isFacingRight && Camera.main.ScreenToWorldPoint(Input.mousePosition).x < transform.position.x)
        {
            Flip();
        }
    }
    private void FixedUpdate()
    {
        UpdateSpeed();
        AdjustGravity();
        CheckForFallDamage();
    }

    private void CheckForFallDamage()
    {
        if (!IsGrounded())
        {
            animScript.ChangeAnimationsState(animator, AIR_ANIM);

            if (rb.velocity.y < 0 && firstTime)
            {
                firstTime = false;
                isFalling = true;
                highestPosition = transform.position.y;
            }
            previousPosition = transform.position;
        }

        if (IsGrounded() && isFalling)
        {
            animScript.ChangeAnimationsState(animator, IDLE_ANIM);

            if (highestPosition - transform.position.y > maxFallHeight)
            {
                SceneController.instance.ReloadCurrentScene();
            }

            isFalling = false;
            firstTime = true;
        }
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
        if (Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer) || onPlatform)
        {
            if (rb.velocity.x == 0)
            {
                animScript.ChangeAnimationsState(animator, IDLE_ANIM);
            }

            return true;
        }
        else
        {
            return false;
        }
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
            if (isFacingRight)
            {
                rb.velocity = new Vector2(Mathf.Clamp(horizontal * maxMoveSpeed, -maxMoveSpeed, 0), rb.velocity.y);
            }
            else
            {
                rb.velocity = new Vector2(Mathf.Clamp(horizontal * maxMoveSpeed, 0, maxMoveSpeed), rb.velocity.y);
            }
            
        }
        else if (hittingWallRight)
        {
            if (isFacingRight)
            {
                rb.velocity = new Vector2(Mathf.Clamp(horizontal * maxMoveSpeed, -maxMoveSpeed, 0), rb.velocity.y);
            }
            else
            {
                rb.velocity = new Vector2(Mathf.Clamp(horizontal * maxMoveSpeed, 0, maxMoveSpeed), rb.velocity.y);
            }
        }

        // normal ground movement
        else if (IsGrounded() && horizontal != 0)
        {
            animScript.ChangeAnimationsState(animator, RUN_ANIM);

            if (rb.velocity.x < maxMoveSpeed)
            {
                if (Mathf.Sign(rb.velocity.x) == Mathf.Sign(horizontal))
                {
                    //rb.velocity = new Vector3(Mathf.SmoothStep(
                    //    rb.velocity.x, maxMoveSpeed * horizontal, accelleration * Time.fixedDeltaTime), rb.velocity.y);
                    rb.velocity = new Vector3(maxMoveSpeed * horizontal, rb.velocity.y);
                }
                else
                {
                   // rb.velocity = new Vector3(Mathf.SmoothStep(
                   //     rb.velocity.x, maxMoveSpeed * horizontal, flipAccelleration * Time.fixedDeltaTime), rb.velocity.y);
                    rb.velocity = new Vector3(maxMoveSpeed * horizontal, rb.velocity.y);
                }
            }
            else
            {
                rb.velocity = new Vector2(horizontal * maxMoveSpeed, rb.velocity.y);
            }
        }

        // hit by explosion movement
        else if (hitByExplosion)
        {
            if (horizontal != 0)
            {
                hitByExplosion = false;
            }
            else
            {
                rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y);
            }
        }

        // freefall movement
        else if (horizontal == 0)
        {
            if (IsGrounded())
            {
                animScript.ChangeAnimationsState(animator, IDLE_ANIM);
            }
            rb.velocity = new Vector2(0, rb.velocity.y);
        }
        else
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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Platform"))
        {
            onPlatform = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Platform"))
        {
            onPlatform = false;
        }
    }


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
    }
}
