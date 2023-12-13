/*AUTHOR: EYAL*/

using UnityEngine;

public class PauseManager : MonoBehaviour
{
    public bool isPaused = false;
    
    public GameObject pauseMenu;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Pause();
        }

        if (Input.GetKeyDown(KeyCode.Q) && isPaused)
        {
            Quit();
        }
    }

    public void Pause()
    {
        isPaused = !isPaused;
        if (isPaused)
        {
            pauseMenu.SetActive(true);
            Time.timeScale = 0;
        }
        else
        {
            pauseMenu.SetActive(false);
            Time.timeScale = 1;
        }
    }

    private void Quit()
    {
        Application.Quit();
    }
}