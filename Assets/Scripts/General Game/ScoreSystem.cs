using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ScoreSystem : MonoBehaviour
{
    
    float timeStart;
    float timeNow;

    float nextScoreTime = 5;
    public int score = 0;
    float cooldown = 5;

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

    // Update is called once per frame
    void Update()
    {
        /*
        timeNow = Time.time;
        if(Time.time > nextScoreTime)
        {
            nextScoreTime += cooldown;
            score += 1;
            ScoreUpdate();
        }
        */


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
