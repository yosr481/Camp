﻿using System.Collections;
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
    public float brakingForce;
    public float currentSpeed = 0f;

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
            Brakes(true);
        }
        else
        {
            Brakes(false);
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
        currentSpeed = 2 * Mathf.PI * wheelColliders[0].radius * wheelColliders[0].rpm * 60 / 1000;

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

    void Brakes(bool isBreaking)
    {
        if (isBreaking)
        {
            if (driveMode == DriveMode.AllWheels)
            {
                wheelColliders[0].brakeTorque = brakingForce;
                wheelColliders[1].brakeTorque = brakingForce;
                wheelColliders[2].brakeTorque = brakingForce;
                wheelColliders[3].brakeTorque = brakingForce;
            }
            else if (driveMode == DriveMode.Rear)
            {
                wheelColliders[2].brakeTorque = brakingForce;
                wheelColliders[3].brakeTorque = brakingForce;
            }
            else
            {
                wheelColliders[0].brakeTorque = brakingForce;
                wheelColliders[1].brakeTorque = brakingForce;
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
