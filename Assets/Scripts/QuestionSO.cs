using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Question", menuName = "Quiz Question")]
public class QuestionSO : ScriptableObject {
    [SerializeField] string subjectOfQuestion = "Subject of question";
    [TextArea(2,5)] [SerializeField] string question = "Enter new question text here";
    [SerializeField] string[] answers = new string[4];
    [SerializeField] string[] pathToAnswerImages = new string[4];
    [SerializeField] int correctAnswerIndex;
    
    public string GetSubjectOfQuestion() {
        return subjectOfQuestion;
    }

    public string GetQuestion() {
        return question;
    }

    public string GetAnswer(int index) {
        return answers[index];
    }

    public string GetPathToAnswerImage(int index) {
        return pathToAnswerImages[index];
    }

    public int GetCorrectAnswerIndex() {
        return correctAnswerIndex;
    }

}