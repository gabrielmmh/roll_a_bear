using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Menu : MonoBehaviour
{
    public void StartGame()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("RollaBear");
    }

    public void RestartGame()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("RollaBear");
    }

    public void GoToMenu()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Menu");
    }
}