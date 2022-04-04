using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.AI;

public class HammerMan : MonoBehaviour
{
    public float ms = 200;
    GameObject player = null;

    GameObject core;

    Vector3 targetPosition;

    Vector3[] targets;

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
        core = GameObject.FindGameObjectWithTag("Core");

        aS = player.GetComponent<AudioSource>();
        animator = GetComponent<Animator>();

        agent = GetComponent<NavMeshAgent>();
        agent.autoTraverseOffMeshLink = true;

        targets = new Vector3[] {player.transform.position, new Vector3(7, 2, 7), new Vector3(7, 2, -7), new Vector3(-7, 2, 7), new Vector3(-7, 2, -7)};

        if(SceneManager.GetActiveScene().name == "scene_main")
        {
            NavMeshPath path = new NavMeshPath();
            agent.CalculatePath(new Vector3(1123, 2, 1000), path);
            if (path.status == NavMeshPathStatus.PathPartial) { Destroy(gameObject); }
        }
    }

    void Update()
    {
        targets[0] = player.transform.position;

        if (animator.GetCurrentAnimatorStateInfo(0).IsName("helikopter")) { return; }

        Vector3 p1 = transform.position, p2 = player.transform.position;

        timeNow = Time.time;
        transform.LookAt(new Vector3(targetPosition.x, 2, targetPosition.z), Vector3.up);

        if (windupTime > Mathf.Abs(timeStart - timeNow)) { return; }

        float d = Vector3.Distance(p1, p2);
        //Debug.Log(d);

        if (d > 2.5f)
        {
            //look at player and move towards him
            SelectTarget();
            //transform.position = Vector3.MoveTowards( p1, p2, ms * Time.deltaTime);
            //transform.position += transform.forward * ms * Time.deltaTime;
        }
        else
        {
            if (!aS.isPlaying)
            {
                aS.clip = attacksoundList[Random.Range(0, attacksoundList.Length)];
                aS.Play();
                
                //player.GetComponent<Health>().TakeDamage(25);
            }
            animator.Play("helikopter");

        }
    }

    void SelectTarget(){
        targetPosition = player.transform.position;

        foreach (Vector3 pos in targets)
        {
            if (Vector3.Distance(transform.position, pos) <= (Vector3.Distance(transform.position, targetPosition))){
                targetPosition = pos;
            }   
        }

        agent.SetDestination(targetPosition);
    }
}