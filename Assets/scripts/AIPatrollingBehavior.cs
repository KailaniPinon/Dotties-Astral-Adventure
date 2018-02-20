using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIPatrollingBehavior : MonoBehaviour {
    [SerializeField] public Transform[] destinationPoint;
    [SerializeField] public int randomPoint;
    [SerializeField] private float stallTime;
    [SerializeField] private float initialStallTime;

    [SerializeField] private AIMovement aimScript;

    private void Start() {
        aimScript = GetComponent<AIMovement>();
        destinationPoint[0] = GetComponent<Transform>();
        randomPoint = Random.Range(0,destinationPoint.Length);
        stallTime = initialStallTime;
    }

    public void PatrolBehavior() {
        aimScript.destination = destinationPoint[randomPoint].position;

        if (Vector3.Distance(transform.position, destinationPoint[randomPoint].position) < aimScript.rangeInnerLimit) {
            if (stallTime <= 0) {
                randomPoint = Random.Range(0, destinationPoint.Length);
                stallTime = initialStallTime;
            } else { stallTime -= Time.deltaTime; }
        }
    }
}

//https://www.youtube.com/watch?v=8eWbSN2T8TE
