using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndSceneManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scoreText;
    void Start()
    {
        scoreText.text = "Score: " + GameManager.Instance.score.ToString();
        Cursor.lockState = CursorLockMode.None;
    }

    public void PlayAgain()
    {
        GameManager.Instance.score = 0;
        GameManager.Instance.Difficulty = "";
        GameManager.Instance.enemySpeed = 0;
        SoundManager.Instance.stopAllAudio();
        SceneManager.LoadScene("MenuScene");
    }
}