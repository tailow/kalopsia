using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuButton : MonoBehaviour
{
    public GameObject pauseMenu;

    public void StartGame(){
        SceneManager.LoadScene("scene_main");
    }

    public void MainMenu(){
        SceneManager.LoadScene("scene_menu");

        Time.timeScale = 1;
    }

    public void Resume(){
        if (pauseMenu){
            pauseMenu.SetActive(false);

            Cursor.lockState = CursorLockMode.Locked;

            Time.timeScale = 1;
        }
    }

    public void ExitGame(){
        Application.Quit();
    }
}
