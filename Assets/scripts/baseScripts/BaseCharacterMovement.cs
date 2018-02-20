using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class BaseCharacterMovement : MonoBehaviour {
    [SerializeField] private float m_baseTranslationSpeed = 60.0f;
    [SerializeField] private float m_baseRotationSpeed = 3.0f;
    [SerializeField] private float m_baseJumpForce = 20.0f;
    [SerializeField] protected Rigidbody m_baseRigidBody;
    protected enum movementEffects {none, hindered, sprint}
    [SerializeField] protected movementEffects m_movementEffect;
    [SerializeField] protected bool m_isGrounded;

    private void Start() {
        m_baseRigidBody = GetComponent<Rigidbody>();  //retrieve the current object's rigidbody
        m_movementEffect = movementEffects.none;      //start with no movement effects
        m_isGrounded = true;
    }

    protected void Move(float rotationSpeed, float translationSpeed, movementEffects currentMovementEffect) {
        float movementEffectSpeed = 1.0f;
        switch (m_movementEffect) {
            case movementEffects.none: movementEffectSpeed = 1.0f; break;
            case movementEffects.hindered: movementEffectSpeed = 0.50f; break;
            case movementEffects.sprint: movementEffectSpeed = 1.50f; break;
        }

        m_baseRigidBody.AddForce(transform.forward * m_baseTranslationSpeed * translationSpeed * movementEffectSpeed);   //translate
        m_baseRigidBody.AddTorque(Vector3.up * m_baseRotationSpeed * rotationSpeed);    //rotate
    }

    private void OnCollisionStay(Collision terrain) {
        if (terrain.gameObject.tag == "Terrain" || terrain.gameObject.tag == "Climbable") {
            m_isGrounded = true;
//            Debug.Log("Touching Terrain");
        }
    }

    private void OnCollisionExit(Collision terrain) {
        m_isGrounded = false;
    }

    protected void Jump (float jumpForce) { 
        if (m_isGrounded==true) {
            float totalJump = jumpForce + m_baseJumpForce;
            m_baseRigidBody.AddForce(new Vector3(0, totalJump, 0), ForceMode.Impulse);
        }
    }
}

