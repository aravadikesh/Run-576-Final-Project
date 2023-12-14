/* AUTHOR: YUSEF*/

using TMPro;
using UnityEngine;

public class ScoreUpdater : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scoreText;

    void Start()
    {
        scoreText.text = "Score: " + GameManager.Instance.score.ToString();
    }

    public void MenuScene()
    {
        GameManager.Instance.MenuScene();
    }
}