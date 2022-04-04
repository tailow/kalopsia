using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wavespawning : MonoBehaviour
{

    public GameObject Hammerman;
    GameObject player;
    AudioSource aS;

    float runTime;
    int wave = 1;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        aS = GetComponent<AudioSource>();
        Invoke("spawnWave", 2);
    }

    void Update()
    {
        runTime += Time.deltaTime;
        
        if (runTime > 16 * wave){ spawnWave(); }
    }

    void spawnWave()
    {
        for (int i = 0; i < 4 + wave * 2; i++)
        {
            Vector3 randomPosition = Random.insideUnitSphere * Random.Range(40, 70);
            randomPosition.y = 3;
            Instantiate(Hammerman, randomPosition, Quaternion.identity);
        }

        aS.Play();
        wave += 1;
    }
}
