using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class MovingPlatform : MonoBehaviour
{
    [SerializeField] Transform movingPlatform;
    [SerializeField] Transform startPoint;
    [SerializeField] Transform endPoint;

    [SerializeField] private float speed = 1.5f;
    private Vector3 prevPos;
    int direction = 1;

    private void Update()
    {
        Vector2 target = currentMovementTarget();

        movingPlatform.position = Vector2.MoveTowards(movingPlatform.position, target, speed * Time.deltaTime);

        float distance = (target - (Vector2)movingPlatform.position).magnitude;

        if (distance <= 0.05f)
        {
            direction *= -1;
        }
    }

    private void FixedUpdate()
    {
        prevPos = transform.localPosition;
    }

    Vector2 currentMovementTarget()
    {
        if (direction == 1)
        {
            return startPoint.position;
        }
        else
        {
            return endPoint.position;

        }
    }

    public Vector3 GetMovementDelta()
    {
        return transform.localPosition - prevPos;
    }

    private void OnDrawGizmosSelected()
    {
        if (movingPlatform != null && startPoint != null && endPoint != null)
        {
            Gizmos.DrawLine(movingPlatform.position, startPoint.position);
            Gizmos.DrawLine(movingPlatform.position, endPoint.position);
        }
    }
}
