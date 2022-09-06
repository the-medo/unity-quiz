using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Quiz : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI headerText;
    [SerializeField] TextMeshProUGUI questionText;
    [SerializeField] TextMeshProUGUI questionCounterText;
    [SerializeField] TMP_InputField answersInputFieldText;
    
    [SerializeField] GameObject prevQuestionButton;
    [SerializeField] GameObject nextQuestionButton;
    [SerializeField] GameObject questionAnswers;
    [SerializeField] GameObject playerAnswers;

    [SerializeField] GameObject[] answerButtons;
    [SerializeField] GameObject[] playerResultRows;
    [SerializeField] QuestionSO[] allQuestions;
    [SerializeField] string[] players;

    QuestionSO question;
    int selectedQuestionIndex = 0;
    string[] answersToQuestions;
    bool showingResults = false;

    void Start()
    {
        answersToQuestions = new string[allQuestions.Length];

        for (int i = 0; i < playerResultRows.Length; i++) {
            if (i < players.Length) {
                playerResultRows[i].SetActive(true);
            } else {
                playerResultRows[i].SetActive(false);
            }
        }

        setSelectedQuestionIndex(selectedQuestionIndex);
    }

    void saveCurrentAnswers() {
        answersToQuestions[selectedQuestionIndex] = answersInputFieldText.text ?? "";
    }

    public void setSelectedQuestionIndex(int index) {
        saveCurrentAnswers();

        selectedQuestionIndex = index;
        question = allQuestions[index];

        nextQuestionButton.SetActive(selectedQuestionIndex < allQuestions.Length - 1);
        prevQuestionButton.SetActive(selectedQuestionIndex > 0);

        renderSelectedQuestion();
    }

    int getSelectedQuestionIndex() {
        return selectedQuestionIndex;
    }

    public void increaseSelectedQuestion() {
        if (selectedQuestionIndex + 1 < allQuestions.Length) {
            // 0:  1 < 3 => open 1
            // 1:  2 < 3 => open 2
            // 2:  3 < 3 => nothing
            setSelectedQuestionIndex(selectedQuestionIndex + 1);
        }
    }

    public void decreaseSelectedQuestion() {
        if (selectedQuestionIndex > 0) {
            setSelectedQuestionIndex(selectedQuestionIndex - 1);
        }
    }

    public void toggleShowResults() {
        showingResults = !showingResults;
        saveCurrentAnswers();
        renderSelectedQuestion();
    }

    public char intToAbcd(int answerInt) {
    //    Debug.Log("intToAbcd: " + answerInt + " => " + "abcd"[answerInt]); 
        return "abcd"[answerInt];
    }

    public int abcdToInt(char answerChar) {
        // Debug.Log("abcdToInt: " + answerChar + " => " + "abcd".IndexOf(answerChar));
        return "abcd".IndexOf(answerChar);
    }

    public bool isCorrectAnswer(QuestionSO q, char answerChar) {
        return intToAbcd(q.GetCorrectAnswerIndex()) == answerChar;
    }

    public int[] playerPoints() {
        int[] playerPointsResult = new int[players.Length];
        for (int playerIndex = 0; playerIndex < players.Length; playerIndex++) {
            playerPointsResult[playerIndex] = 0;
        }

        for (int questionIndex = 0; questionIndex <= selectedQuestionIndex; questionIndex++) {
            for (int playerIndex = 0; playerIndex < players.Length; playerIndex++) {
                playerPointsResult[playerIndex] += isCorrectAnswer(allQuestions[questionIndex], answersToQuestions[questionIndex][playerIndex]) ? 1 : 0;
            }
        }

        return playerPointsResult;
    }

    void renderSelectedQuestion() {
        
        questionText.text = question.GetQuestion();
        questionCounterText.text = (selectedQuestionIndex + 1) + "/" + allQuestions.Length;
        headerText.text = "2H2H Quiz - " + question.GetSubjectOfQuestion();

        playerAnswers.SetActive(showingResults);
        questionAnswers.SetActive(!showingResults);
        
        answersInputFieldText.text = answersToQuestions[selectedQuestionIndex] ?? "";

        if (showingResults) {

            int[] pp = playerPoints();

            for (int i = 0; i < players.Length; i++) {
                playerResultRows[i].SetActive(true);
                foreach(Transform child in playerResultRows[i].transform) {
                    TextMeshProUGUI textComponent = child.gameObject.GetComponent<TextMeshProUGUI>();
                    char playerAbcdAnswer = answersToQuestions[selectedQuestionIndex][i];

                    if(child.tag == "PlayerName") {
                        textComponent.text = players[i];
                    } else if(child.tag == "PlayerAnswer") {
                        textComponent.text = question.GetAnswer(abcdToInt(answersToQuestions[selectedQuestionIndex][i]));
                    } else if(child.tag == "AnswerPoints") {
                        textComponent.text = isCorrectAnswer(question, playerAbcdAnswer) ? "YES = 1 bod" : "NO = 0 bodov";
                    } else if(child.tag == "PlayerPoints") {
                        textComponent.text = pp[i] + " bodov";
                    }
                }
            }
        } else {

            for (int i = 0; i < answerButtons.Length; i++) {
                
                string pathToAnswerImage = question.GetPathToAnswerImage(i);
                bool imageExists = pathToAnswerImage != "none" ? true : false;

                Image imageComponent = null;            

                foreach(Transform child in answerButtons[i].transform)
                {
                    if(child.tag == "QuestionImage") {
                        child.gameObject.SetActive(imageExists);
                        if (imageExists) {
                            imageComponent = child.GetComponentInChildren<Image>();
                            if (imageComponent != null) {
                                imageComponent.overrideSprite = Resources.Load<Sprite>(pathToAnswerImage);
                            }
                        }
                    } else if(child.tag == "QuestionImageText") {
                        child.gameObject.SetActive(imageExists);
                    } else if(child.tag == "QuestionText") {
                        child.gameObject.SetActive(!imageExists);
                    }
                }

                TextMeshProUGUI[] buttonTexts = answerButtons[i].GetComponentsInChildren<TextMeshProUGUI>();
                foreach(TextMeshProUGUI buttonText in buttonTexts) {
                    buttonText.text = question.GetAnswer(i);
                }
                
            }
        }
    }
}