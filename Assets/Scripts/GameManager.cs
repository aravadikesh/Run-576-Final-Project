/*AUTHOR: YUSEF*/

using UnityEngine;

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
}