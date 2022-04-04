using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public GameObject pauseMenu;
    public GameObject deathScreen;

    void Start()
    {
        Cursor.visible = false;

        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update(){
        if (Input.GetButtonDown("Exit") && pauseMenu && !deathScreen.activeInHierarchy){
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

    public void GameOver()
    {
        Time.timeScale = 0;

        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;

        int minutes = Mathf.FloorToInt(Time.timeSinceLevelLoad / 60f);
        int seconds = (int)Time.timeSinceLevelLoad % 60;

        deathScreen.SetActive(true);

        deathScreen.transform.Find("TimeText").GetComponent<TMP_Text>().text = "Time alive: " + minutes.ToString() + "m " + seconds.ToString() + "s";
        deathScreen.transform.Find("ScoreText").GetComponent<TMP_Text>().text = "Score: " + GetComponent<ScoreSystem>().score;
    }
}
