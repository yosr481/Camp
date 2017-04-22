using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestionHolder : MonoBehaviour {

    public Question[] allQuestions;
    [HideInInspector]
    public List<Question> sevenseventy;
    [HideInInspector]
    public List<Question> books;
    [HideInInspector]
    public List<Question> army;
    [HideInInspector]
    public List<Question> stop;
    [HideInInspector]
    public List<Question> ten;
    [HideInInspector]
    public List<Question> transition;
    [HideInInspector]
    public List<Question> uTurn;
    [HideInInspector]
    public List<Question> cop;
    [HideInInspector]
    public List<Question> funFact;

    // Use this for initialization
    void Start () {
        FillingLists();

    }

    void FillingLists()
    {
        foreach (Question q in allQuestions)
        {
            if (q.category == "770")
            {
                sevenseventy.Add(q);
            }
            else if (q.category == "Books")
            {
                books.Add(q);
            }
            else if (q.category == "Army")
            {
                army.Add(q);
            }
            else if (q.category == "Stop")
            {
                stop.Add(q);
            }
            else if (q.category == "10")
            {
                ten.Add(q);
            }
            else if (q.category == "Transition")
            {
                transition.Add(q);
            }
            else if (q.category == "U-Turn")
            {
                uTurn.Add(q);
            }
            else if (q.category == "Cop")
            {
                cop.Add(q);
            }
            else if (q.category == "Fun-Fact")
            {
                funFact.Add(q);
            }
        }
    }
}
