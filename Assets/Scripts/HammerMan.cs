using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HammerMan : MonoBehaviour
{
    // Start is called before the first frame update

    public float ms = 200;
    GameObject player = null;

    public float windupTime = 5;
    public float timeStart;
    public float timeNow;

    public Animator animator;

    void Start()
    {
        timeStart = Time.time;
        player = GameObject.FindGameObjectWithTag("Player");
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        timeNow = Time.time;
        transform.LookAt(player.transform);

        if (windupTime > Mathf.Abs(timeStart - timeNow)) { return; }

        float d = Vector3.Distance(transform.position, player.transform.position);
        Debug.Log(d);

        if (d > 2.5f)
        {
            //look at player and move towards him
            transform.position += transform.forward * ms * Time.deltaTime;
        }
        else
        {
            // play animation attack
            animator.SetBool("attack", true);
        }
    }
}
