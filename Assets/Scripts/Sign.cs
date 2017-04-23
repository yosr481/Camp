using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sign : MonoBehaviour {
    [Tooltip("Used to organized the questions by the sign category")]
    public string signCategory;
    public List<Question> questions;
    public bool isSpawnable =false;

    QuestionHolder questionHolder;
	// Use this for initialization
	void Awake () {
        questionHolder = GameObject.FindObjectOfType<QuestionHolder>();

        FindAndAddQuestions();
        CheckIfSpawnable();
    }

    void FindAndAddQuestions()
    {
        foreach (Question q in questionHolder.allQuestions)
        {
            if (q.category == signCategory)
            {
                questions.Add(q);
            }
        }
    }
	
	void CheckIfSpawnable()
    {
        if (questions.Count > 0)
            isSpawnable = true;
        else
            isSpawnable = false;
    }
}
