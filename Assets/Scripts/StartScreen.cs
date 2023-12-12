using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartScreen : MonoBehaviour
{
    public Button Tutorial;
    public Button Play;
    // Start is called before the first frame update
    void Start()
    {
        Tutorial.onClick.AddListener(RunTutorial);
        Play.onClick.AddListener(RunPlay);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void RunTutorial()
    {

    }

    void RunPlay()
    {
        SceneManager.LoadScene("YusefScene", LoadSceneMode.Single);
    }

}
