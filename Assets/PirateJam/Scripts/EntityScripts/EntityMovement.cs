using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EntityMovement : MonoBehaviour
{

    public float moveSpeed;
    public float moveForce;
    public Rigidbody2D rb;
    public Transform groundCheck;
    private float groundCheckRadius;

    [Header("Platform Patrol Settings")]
    private float checkDistance;
    private Vector3 checkDirection;
    public Transform checkPosition;
    public LayerMask platformMask;

    private float originalLocalScaleMagnitude;

    [Header("Explosion Settings")]
    public float explosionHitResetCD;
    private Coroutine explosionRoutineVariable;

    [Header("DEBUG, READONLY")]
    public bool hitByExplosion;
    private Vector3 prevPos;
    private bool isFacingRight;
    private float isFacingRightSign;

    // Start is called before the first frame update
    void Awake()
    {
        isFacingRight = true;
        explosionRoutineVariable = null;
        hitByExplosion = false;
        originalLocalScaleMagnitude = transform.localScale.magnitude;
        rb = GetComponent<Rigidbody2D>();
        checkDistance = Vector3.Distance(transform.position, checkPosition.position);
        groundCheckRadius = transform.localScale.magnitude / 10;
    }

    private void FixedUpdate()
    {
        prevPos = transform.position;

        //Debug.Log(rb.velocity);
        if (IsGrounded())
        {
            PlatformPatrol();
        }
        if (hitByExplosion)
        {
            if (explosionRoutineVariable == null)
            {
                explosionRoutineVariable = StartCoroutine(explosionRoutine());
            }
        }
    }

    public void Flip()
    {
        isFacingRight = !isFacingRight;

        if (isFacingRight == true)
        {
            isFacingRightSign = 1;
            transform.rotation = new Quaternion(transform.rotation.x, 0, transform.rotation.z, transform.rotation.w);
        }
        else
        {
            isFacingRightSign = -1;
            transform.rotation = new Quaternion(transform.rotation.x, 180, transform.rotation.z, transform.rotation.w);
        }
    }

    public void PlatformPatrol()
    {
        groundCheckRadius = transform.localScale.magnitude / 10;

        CheckEdge();

        //rb.velocity = new Vector3(Mathf.Sign(transform.localScale.x)*moveSpeed, rb.velocity.y, 0);

        if (Mathf.Abs(rb.velocity.x) < moveSpeed)
        {
            rb.AddForce(new Vector2(Mathf.Sign(isFacingRightSign) * moveForce, 0));
        }
        else
        {
            //Debug.Log("4");
            //Debug.Break();
            rb.velocity = new Vector3(Mathf.Sign(isFacingRightSign) * moveSpeed, rb.velocity.y, 0);
        }

    }

    public void CheckEdge()
    {
        checkDirection = Vector3.Normalize(checkPosition.position - transform.position);

        RaycastHit2D hit = Physics2D.Raycast(transform.position, checkDirection, checkDistance, platformMask);

        if (!hit && IsGrounded())
        {
            Flip();
        }
    }

    public bool IsGrounded()
    {
        if (Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, platformMask))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public Vector3 GetMovementDelta()
    {
        return transform.position - prevPos;
    }

    private IEnumerator explosionRoutine()
    {
        yield return new WaitForSeconds(explosionHitResetCD);

        explosionRoutineVariable = null;

        hitByExplosion = false;
        
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawLine(transform.position, checkPosition.position);
        Gizmos.DrawWireSphere(transform.position, checkDistance);
        Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
    }
}
