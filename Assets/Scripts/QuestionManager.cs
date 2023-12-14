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

    [SerializeField] private Transform parentPosition, tipPosition;

    [SerializeField] private LineRenderer lineRenderer;

    private bool isActive = false;

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
        questionsFile = "Assets/" + GameManager.Instance.Difficulty + "Questions.txt";
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
        isActive = true;
        DisplayQuestion();
    }

    void OnTriggerExit(Collider other)
    {
        isActive = false;
        HideCanvas();
    }

    void Update()
    {
        //manage waypoint
        lineRenderer.SetPosition(0, tipPosition.position);
        lineRenderer.SetPosition(1, tipPosition.position + new Vector3(0, 800, 0));

        if (!isActive) return; //only check input if player in radius

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            OnAnswerSelect(0);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            OnAnswerSelect(1);
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            OnAnswerSelect(2);
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            OnAnswerSelect(3);
        }
    }

    public void OnAnswerSelect(int answerIndex)
    {
        if (currQuestion.correctAnswerIndex == answerIndex)
        {
            GameManager.Instance.score += 1;
            GameManager.Instance.enemySpeed = Mathf.Max(0, GameManager.Instance.enemySpeed - 2);
            SoundManager.Instance.playEffect("Success");
        }
        else
        {
            GameManager.Instance.score -= 1;
            GameManager.Instance.enemySpeed = Mathf.Max(0, GameManager.Instance.enemySpeed - 2);
            SoundManager.Instance.playEffect("Fail");
        }
        HideCanvas();
        
        parentPosition.position = new Vector3(Random.Range(0,250), 0, Random.Range(0,250));
        parentPosition.GetComponent<Objects>().FindLand();
    }
}