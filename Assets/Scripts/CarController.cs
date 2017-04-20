using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : MonoBehaviour {

    public enum DriveMode
    {
        Rear,
        Front,
        AllWheels
    }
    public DriveMode driveMode = DriveMode.AllWheels;
    public float maxTorque = 50f;
    public float steerForce = 2f;
    public float breakingForce;

    public WheelCollider[] wheelColliders = new WheelCollider[4];
    public Transform[] tireMeshes = new Transform[4];
    public Transform centerOfMass;

    Rigidbody rigid;

	// Use this for initialization
	void Start () {
        rigid = GetComponent<Rigidbody>();
        rigid.centerOfMass = centerOfMass.localPosition;
	}
	
	// Update is called once per frame
	void Update () {
        UpdateMeshesPositions();
	}

    void FixedUpdate()
    {
        FrontWheelsSteering();
        Acceleration();
        if (Input.GetButton("Fire1"))
        {
            Breaks(true);
        }
        else
        {
            Breaks(false);
        }
    }

    void FrontWheelsSteering()
    {
        float finalSteer = steerForce * Input.GetAxis("Horizontal");

        wheelColliders[0].steerAngle = finalSteer;
        wheelColliders[1].steerAngle = finalSteer;
    }

    void Acceleration()
    {
        float acceleration = maxTorque * Input.GetAxis("Vertical");
        if(driveMode == DriveMode.AllWheels)
        {
            wheelColliders[0].motorTorque = acceleration;
            wheelColliders[1].motorTorque = acceleration;
            wheelColliders[2].motorTorque = acceleration;
            wheelColliders[3].motorTorque = acceleration;
        }
        else if (driveMode == DriveMode.Rear)
        {
            wheelColliders[2].motorTorque = acceleration;
            wheelColliders[3].motorTorque = acceleration;
        }
        else
        {
            wheelColliders[0].motorTorque = acceleration;
            wheelColliders[1].motorTorque = acceleration;
        }
    }

    void Breaks(bool isBreaking)
    {
        if (isBreaking)
        {
            if (driveMode == DriveMode.AllWheels)
            {
                wheelColliders[0].brakeTorque = breakingForce;
                wheelColliders[1].brakeTorque = breakingForce;
                wheelColliders[2].brakeTorque = breakingForce;
                wheelColliders[3].brakeTorque = breakingForce;
            }
            else if (driveMode == DriveMode.Rear)
            {
                wheelColliders[2].brakeTorque = breakingForce;
                wheelColliders[3].brakeTorque = breakingForce;
            }
            else
            {
                wheelColliders[0].brakeTorque = breakingForce;
                wheelColliders[1].brakeTorque = breakingForce;
            }
        }
        else
        {
            if (driveMode == DriveMode.AllWheels)
            {
                wheelColliders[0].brakeTorque = 0;
                wheelColliders[1].brakeTorque = 0;
                wheelColliders[2].brakeTorque = 0;
                wheelColliders[3].brakeTorque = 0;
            }
            else if (driveMode == DriveMode.Rear)
            {
                wheelColliders[2].brakeTorque = 0;
                wheelColliders[3].brakeTorque = 0;
            }
            else
            {
                wheelColliders[0].brakeTorque = 0;
                wheelColliders[1].brakeTorque = 0;
            }
        }
        
    }

    void UpdateMeshesPositions()
    {
        for(int i = 0; i < 4; i++)
        {
            Quaternion rotation;
            Vector3 position;
            wheelColliders[i].GetWorldPose(out position, out rotation);

            tireMeshes[i].position = position;
            tireMeshes[i].rotation = rotation;
        }
    }
}
