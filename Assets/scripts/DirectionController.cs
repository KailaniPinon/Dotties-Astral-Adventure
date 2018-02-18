using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]

public class DirectionController : MonoBehaviour {

    public Rigidbody rb;
    public GameObject targetObj;
    [SerializeField] Transform target;
    [SerializeField] float enemyRotationSpeed = 20.0f;
//    [SerializeField] float enemyForward

    void Start () {
        rb = GetComponent<Rigidbody>();
    }

    void Update () {
        targetObj = GameObject.Find("player");
        target = targetObj.transform;

        Vector3 targetPos = target.position;
        Vector3 targetDir = target.position - transform.position;
        float angle = Vector3.Angle(targetDir, transform.up);

        Vector3 inverseAngle = transform.InverseTransformPoint(targetPos);
        if (inverseAngle.y > 0) {
            angle = -angle;
        }

        //DIRECTION FORCE - TORQUE
        Debug.DrawLine(transform.position, target.position);
        rb.AddTorque(Vector3.forward * angle * enemyRotationSpeed);

    }
}
