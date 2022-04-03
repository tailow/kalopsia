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
        if (gameObject.name == "VolumeSlider")
        {
            GetComponent<Slider>().SetValueWithoutNotify(PlayerPrefs.GetFloat("audioVolume"));
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
