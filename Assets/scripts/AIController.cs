using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIController : MovementController
{

    public Transform targetPlayer;      //target to approach (follow) {declaration}
    //NOTE: isWIthinProximityis PUBLIC so that specific AI behavior scripts can access them directly
    public bool isWithinProximity;          //yes or no condition to prompt target interaction {declaration}
    [SerializeField] float proximityDistance;           //distance between target and self
    [SerializeField] float proximityLimit = 8.5f;       //distance within limit boundary (range for interaction)
    [SerializeField] float proximityStop = 5.0f;

    public Vector3 posTarget;                           //shorthand for target position
    private AI_PositionReturn scriptPosReturn;
    AI_DetectWall scriptaiDetectWall;

    [SerializeField] LayerMask player = 9;

    public bool isPlayerVisible = false;
    public float wallDetRadius = 10.0f;


    //////////////////////////////////////////////////
    enum State { Passive, Interact }
    enum StatePassive { Idle, ApproachPlayer, ApproachStart }
    enum StateAggressive { Idle, Attack }

    [SerializeField] State m_stateOfInteraction;
    [SerializeField] StatePassive m_statePassive;
    [SerializeField] StateAggressive m_stateAggro;

    private void Start(){
        m_stateOfInteraction = State.Passive;
        m_statePassive = StatePassive.Idle;
        m_stateAggro = StateAggressive.Idle;
        scriptPosReturn = GetComponent<AI_PositionReturn>();
        scriptaiDetectWall = GetComponent<AI_DetectWall>();

    }

    public void DetectPlayer() {
        RaycastHit hit;

        if (Physics.Raycast(transform.position, (targetPlayer.position - transform.position), out hit)) {
            if (hit.collider.tag == "Player") {
                Debug.Log("Is hitting player");
                ChaseBehavior();
            }
                isPlayerVisible = true;
        }
        else {
            isPlayerVisible = false;    //else if not looking at wall: isLooking... false
        }

        if (isPlayerVisible == true)
        {
            //Debug.Log("Player is VISIBLE");
            m_stateOfInteraction = State.Interact;
        }
        else {
            m_stateOfInteraction = State.Passive;
        }
    }

void StateApproachPlayer() {
        m_statePassive = StatePassive.ApproachStart;
    }

    public void RunIdle() {
        m_statePassive = StatePassive.Idle;
        //scriptaiDetectWall.isLookingAtObstacle = false;
    }

    void RoamingBehavior()
    {
        //TODO: Add roaming code...
    }


    public void ChaseBehavior() {
        Vector3 posSelf = transform.position;                   //shorthand for self position
        Vector3 destinationDirection = posTarget - posSelf;     //see end for note on Addition & Subtraction of Vectors
        destinationDirection.Normalize();                       //set destinationDirection vector to (1,1,1) for easier manipulation
        float angle = Vector3.Angle(destinationDirection, transform.forward);   //Vector3.Angle(a,b); returns angle (in degrees) between a and b
        Vector3 angleCheck = transform.InverseTransformPoint(posTarget);    //transform position of (targetPos) from world space to local space
                                                                            //Transform.TransformPoint converts local to world
        if (angleCheck.x < 0){ angle = -angle;}                 //adjust angle as needed; prevents orbiting

        if (proximityDistance > proximityStop && m_statePassive != StatePassive.Idle) { //if dist is greater than inner stop && not idle
 //           scriptaiDetectWall.DetectPlayer(targetPlayer);
            if (isPlayerVisible == true) {
                Move(1, angle);
            }
        }
    }

    void StatePassiveAccess() {   //function to encapsulate and access m_passiveState 
        switch (m_statePassive){
            case StatePassive.Idle:
                RunIdle();
                break;
            case StatePassive.ApproachPlayer:
                RoamingBehavior();
                break;
            case StatePassive.ApproachStart:
                //TODO: go to start?
                break;
        }
    }

    void StateAggroAccess() {   //function to encapusulate and access m_aggressive
        switch (m_stateAggro) {
            case StateAggressive.Idle:
                RunIdle();
                break;
            case StateAggressive.Attack:
                DetectPlayer();
                break;
        }
    }

    void Update() {
        proximityDistance = Vector3.Distance(targetPlayer.position,transform.position);

        if (proximityDistance > proximityLimit ||           //if not within donut
            proximityDistance < proximityStop) {
            m_stateOfInteraction = State.Passive;

            if (Vector3.Distance(scriptPosReturn.startPos, transform.position) > proximityStop) {   //if the distance between the start position and self is greater than the stop limit then switch enum to ApproachStart
                m_statePassive = StatePassive.ApproachStart;
            }
            else if (Vector3.Distance(scriptPosReturn.startPos, transform.position) < proximityStop) {
                m_statePassive = StatePassive.Idle;
            }
            isWithinProximity = false;
        }
        if (proximityDistance < proximityLimit) {           //if the distance between objects is less than the range
            m_stateOfInteraction = State.Interact;          //switch interaction state to INTERACTION
            m_statePassive = StatePassive.ApproachStart;   //switch passive state to ApproachStart
            isWithinProximity = true;
            DetectPlayer();     //Execute RayCast

        }

        if (proximityDistance < proximityLimit) {

        }
    }
    void InteractionState() {
        switch (m_stateOfInteraction) {
            case State.Passive:            //in the scenario where the object is "passive"
                StatePassiveAccess();      //execute PassiveBehavior();
                break;
            case State.Interact:           //in the scenarior where the object is "aggressive"
                DetectPlayer();            //execute ChaseBehavior();
                break;
        }
    }

    private void FixedUpdate(){
        InteractionState();
    }
}

/*
Addition of Vectors:
    Vector addition is the way forces and velocities combine.
    Given A and B: A + B can be considered <Ax + Bx, Ay + By, Az + Bz>
    
    Translation occurs in a chain from tail to head.

Subtraction of Vectors:
    Subtraction is addition with the opposite vector.
    Given A and B: B - A = B + (- A)

    This is equivalent to turn vector A around.
    Given A and B and X: B - A = X , such as A + X = B 
    (disregard heads & tails for subtraction)
 

    WATCH for loose function calls
*/
