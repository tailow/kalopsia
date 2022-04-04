using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ScoreSystem : MonoBehaviour
{
    float timeStart;

    public int score = 0;

    public delegate void OnVariableChangeDelegate(int newVal);
    public event OnVariableChangeDelegate OnVariableChange;

    [SerializeField] TMP_Text scoretext;
    [SerializeField] AudioClip[] scoreAudios;
    AudioSource aS;

    void Start()
    {
        aS = GetComponent<AudioSource>();
        timeStart = Time.time;
    }

    public void AddScore(int amount)
    {
        score += amount;
        ScoreUpdate();
    }

    void ScoreUpdate()
    {
        int len = score.ToString().Length;
        string s = score.ToString();

        if(len < 4)
        {
            for(int i = 0; i < 4 - len; i++)
            {
                s = "0" + s;
            }
        }

        scoretext.text = s;

        if (!aS.isPlaying)
        {
            aS.clip = scoreAudios[Random.Range(0, scoreAudios.Length)];
            aS.Play();
        }
    }
}
