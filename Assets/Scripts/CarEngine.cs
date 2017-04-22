using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarEngine : MonoBehaviour {

    public Transform centerOfMass;
    public Transform pathHolder;

    public enum DrivingMode
    {
        FrontWheels,
        RearWheels,
        AllWheels
    }
    [Header("Steering & Driving")]
    public DrivingMode drivingMode = DrivingMode.AllWheels;
    public float maxSteeringAngle = 45f;
    public float maxMotorTorque = 80f;
    public float maxBrakeTorque = 150f;
    public float currentSpeed;
    public float maxSpeed = 120f;
    public float currentSteer;
    public float steerDamp = .5f;
    public float distanceToWayPoint = 1.5f;
    public bool isBraking;

    [Header("Sensors")]
    public float sensorLength = 5f;
    public float centerSensorLength = 15f;
    public float frontSensorPos = 2.3f;
    public float frontSideSensorPos = 0.8f;
    public float frontSensorAngle = 30f;
    public float wideFrontSensorAngle = 90f;
    public bool isRightAvailable = true;
    public bool isLeftAvailable = true;
    public bool isWideRightAvailable = true;
    public bool isWideLeftAvailable = true;
    WaitForSeconds drivingBack = new WaitForSeconds(5f);
    WaitForSeconds braking = new WaitForSeconds(2.5f);

    [Header("Wheels")]
    public WheelCollider[] wheels = new WheelCollider[4];
    public Transform[] tireMeshes = new Transform[4];

    Transform[] wayPoints;
    List<Transform> nodes;
    int currentNode = 0;
    bool avoiding = false;
    bool isDrivingBack;
    
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
    void Update()
    {
        UpdateMeshesPositions();
    }
	
	void FixedUpdate () {
        Sensors();
        ApplySteer();
        Drive();
        CheckWaypointDistance();
        Braking();
	}

    void ApplySteer()
    {
        if (avoiding) return;

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

        if(currentSpeed < maxSpeed && !isBraking && !isDrivingBack)
        {
            if (drivingMode == DrivingMode.AllWheels)
            {
                wheels[0].motorTorque = maxMotorTorque;
                wheels[1].motorTorque = maxMotorTorque;
                wheels[2].motorTorque = maxMotorTorque;
                wheels[3].motorTorque = maxMotorTorque;
            }
            else if(drivingMode == DrivingMode.FrontWheels)
            {
                wheels[0].motorTorque = maxMotorTorque;
                wheels[1].motorTorque = maxMotorTorque;
            }
            else
            {
                wheels[2].motorTorque = maxMotorTorque;
                wheels[3].motorTorque = maxMotorTorque;
            }
        }
        else
        {
            if(drivingMode == DrivingMode.AllWheels)
            {
                wheels[0].motorTorque = 0;
                wheels[1].motorTorque = 0;
                wheels[2].motorTorque = 0;
                wheels[3].motorTorque = 0;
            }
            else if(drivingMode == DrivingMode.FrontWheels)
            {
                wheels[0].motorTorque = 0;
                wheels[1].motorTorque = 0;
            }
            else
            {
                wheels[2].motorTorque = 0;
                wheels[3].motorTorque = 0;
            }
        }

        if (isDrivingBack)
        {
            if (drivingMode == DrivingMode.AllWheels)
            {
                wheels[0].motorTorque = -maxMotorTorque;
                wheels[1].motorTorque = -maxMotorTorque;
                wheels[2].motorTorque = -maxMotorTorque;
                wheels[3].motorTorque = -maxMotorTorque;
            }
            else if (drivingMode == DrivingMode.FrontWheels)
            {
                wheels[0].motorTorque = -maxMotorTorque;
                wheels[1].motorTorque = -maxMotorTorque;
            }
            else
            {
                wheels[2].motorTorque = -maxMotorTorque;
                wheels[3].motorTorque = -maxMotorTorque;
            }
            wheels[0].steerAngle = Mathf.Lerp(currentSteer, 0f, steerDamp * Time.deltaTime);
            wheels[1].steerAngle = Mathf.Lerp(currentSteer, 0f, steerDamp * Time.deltaTime);
        }
    }

    IEnumerator DriveBackwords()
    {
        currentSpeed = 2 * Mathf.PI * wheels[0].radius * wheels[0].rpm * 60 / 1000;

        isBraking = true;

        yield return braking;

        isBraking = false;
        
        if (!isBraking)
        {
            isDrivingBack = true;
        }

        yield return drivingBack;

        isDrivingBack = false;
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

    void Sensors()
    {
        RaycastHit hit;
        Vector3 centerSensorStartPos = transform.position + transform.forward * frontSensorPos;
        Vector3 rightSensorStartPos = transform.position + transform.forward * frontSensorPos + transform.right * frontSideSensorPos;
        Vector3 rightAngleSensorDirection = Quaternion.AngleAxis(frontSensorAngle, transform.up) * transform.forward;
        Vector3 wideRightAngleSensorDirection = Quaternion.AngleAxis(wideFrontSensorAngle, transform.up) * transform.forward;
        Vector3 leftSensorStartPos = transform.position + transform.forward * frontSensorPos - transform.right * frontSideSensorPos;
        Vector3 leftAngleSensorDirection = Quaternion.AngleAxis(-frontSensorAngle, transform.up) * transform.forward;
        Vector3 wideLeftAngleSensorDirection = Quaternion.AngleAxis(-wideFrontSensorAngle, transform.up) * transform.forward;
        float avoidMultiplier = 0f;
        avoiding = false;
        isBraking = false;
        isLeftAvailable = true;
        isRightAvailable = true;
        isWideLeftAvailable = true;
        isWideRightAvailable = true;

        //Raycasting right side sensor
        if (Physics.Raycast(rightSensorStartPos, transform.forward, out hit, sensorLength))
        {
            if (!hit.collider.CompareTag("Terrain"))
            {
                Debug.DrawLine(rightSensorStartPos, hit.point, Color.red);
                avoiding = true;
                isRightAvailable = false;
                avoidMultiplier -= 1f;
            }
            else if (hit.collider.CompareTag("Car"))
            {
                isBraking = true;
            }
        }

        //Raycasting right angle side sensor
        else if (Physics.Raycast(rightSensorStartPos, rightAngleSensorDirection, out hit, sensorLength * 1.2f))
        {
            if (!hit.collider.CompareTag("Terrain"))
            {
                Debug.DrawLine(rightSensorStartPos, hit.point, Color.red);
                avoiding = true;
                isRightAvailable = false;
                avoidMultiplier -= 0.5f;
            }
            else if (hit.collider.CompareTag("Car"))
            {
                isBraking = true;
            }
        }

        //Raycasting wide right angle side sensor
        else if (Physics.Raycast(rightSensorStartPos, wideRightAngleSensorDirection, out hit, sensorLength * 0.5f))
        {
            if (!hit.collider.CompareTag("Terrain"))
            {
                Debug.DrawLine(rightSensorStartPos, hit.point, Color.red);
                avoiding = true;
                isWideRightAvailable = false;
                avoidMultiplier -= 0.25f;
            }
            else if (hit.collider.CompareTag("Car"))
            {
                isBraking = true;
            }
        }

        //Raycasting left side sensor
        if (Physics.Raycast(leftSensorStartPos, transform.forward, out hit, sensorLength))
        {
            if (!hit.collider.CompareTag("Terrain"))
            {
                Debug.DrawLine(leftSensorStartPos, hit.point, Color.red);
                avoiding = true;
                isLeftAvailable = false;
                avoidMultiplier += 1f;
            }
            else if (hit.collider.CompareTag("Car"))
            {
                isBraking = true;
            }
        }

        //Raycasting left angle side sensor
        else if (Physics.Raycast(leftSensorStartPos, leftAngleSensorDirection, out hit, sensorLength * 1.2f))
        {
            if (!hit.collider.CompareTag("Terrain"))
            {
                Debug.DrawLine(leftSensorStartPos, hit.point, Color.red);
                avoiding = true;
                isLeftAvailable = false;
                avoidMultiplier += 0.5f;
            }
            else if (hit.collider.CompareTag("Car"))
            {
                isBraking = true;
            }
        }

        //Raycasting wide left angle side sensor
        else if (Physics.Raycast(leftSensorStartPos, wideLeftAngleSensorDirection, out hit, sensorLength * 0.5f))
        {
            if (!hit.collider.CompareTag("Terrain"))
            {
                Debug.DrawLine(leftSensorStartPos, hit.point, Color.red);
                avoiding = true;
                isWideRightAvailable = false;
                avoidMultiplier += 0.25f;
            }
            else if (hit.collider.CompareTag("Car"))
            {
                isBraking = true;
            }
        }

        //Raycasting center sensor
        if (avoidMultiplier == 0)
        {
            if (Physics.Raycast(centerSensorStartPos, transform.forward, out hit, centerSensorLength))
            {
                if (!hit.collider.CompareTag("Terrain"))
                {
                    Debug.DrawLine(centerSensorStartPos, hit.point, Color.red);
                    avoiding = true;
                    if(hit.normal.x < 0)
                    {
                        avoidMultiplier += 1f;
                    }
                    else if (hit.normal.x > 0)
                    {
                        avoidMultiplier -= 1f;
                    }
                    else
                    {
                        //StartCoroutine(DriveBackwords());
                        avoidMultiplier += CheckAvailableSensors();
                    }
                }
                else if (hit.collider.CompareTag("Car"))
                {
                    isBraking = true;
                }
            }
        }

        if (avoiding)
        {
            currentSteer = wheels[0].steerAngle;
            float newSteerLocal = maxSteeringAngle * avoidMultiplier;
            wheels[0].steerAngle = Mathf.Lerp(currentSteer, newSteerLocal, steerDamp * Time.deltaTime);
            wheels[1].steerAngle = Mathf.Lerp(currentSteer, newSteerLocal, steerDamp * Time.deltaTime);
        }
    }

    float CheckAvailableSensors()
    {
        if (isWideLeftAvailable)
        {
            return -1f;
        }
        else if (isLeftAvailable)
        {
            return -0.5f;
        }

        if (isWideRightAvailable)
        {
            return 1f;
        }
        else if (isRightAvailable)
        {
            return 0.5f;
        }

        return Random.value;
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
        if(Vector3.Distance(transform.position, nodes[currentNode].position) < distanceToWayPoint)
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
