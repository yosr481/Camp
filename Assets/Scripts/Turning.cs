using UnityEngine;
using System.Collections;

public class Turning : MonoBehaviour {
    public Vector3 offset;
    public float speed;

    Vector3 rotationCamera;

    void Awake()
    {
        rotationCamera = new Vector3(offset.x, offset.y, offset.z);
    }

	void FixedUpdate () {
        transform.Rotate(rotationCamera, Time.deltaTime * speed);
	}
}
