using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Path : MonoBehaviour {
    public Color gizmosColor;

    Transform[] wayPoints;
    List<Transform> nodes;
    // Use this for initialization
    void Start () {
        wayPoints = GetComponentsInChildren<Transform>();
        nodes = new List<Transform>();

        for (int i = 1; i < wayPoints.Length; i++)
        {
            nodes.Add(wayPoints[i]);
        }
    }
	
	// Update is called once per frame
	void Update() {
		for(int i = 0; i < nodes.Count; i++)
        {
            Vector3 currentNode = nodes[i].position;
            Vector3 prevNode = Vector3.zero;

            if(i > 0)
            {
                prevNode = nodes[i - 1].position;
            }
            else if(i == 0 && nodes.Count > 1)
            {
                prevNode = nodes[nodes.Count - 1].position;
            }

            Debug.DrawLine(prevNode, currentNode, gizmosColor);
        }
	}
}
