using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;

public class HammerMan : MonoBehaviour
{
    // Start is called before the first frame update

    public float ms = 200;
    GameObject player = null;

    public float windupTime = 5;
    float timeStart;
    float timeNow;

    AudioSource aS;
    [SerializeField] AudioClip[] attacksoundList;

    Animator animator;
    NavMeshAgent agent;

    void Start()
    {
        timeStart = Time.time;
        player = GameObject.FindGameObjectWithTag("Player");
        aS = player.GetComponent<AudioSource>();
        agent = GetComponent<NavMeshAgent>();
        agent.autoTraverseOffMeshLink = true;
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("helikopter")) { return; }

        Vector3 p1 = transform.position, p2 = player.transform.position;

        timeNow = Time.time;
        transform.LookAt(p2, Vector3.up);

        if (windupTime > Mathf.Abs(timeStart - timeNow)) { return; }
        

        float d = Vector3.Distance(p1, p2);
        //Debug.Log(d);

        if (d > 2.5f)
        {
            //look at player and move towards him
            agent.destination = p2;
            //transform.position = Vector3.MoveTowards( p1, p2, ms * Time.deltaTime);
            //transform.position += transform.forward * ms * Time.deltaTime;
        }
        else
        {
            if (!aS.isPlaying)
            {
                aS.clip = attacksoundList[Random.Range(0, attacksoundList.Length)];
                aS.Play();
            }
            animator.Play("helikopter");
        }
    }
}