using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : BaseCharacterMovement {
    BaseCharacterMovement bcmScript;

    private void Start() {
        bcmScript = GetComponent<BaseCharacterMovement>();  //retrieve script
        m_baseRigidBody = GetComponent<Rigidbody>();
        transform.position = new Vector3 (155, 1, 200);
    }

    void FixedUpdate() {
        Move(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), movementEffects.none);

        if (Input.GetButtonDown("Jump") && m_isGrounded == true){
            Jump(1);
            Debug.Log("JUMP KEY PRESSED");
        }
    }
}
