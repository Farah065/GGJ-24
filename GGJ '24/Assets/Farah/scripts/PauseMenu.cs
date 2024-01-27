using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public bool isPaused;
    private GameObject pauseMenuUI;

    void Awake()
    {
        isPaused = false;
        pauseMenuUI = gameObject.transform.Find("Pause Menu").gameObject;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyUp(KeyCode.P))
        {
            if (!isPaused)
            {
                pause();
            }
            else
            {
                unpause();
            }
        }
    }

    private void pause()
    {
        isPaused = true;
        Time.timeScale = 0;
        pauseMenuUI.SetActive(true);
    }

    private void unpause()
    {
        isPaused = false;
        Time.timeScale = 1;
        pauseMenuUI.SetActive(false);
    }

    public void restart()
    {
        string currentScene = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(currentScene);
        unpause();
    }

    public void menu()
    {
        SceneManager.LoadScene("Main Menu");
        unpause();
    }

    public void resume()
    {
        unpause();
    }
}
