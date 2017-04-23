using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    public Sign[] signs;

    public List<GameObject> signsPrefabs;
	// Use this for initialization
	void Start () {
		for(int i = 0; i < signs.Length; i++)
        {
            if (signs[i].isSpawnable)
            {
                signsPrefabs.Add(signs[i].gameObject);
                Debug.Log(signs[i].gameObject.name + " is spawnable");
            }
        }
    }
	
	// Update is called once per frame
	void Update () {
        

	}
}
