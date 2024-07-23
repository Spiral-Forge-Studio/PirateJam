using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectExplosion : MonoBehaviour
{
    Collider2D[] inExplosionRadius = null;
    public float ExplosionForceMulti;
    public float ExplosionRadius;

    private void FixedUpdate()
    {

    }

    public void Explode()
    {
        inExplosionRadius = Physics2D.OverlapCircleAll(transform.position, ExplosionRadius);

        foreach (Collider2D hitObject in inExplosionRadius)
        {
            Rigidbody2D hitObjectRB = hitObject.GetComponent<Rigidbody2D>();

            if (hitObjectRB != null)
            {
                if (hitObjectRB.gameObject.CompareTag("Ground"))
                {
                    continue;
                }

                Vector2 distanceVec = hitObject.transform.position - transform.position;

                RaycastHit2D ray = Physics2D.Raycast(transform.position, distanceVec.normalized, ExplosionRadius);

                Debug.DrawLine(transform.position, hitObjectRB.position, Color.red, 2f);
                //Debug.Log("raycast hit: " + ray.collider.gameObject.name);

                //if (distanceVec.magnitude > 0 && !ray.collider.gameObject.CompareTag("Ground"))
                if (ray.collider != null)
                {
                    if (distanceVec.magnitude > 0 && !ray.collider.gameObject.CompareTag("Ground"))
                    {
                        //float explosionForce = ExplosionForceMulti/(distanceVec.magnitude);
                        float explosionForce = ExplosionForceMulti;
                        hitObjectRB.AddForce(distanceVec.normalized * explosionForce);
                    }
                }
            }
        }
        //Debug.Break();

        Destroy(gameObject, 0.05f);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, ExplosionRadius);
    }

}
