using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject pauseMenu;

    void Start()
    {
        Cursor.visible = false;

        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update(){
        if (Input.GetButtonDown("Exit") && pauseMenu){
            if (!pauseMenu.activeInHierarchy){
                pauseMenu.SetActive(true);

                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.Confined;

                Time.timeScale = 0;
            }

            else if (pauseMenu.activeInHierarchy){
                pauseMenu.SetActive(false);

                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;

                Time.timeScale = 1;
            }
        }
    }
}
