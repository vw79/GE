using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOver : MonoBehaviour
{
    public GameObject LoseScreen; // Assign your Win Screen GameObject in the inspector
    void Start()
    {
        // Ensure the win screen is not visible when the game starts
        LoseScreen.SetActive(false);
    }
    public void MainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Main Menu");
    }
    void Update()
    {
        // When the 'L' key is pressed, display the win screen
        if (Input.GetKeyDown(KeyCode.N))
        {
            LoseScreen.SetActive(true);
        }
    }
}
