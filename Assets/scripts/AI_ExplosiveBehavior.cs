using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI_ExplosiveBehavior : MonoBehaviour
{
    AIController script;
//    [SerializeField] Transform target;
    [SerializeField] float explosivePower = 10.0f;
    [SerializeField] float explosiveRadius = 5.0f;
    [SerializeField] float explosiveUpForce = 1.0f;

    void Start() {
        script = GetComponent<AIController>();
    }

    void FixedUpdate() {
        if (script.isWithinProximity == true){
            Invoke("DetonateExplosion", 1.0f);
        }
    }

    void DetonateExplosion() {
        Collider[] colliders = Physics.OverlapSphere(transform.position, explosiveRadius); //hits everything within explosion boundary
        foreach (Collider hit in colliders) {
            Rigidbody rb = hit.GetComponent<Rigidbody>();
            if (rb != null) {
                rb.AddExplosionForce(explosivePower, transform.position, explosiveRadius, explosiveUpForce, ForceMode.VelocityChange);
            }
        }
    }
}
