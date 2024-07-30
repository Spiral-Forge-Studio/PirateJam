using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileTrapScript : MonoBehaviour
{
    [SerializeField] private int maxDistance;
    [SerializeField] private Transform firePoint;

    private void Update()
    {

        Vector2 raycastDirection = transform.forward;
        Physics2D.Raycast(firePoint.position, raycastDirection,maxDistance);

        Debug.DrawLine(firePoint.position, raycastDirection, Color.red);
    }



}
