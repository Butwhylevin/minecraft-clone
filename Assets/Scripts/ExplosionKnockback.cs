using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionKnockback : MonoBehaviour
{
    public float power = 10.0f;
    public float radius = 4.0f;

    
    void Start()
    {
        Vector3 explosionPos = transform.position;
        Collider[] colliders = Physics.OverlapSphere(explosionPos, radius);
        foreach (Collider hit in colliders)
        {
            Debug.Log(hit.gameObject);
            Rigidbody rb = hit.GetComponent<Rigidbody>();

            if (rb != null)
                rb.AddExplosionForce(power, explosionPos, radius, 3.0F);
        }
    }
}
