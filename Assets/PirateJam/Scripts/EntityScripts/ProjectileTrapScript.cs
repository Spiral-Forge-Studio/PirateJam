using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileTrapScript : MonoBehaviour
{
    [SerializeField] private float maxDistance;
    [SerializeField] private Transform firePoint;

    private void Update()
    {
        //if statement for door opening and closing

        Vector3 raycastDirection = transform.right;
        raycastDirection.x += maxDistance;

        RaycastHit2D hit = Physics2D.Raycast(firePoint.position, raycastDirection, maxDistance);

        Debug.DrawRay(firePoint.position, raycastDirection, Color.red);

        if (hit)
        {
            Vector3 endPoint = hit.point;
            Debug.DrawRay(firePoint.position, endPoint - firePoint.position, Color.blue);

            // Do Damage to slime and 
        }
    }



}
