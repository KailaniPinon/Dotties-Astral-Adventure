using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFacingBillboard : MonoBehaviour {

    public Camera m_MainCam;

    void Update() {
        transform.LookAt(transform.position + m_MainCam.transform.rotation * Vector3.forward,
            m_MainCam.transform.rotation * Vector3.up);
    }
}
