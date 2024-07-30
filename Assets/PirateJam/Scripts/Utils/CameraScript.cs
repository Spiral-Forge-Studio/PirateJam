using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    public float smoothTime = 0.25f;
    private Vector3 velocity = Vector3.zero;

    [SerializeField] private Transform target;

    private void LateUpdate()
    {
        Vector3 targetPosition = new Vector3(target.position.x, target.position.y, transform.position.z);
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);
    }
}
