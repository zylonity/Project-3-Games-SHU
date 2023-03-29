using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Buttons : MonoBehaviour
{
    //ya v ahue pizdez blyat'
    public void PlayButton()
    {
        Time.timeScale = 1.0f;
    }
    public void StartGameButton()
    {
        Time.timeScale = 1.0f;
        SceneManager.LoadScene(1);
    }

    public void MainMenu()
    {
        SceneManager.LoadScene(0);
    }

}
