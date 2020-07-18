using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Resume : MonoBehaviour
{
    public GameObject hud;
    public GameObject pauseMenu;

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
        pauseMenu.SetActive(false);
    }
}
