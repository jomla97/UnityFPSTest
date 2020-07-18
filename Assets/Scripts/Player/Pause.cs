using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Pause : MonoBehaviour
{
    public GameObject hud;
    public GameObject pauseMenu;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (Time.timeScale > 0)
            {
                PauseGame();
            }
        }
    }

    void PauseGame()
    {
        Debug.Log("Pause");

        Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.None;
        hud.SetActive(false);
        pauseMenu.SetActive(true);
    }
}
