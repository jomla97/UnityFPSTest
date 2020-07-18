using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public GameObject hud;
    public GameObject canvas;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ResumeGame();
        }
    }

    public void ResumeGame()
    {
        Debug.Log("Resume");

        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.Locked;
        hud.SetActive(true);
        canvas.SetActive(false);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
