using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarEngine : MonoBehaviour {

    public Transform centerOfMass;
    public Transform pathHolder;

    [Header("Steering & Driving")]
    public float maxSteeringAngle = 45f;
    public float maxMotorTorque = 80f;
    public float maxBrakeTorque = 150f;
    public float currentSpeed;
    public float maxSpeed = 120f;
    public float currentSteer;
    public float steerDamp = .5f;
    public bool isBraking;

    [Header("Sensors")]


    [Header("Wheels")]
    public WheelCollider[] wheels = new WheelCollider[4];
    public Transform[] tireMeshes = new Transform[4];

    Transform[] wayPoints;
    List<Transform> nodes;
    int currentNode = 0;

    Rigidbody rigid;
    // Use this for initialization
    void Start () {
        rigid = GetComponent<Rigidbody>();
//        rigid.centerOfMass = centerOfMass.position;

        wayPoints = pathHolder.GetComponentsInChildren<Transform>();
        nodes = new List<Transform>();

        for(int i = 1; i < wayPoints.Length; i++)
        {
            nodes.Add(wayPoints[i]);
        }
    }

    // Update is called once per frame
    void Update()
    {
        UpdateMeshesPositions();
    }
	
	void FixedUpdate () {
        ApplySteer();
        Drive();
        CheckWaypointDistance();
        Braking();
	}

    void ApplySteer()
    {
        Vector3 relativeVector = transform.InverseTransformPoint(nodes[currentNode].position);
        relativeVector /= relativeVector.magnitude;
        currentSteer = wheels[0].steerAngle;

        float newSteer = (relativeVector.x / relativeVector.magnitude) * maxSteeringAngle;
        wheels[0].steerAngle = Mathf.Lerp(currentSteer, newSteer, steerDamp * Time.deltaTime);
        wheels[1].steerAngle = Mathf.Lerp(currentSteer, newSteer, steerDamp * Time.deltaTime);
    }

    void Drive()
    {
        currentSpeed = 2 * Mathf.PI * wheels[0].radius * wheels[0].rpm * 60 / 1000;

        if(currentSpeed < maxSpeed && !isBraking)
        {
            wheels[0].motorTorque = maxMotorTorque;
            wheels[1].motorTorque = maxMotorTorque;
        }
        else
        {
            wheels[0].motorTorque = 0;
            wheels[1].motorTorque = 0;
        }
    }

    void Braking()
    {
        if (isBraking)
        {
            wheels[2].brakeTorque = maxBrakeTorque;
            wheels[3].brakeTorque = maxBrakeTorque;
        }
        else
        {
            wheels[2].brakeTorque = 0;
            wheels[3].brakeTorque = 0;
        }
    }

    void UpdateMeshesPositions()
    {
        for (int i = 0; i < 4; i++)
        {
            Quaternion rotation;
            Vector3 position;
            wheels[i].GetWorldPose(out position, out rotation);

            tireMeshes[i].position = position;
            tireMeshes[i].rotation = rotation;
        }
    }

    void CheckWaypointDistance()
    {
        if(Vector3.Distance(transform.position, nodes[currentNode].position) < 0.5f)
        {
            if(currentNode == nodes.Count - 1)
            {
                currentNode = 0;
            }
            else
            {
                currentNode++;
            }
        }
    }
}
