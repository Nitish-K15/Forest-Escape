using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class Menus : MonoBehaviour
{
    public delegate void ClickAction();
    public static event ClickAction OnClicked;
    public void TryAgain()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void ToMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void Exit()
    {
        Application.Quit();
    }

    public void Game()
    {
        SceneManager.LoadScene("Game");
    }

    public void Retry()
    {
        OnClicked();
    }
}
