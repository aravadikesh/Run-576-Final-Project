/*AUTHOR: YUSEF*/

using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public int score = 0;
    public int lives = 3;
    public int enemySpeed = 1;
    public string Difficulty;

    private void Awake()
    {
        if (Instance == null) { Instance = this; DontDestroyOnLoad(gameObject);}
        else { Destroy(gameObject); }
    }

    public void GameOver()
    {
        SceneManager.LoadScene("EndScene");
    }

    public void MenuScene()
    {
        SceneManager.LoadScene("MenuScene");
    }
}