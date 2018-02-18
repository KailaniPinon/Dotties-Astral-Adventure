using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI_PositionReturn : MonoBehaviour {

    AIController scriptAICtrl;
    MovementController scriptMoveCtrl;
    [SerializeField] Transform startPosRef;
    public Vector3 startPos;
    public Quaternion startRot;    //TODO: adjust direction after AI returns

    void Start() {
        startPosRef.position = transform.position;
        startPos = startPosRef.position;
//        startPosRef.rotation = Quaternion.Slerp(transform.rotation, Quaternion.identity, Time.deltaTime * 2.0f);
//        startRot = startPosRef.rotation;
        scriptAICtrl = GetComponent<AIController>();
        scriptMoveCtrl = GetComponent<MovementController>();
    }

    void Update() {
        if (scriptAICtrl.isWithinProximity == false){
            scriptAICtrl.posTarget = startPos;
            scriptAICtrl.ChaseBehavior();
            
        }
        else if (scriptAICtrl.isWithinProximity == true) {
            scriptAICtrl.posTarget = scriptAICtrl.targetPlayer.position;
//            scriptAICtrl.ChaseBehavior();
        }
    }
}

//TODO: return rotation to stored start rotation
//https://docs.unity3d.com/Manual/nav-AgentPatrol.html