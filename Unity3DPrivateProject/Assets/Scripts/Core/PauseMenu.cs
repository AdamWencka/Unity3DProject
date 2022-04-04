using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public static bool GameIsPaused = false;

    [SerializeField]
    private PlayerInput playerInput;

    [SerializeField]
    private GameObject pauseMenuUI;
    [SerializeField]
    private GameObject tutorialMenuUI;
    [SerializeField]
    private GameObject resumeButton;
    [SerializeField]
    private GameObject aimCanvas;
    [SerializeField]
    private GameObject thirdPersonCanvas;

    [SerializeField]
    private PlayerController playerController;
    [SerializeField]
    private Gun gun;
    private bool hasLost;

    private InputAction pauseAction;

    private void Awake()
    {
        pauseAction = playerInput.actions["Pause"];
    }
    private void Update()
    {
        if (pauseAction.triggered && !hasLost)
        {
            if (GameIsPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        tutorialMenuUI.SetActive(false);
        Time.timeScale = 1f;
        GameIsPaused = false;
        aimCanvas.SetActive(true);
        thirdPersonCanvas.SetActive(true);
        playerController.enabled = true;
        gun.enabled = true;
    }
    void Pause()
    {
        pauseMenuUI.SetActive(true);
        tutorialMenuUI.SetActive(false);
        Time.timeScale = 0f;
        GameIsPaused = true;
        aimCanvas.SetActive(false);
        thirdPersonCanvas.SetActive(false);
        playerController.enabled = false;
        gun.enabled = false;
        Cursor.lockState = CursorLockMode.None;
    }
    public void LoadMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Menu");
    }
    public bool getHasLost()
    {
        return hasLost;
    }
    public void GameOver()
    {
        hasLost = true;
        Invoke("Delay", 1.5f);
    }

    void Delay()
    {
        Pause();
        resumeButton.SetActive(false);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void LoadTutorial()
    {
        pauseMenuUI.SetActive(false);
        tutorialMenuUI.SetActive(true);
    }
    public void LoadPauseMenu()
    {
        Pause();
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        Time.timeScale = 1f;
    }
}
