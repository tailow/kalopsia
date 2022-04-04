using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuButton : MonoBehaviour
{
    public GameObject pauseMenu;

    public AudioSource selectSource;

    public void StartGame(){
        Time.timeScale = 1;

        SceneManager.LoadScene("scene_main");
    }

    public void MainMenu(){
        Time.timeScale = 1;

        selectSource.Play();

        SceneManager.LoadScene("scene_menu");
    }

    public void Resume(){
        if (pauseMenu){
        
            Time.timeScale = 1;

            selectSource.Play();

            pauseMenu.SetActive(false);

            Cursor.lockState = CursorLockMode.Locked;
        }
    }

    public void ExitGame(){
        Application.Quit();
    }
}
