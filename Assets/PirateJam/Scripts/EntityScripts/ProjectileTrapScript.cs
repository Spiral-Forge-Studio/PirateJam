using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileTrapScript : MonoBehaviour
{
    [SerializeField] private float maxDistance;
    [SerializeField] private Transform firePoint;

    private void Update()
    {

        Vector3 raycastDirection = transform.right;
        raycastDirection.x += maxDistance;

        Physics2D.Raycast(firePoint.position, raycastDirection, maxDistance);

        Debug.DrawRay(firePoint.position, raycastDirection, Color.red);
    }



}
