using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameVictory : MonoBehaviour
{
    public GameObject winScreen; // Assign your Win Screen UI GameObject in the inspector

    // Start is called before the first frame update
    void Start()
    {
        // Ensure the win screen is not visible when the game starts
        winScreen.SetActive(false);
    }
    public void MainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Main Menu");
    }
    // Update is called once per frame
    void Update()
    {
        // When the 'L' key is pressed, display the win screen
        if (Input.GetKeyDown(KeyCode.L))
        {
            winScreen.SetActive(true);
        }
    }
}
