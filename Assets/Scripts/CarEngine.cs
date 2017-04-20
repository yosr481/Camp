using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarEngine : MonoBehaviour {

    public Transform pathHolder;
    public float maxSteeringAngle = 45f;
    public WheelCollider[] frontWheels = new WheelCollider[2];

    Transform[] wayPoints;
    List<Transform> nodes;
    int currentNode = 0;
    // Use this for initialization
    void Start () {
        wayPoints = pathHolder.GetComponentsInChildren<Transform>();
        nodes = new List<Transform>();

        for(int i = 1; i < wayPoints.Length; i++)
        {
            nodes.Add(wayPoints[i]);
        }
    }
	
	// Update is called once per frame
	void FixedUpdate () {
        ApplySteer();
	}

    void ApplySteer()
    {
        Vector3 relativeVector = transform.InverseTransformPoint(nodes[currentNode].position);
        relativeVector /= relativeVector.magnitude;

        float newSteer = (relativeVector.x / relativeVector.magnitude) * maxSteeringAngle;
        frontWheels[0].steerAngle = newSteer;
        frontWheels[1].steerAngle = newSteer;
    }
}
