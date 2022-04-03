using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuButton : MonoBehaviour
{
    public void StartGame(){
        SceneManager.LoadScene("scene_main");
    }

    public void ExitGame(){
        Application.Quit();
    }
}
