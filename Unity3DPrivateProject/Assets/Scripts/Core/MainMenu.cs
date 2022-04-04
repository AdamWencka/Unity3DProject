using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField]
    private GameObject MenuUI;
    [SerializeField]
    private GameObject tutorialMenuUI;


    public void PlayGame()
    {
        SceneManager.LoadScene("ThirdPersonShooter");
    }

    public void LoadTutorial()
    {
        MenuUI.SetActive(false);
        tutorialMenuUI.SetActive(true);
    }
    public void BackToMenu()
    {
        MenuUI.SetActive(true);
        tutorialMenuUI.SetActive(false);
    }
    public void QuitGame()
    {
        Application.Quit();
    }
}
