using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class MovementController : MonoBehaviour {
    [SerializeField] float forwardSpeedRate = 30.0f;
    [SerializeField] float angularSpeedRate = 8.0f;
    [SerializeField] float jumpForce = 50.0f;  

    [SerializeField] Rigidbody rbBase;      //control target {declaration}

    private void Start(){
//        Physics.gravity = new Vector3(0, -98.7f, 0); //add force instead of manipulating gravity
//        Debug.Log(Physics.gravity);
        rbBase = GetComponent<Rigidbody>(); //control target {definition} = self rigid body
    }
    protected void Move(float forwardSpeed, float angularSpeed) {
        rbBase.AddForce(transform.forward * forwardSpeedRate * forwardSpeed);   //translate controlled
        rbBase.AddTorque(Vector3.up * angularSpeedRate * angularSpeed );        //rotate controlled
                                                                                //add Upward force
        if (Input.GetKeyDown("space")) {
            transform.Translate(Vector3.up * jumpForce * Time.deltaTime);
        }
    }
}
