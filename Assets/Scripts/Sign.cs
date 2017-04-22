using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sign : MonoBehaviour {

    public string signCategory;

    QuestionHolder questionHolder;
	// Use this for initialization
	void Start () {
        questionHolder = GameObject.FindObjectOfType<QuestionHolder>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
