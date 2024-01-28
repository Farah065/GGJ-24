using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TutorialMenu : MonoBehaviour
{
    public void StartGame()
    {
        SceneManager.LoadScene("Settings");
    }

    void Update()
    {

    }
}
