using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseManu : MonoBehaviour
{
    public GameObject pauseMenu;
    public PlayerController playerController;
    public PlayerCombat playerCombat;

    //public GameObject Crosshair;

    public static bool isPaused;
    // Start is called before the first frame update
    void Start()
    {
        pauseMenu.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }

    }
    public void PauseGame()
    {
        pauseMenu.SetActive(true);
        //Crosshair.SetActive(false);
        playerCombat.enabled = false;
        playerController.enabled = false;
        Time.timeScale = 0f;
        isPaused = true;

    }

    public void ResumeGame()
    {
        pauseMenu.SetActive(false);
        //Crosshair.SetActive(true);
        playerCombat.enabled = true;
        playerController.enabled = true;
        Time.timeScale = 1f;
        isPaused = false;

    }

    public void MainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(0);
    }
    public void QuitGame()
    {
        Application.Quit();
    }
}
