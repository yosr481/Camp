using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Question {
    public string category;
    public int difficulty;
    public Sprite question;
    public Sprite rightAnswer;
    public Sprite[] wrongAnswers = new Sprite[3];
}
