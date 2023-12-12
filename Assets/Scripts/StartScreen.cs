/*AUTHOR: EYAL*/

using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartScreen : MonoBehaviour
{
    public Button Tutorial, EasyPlayButton, MediumPlayButton, HardPlayButton, Back;
    [SerializeField] private GameObject tutorialScreen;
    [SerializeField] private GameObject startScreen;
    // Start is called before the first frame update
    void Start()
    {
        Tutorial.onClick.AddListener(RunTutorial);
        EasyPlayButton.onClick.AddListener(EasyPlay);
        MediumPlayButton.onClick.AddListener(MediumPlay);
        HardPlayButton.onClick.AddListener(HardPlay);
        Back.onClick.AddListener(BackButton);
    }

    void RunTutorial()
    {
        tutorialScreen.SetActive(true);
        startScreen.SetActive(false);
    }

    public void BackButton()
    {
        Debug.Log("Back Button");
        tutorialScreen.SetActive(false);
        startScreen.SetActive(true);
    }

    void EasyPlay()
    {
        GameManager.Instance.Difficulty = "easy";
        SceneManager.LoadScene("GameScene", LoadSceneMode.Single);
    }

    void MediumPlay()
    {
        GameManager.Instance.Difficulty = "medium";
        SceneManager.LoadScene("GameScene", LoadSceneMode.Single);
    }

    void HardPlay()
    {
        GameManager.Instance.Difficulty = "hard";
        SceneManager.LoadScene("GameScene", LoadSceneMode.Single);
    }
}
