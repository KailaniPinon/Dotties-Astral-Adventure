using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraArm : MonoBehaviour {

    public Camera mainCam;
    public Transform targetToView;
    public float lerpSpeed = 1f;
	
	void FixedUpdate () {
        mainCam.transform.position = Vector3.Lerp(mainCam.transform.position, 
            transform.position, Time.fixedDeltaTime * lerpSpeed);

        mainCam.transform.LookAt(targetToView);       
	}
}
