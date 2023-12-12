using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.IO;

/*AUTHOR: YUSEF*/

public class QuestionManager : MonoBehaviour
{
    private List<Question> questions = new List<Question>();
    [SerializeField] private Canvas questionCanvas;
    [SerializeField] private TextMeshProUGUI questionText, firstAnswer, secondAnswer, thirdAnswer, fourthAnswer;
    [SerializeField] private GameObject questionCapsule;

    private bool isActive = true;

    private Question currQuestion;
    [SerializeField] private string questionsFile;

    private void LoadQuestions()
    {
        string[] lines = File.ReadAllLines(questionsFile);

        foreach(string line in lines)
        {
            if(line.StartsWith("#")) continue;

            string[] splitLine = line.Split(',');
            string question = splitLine[0];
            string[] answers = new string[4];
            for (int i = 0; i < 4; i++)
            {
                answers[i] = splitLine[i + 1];
            }
            int correctAnswerIndex = int.Parse(splitLine[5]);
            Question q = new Question(question, answers, correctAnswerIndex);
            questions.Add(q);
        }
    }

    void Start()
    {
        LoadQuestions();
    }

    void DisplayQuestion()
    {
        Question question = questions[Random.Range(0, questions.Count)];
        currQuestion = question;
        questionText.text = question.question;
        firstAnswer.text = question.answers[0];
        secondAnswer.text = question.answers[1];
        thirdAnswer.text = question.answers[2];
        fourthAnswer.text = question.answers[3];
        questionCanvas.gameObject.SetActive(true);
    }

    void HideCanvas()
    {
        questionCanvas.gameObject.SetActive(false);
    }

    void OnTriggerEnter(Collider other)
    {
        if(!isActive) {return;}
        DisplayQuestion();
    }

    void OnTriggerExit(Collider other)
    {
        HideCanvas();
    }

    public void OnAnswerButtonClick(int answerIndex)
    {
        if (currQuestion.correctAnswerIndex == answerIndex)
        {
            GameManager.Instance.score += 1;
            GameManager.Instance.enemySpeed -= 1;
            questionCapsule.GetComponentInChildren<Renderer>().material.color = Color.green;
        }
        else
        {
            questionCapsule.GetComponentInChildren<Renderer>().material.color = Color.red;
        }
        HideCanvas();
        StartCoroutine(Wait());
    }

    private IEnumerator Wait()
    {
        isActive = false;
        yield return new WaitForSeconds(Random.Range(1, 10));
        questionCapsule.GetComponentInChildren<Renderer>().material.color = Color.white;
        isActive = true;
    }
}