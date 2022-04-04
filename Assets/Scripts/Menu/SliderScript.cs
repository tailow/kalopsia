using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class SliderScript : MonoBehaviour
{
    public AudioMixer mixer;

    public PlayerMovement movement;

    void Start(){
        if (PlayerPrefs.GetFloat("audioVolume") == 0f){
            PlayerPrefs.SetFloat("audioVolume", -10f);
        }

        if (PlayerPrefs.GetFloat("sensitivity") == 0f){
            PlayerPrefs.SetFloat("sensitivity", 10f);
        }

        if (gameObject.name == "VolumeSlider")
        {
            GetComponent<Slider>().SetValueWithoutNotify(PlayerPrefs.GetFloat("audioVolume"));

            mixer.SetFloat("audioVolume", GetComponent<Slider>().value);
        }
        else if (gameObject.name == "SensitivitySlider")
        {
            GetComponent<Slider>().SetValueWithoutNotify(PlayerPrefs.GetFloat("sensitivity"));
        }
    }

    public void ChangeVolume(){
        mixer.SetFloat("audioVolume", GetComponent<Slider>().value);

        PlayerPrefs.SetFloat("audioVolume", GetComponent<Slider>().value);
    }

    public void ChangeSensitivity(){
        if (movement){
            movement.sensitivity = GetComponent<Slider>().value;
        }

        PlayerPrefs.SetFloat("sensitivity", GetComponent<Slider>().value);
    }
}
