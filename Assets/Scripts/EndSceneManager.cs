using TMPro;
using UnityEngine;

public class EndSceneManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scoreText;
    void Start()
    {
        scoreText.text = "Score: " + GameManager.Instance.score.ToString();
    }
}