using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MovementController
{
    MovementController charCtrl;
    private void Start(){
    charCtrl = GetComponent<MovementController>();       
    }

    void FixedUpdate() {
        Move(Input.GetAxis("Vertical"), Input.GetAxis("Horizontal"));
    }
}
