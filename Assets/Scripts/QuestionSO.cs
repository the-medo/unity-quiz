using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Question", menuName = "Quiz Question")]
public class QuestionSO : ScriptableObject {
    [TextArea(2,5)] [SerializeField] string question = "Enter new question text here";


}
