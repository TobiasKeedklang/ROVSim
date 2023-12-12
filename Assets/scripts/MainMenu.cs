using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    // Start is called before the first frame update
    public void playGame()
    {
        SceneManager.LoadScene("Task1");
    }

    public void Controls()
    {
        SceneManager.LoadScene("ControlMenu");
    }
    public void ReturnToTitle()
    {
        SceneManager.LoadScene("Menu");
    }

    public void quitFunc()
    {
        Application.Quit();
    }
}
