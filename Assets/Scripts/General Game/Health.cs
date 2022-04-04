using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Health : MonoBehaviour
{
    public int health;

    public int baseHealth;

    public Slider healthBar;

    public GameObject deathScreen;

    [SerializeField] private AudioSource source;

    void Start()
    {
        health = baseHealth;

        if (healthBar)
        {
            healthBar.value = (float)health / baseHealth;
        }
    }

    public void TakeDamage(int amount)
    {
        health -= amount;

        if (healthBar)
        {
            healthBar.value = (float)health / baseHealth;
        }

        if (source)
        {
            source.Play();
        }

        if (health <= 0)
        {
            Death();
        }
    }

    void Death()
    {
        if (gameObject.tag == "Player")
        {
            GameObject.FindGameObjectWithTag("GM").GetComponent<GameManager>().GameOver();
        }
        else if(gameObject.tag == "Enemy")
        {
            GameObject.FindGameObjectWithTag("GM").GetComponent<ScoreSystem>().AddScore(5);

            Destroy(gameObject.transform.parent.gameObject);
        }
        else if (gameObject.tag == "Core")
        {
            GameObject.FindGameObjectWithTag("GM").GetComponent<GameManager>().GameOver();
        }
        else
        {
            Destroy(gameObject);
        }
    }
}