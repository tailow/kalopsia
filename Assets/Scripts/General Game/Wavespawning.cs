using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wavespawning : MonoBehaviour
{

    public GameObject Hammerman;
    GameObject[] SpawnPoints;
    GameObject player;
    AudioSource aS;

    float runTime;
    int wave = 1;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        aS = GetComponent<AudioSource>();
        Invoke("spawnWave", 2);

        SpawnPoints = GameObject.FindGameObjectsWithTag("SpawnPoint");
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
            Vector3 spawnPosition;
            if (Random.Range(0, 100) < 90)
            {
                spawnPosition = Random.insideUnitSphere * Random.Range(40, 70);
                spawnPosition.y = 3;

            }
            else
            {
                spawnPosition = SpawnPoints[Random.Range(0, SpawnPoints.Length)].transform.position;
            }

            Instantiate(Hammerman, spawnPosition, Quaternion.identity);
        }

        aS.Play();
        wave += 1;
    }
}
