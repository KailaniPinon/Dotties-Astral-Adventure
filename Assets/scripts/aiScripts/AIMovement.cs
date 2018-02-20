using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIMovement : BaseCharacterMovement {
    [SerializeField] private Transform playerPosition;
    [SerializeField] public bool isWithinRange;
    [SerializeField] private float rangeOuterLimit = 10.0f;
    [SerializeField] public float rangeInnerLimit = 5.0f;
    [SerializeField] public Vector3 destination;
    [SerializeField] private float range;

    public enum Interaction {unware, interact, aggressive, timid}
    public enum PathBehavior {idle, followPath, followPlayer, returnToPath}
    [SerializeField] public Interaction m_aggroStatus;
    [SerializeField] public PathBehavior m_pathStatus;

    [SerializeField] Transform initialPosTransform;
    [SerializeField] public Vector3 initialPosVec;
    [SerializeField] Vector3 initialOrientation;

    [SerializeField] private AIPatrollingBehavior aipbScript;

    private void Start() {
        m_aggroStatus = Interaction.unware;
        m_pathStatus = PathBehavior.followPath;
        m_baseRigidBody = GetComponent<Rigidbody>();
        initialPosTransform = GetComponent<Transform>();
        initialPosVec = initialPosTransform.position;
        initialOrientation = initialPosTransform.rotation.eulerAngles;
        aipbScript = GetComponent<AIPatrollingBehavior>();
    }

    private void ApproachDestination() {
        Vector3 targetDir = destination - transform.position;
        targetDir.Normalize();
        float angle = Vector3.Angle(targetDir, transform.forward);

        Vector3 angleCheck = transform.InverseTransformPoint(destination);
        if (angleCheck.x < 0) { angle = -angle; }
        if (Vector3.Distance(destination, transform.position) > rangeInnerLimit &&
            m_aggroStatus != Interaction.timid)
            Move(angle,0.5f,movementEffects.none);
    }

    private void DetectPlayer() {
        RaycastHit obstruction;
        if (Physics.Raycast(transform.position, (playerPosition.position - transform.position), out obstruction)) {
            if (obstruction.collider.tag == "Player") {
                Debug.Log("Player had been detected.");
                m_aggroStatus = Interaction.unware;
            } else m_aggroStatus = Interaction.timid;
        }
    }

    private void Update() {
        range = Vector3.Distance(playerPosition.position, transform.position);
        ApproachDestination();

        switch (m_pathStatus) {
            case PathBehavior.idle: destination = initialPosVec; break;
            case PathBehavior.followPath: aipbScript.PatrolBehavior(); break;
            case PathBehavior.followPlayer: destination = playerPosition.position; break;
            case PathBehavior.returnToPath: destination = initialPosVec; break;
        }

        switch (m_aggroStatus) {
            case Interaction.aggressive: break;
            case Interaction.interact: break;
            case Interaction.timid: transform.rotation = Quaternion.Euler(initialOrientation); break;
            case Interaction.unware: break;
        }

        if (Vector3.Distance(playerPosition.position, transform.position) < rangeOuterLimit) {
            isWithinRange = true;
//            Debug.Log("Player is within range of AI");
        } else isWithinRange = false;

        if (isWithinRange == true) {
            m_pathStatus = PathBehavior.followPlayer;
            DetectPlayer();
        } else  {
            m_pathStatus = PathBehavior.followPath;
            if (Vector3.Distance(initialPosVec, transform.position) < rangeInnerLimit) {
                m_pathStatus = PathBehavior.followPath;
            }
        }


            if (m_aggroStatus == Interaction.unware && 
            (m_pathStatus == PathBehavior.idle) &&
            Vector3.Distance(initialPosVec, transform.position) < rangeInnerLimit) {
            transform.rotation = Quaternion.Euler(initialOrientation);
        }
    }
}


