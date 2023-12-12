/*AUTHOR: EYAL*/

using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartScreen : MonoBehaviour
{
    public Button Tutorial, Play, Back;
    [SerializeField] private GameObject tutorialScreen;
    [SerializeField] private GameObject startScreen;
    // Start is called before the first frame update
    void Start()
    {
        Tutorial.onClick.AddListener(RunTutorial);
        Play.onClick.AddListener(RunPlay);
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

    void RunPlay()
    {
        SceneManager.LoadScene("GameScene", LoadSceneMode.Single);
    }
}
