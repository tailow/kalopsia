using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.AI;

public class HammerMan : MonoBehaviour
{
    bool playerOnly = false;
    public float ms = 200;
    GameObject player = null;

    Vector3 targetPosition;

    Vector3[] targets;

    public float windupTime = 5;
    public float attackRange;
    public float attackCooldown;

    public int damage;

    float timeStart;
    float timeNow;

    float lastAttack = -1000;

    AudioSource aS;
    [SerializeField] AudioClip[] attacksoundList;

    Animator animator;
    NavMeshAgent agent;

    void Start()
    {
        timeStart = Time.time;
        player = GameObject.FindGameObjectWithTag("Player");

        aS = GetComponent<AudioSource>();
        animator = GetComponent<Animator>();

        agent = GetComponent<NavMeshAgent>();
        agent.autoTraverseOffMeshLink = true;

        targets = new Vector3[] {player.transform.position, new Vector3(8, 2.5f, 8), new Vector3(8, 2.5f, -8), new Vector3(-8, 2.5f, 8), new Vector3(-8, 2.5f, -8)};

        if(SceneManager.GetActiveScene().name == "scene_main" && agent.isOnNavMesh)
        {
            NavMeshPath path = new NavMeshPath();
            agent.CalculatePath(new Vector3(10, 2, 10), path);
            if (path.status == NavMeshPathStatus.PathPartial) { Destroy(gameObject); }
        }
        else {
            Destroy(gameObject);
        }

        SelectTarget();
    }

    void Update()
    {
        targets[0] = player.transform.position;

        if (animator.GetCurrentAnimatorStateInfo(0).IsName("helikopter")) { return; }

        Vector3 p1 = transform.position, p2 = player.transform.position;

        timeNow = Time.time;
        transform.LookAt(new Vector3(targetPosition.x, 2, targetPosition.z), Vector3.up);

        if (windupTime > Mathf.Abs(timeStart - timeNow)) { return; }


        if(Vector3.Distance(p1, targetPosition) > attackRange) { SelectTarget(); }
        else if (Time.time - lastAttack > attackCooldown)
        {
            // Attack
            Collider[] colliders = Physics.OverlapSphere(p1, attackRange);

            foreach (Collider coll in colliders)
            {
                if (coll.CompareTag("Player")){
                    if (!aS.isPlaying)
                    {
                        aS.clip = attacksoundList[Random.Range(0, attacksoundList.Length)];
                        aS.Play();
                    }

                    coll.gameObject.GetComponent<Health>().TakeDamage(damage);

                    animator.Play("helikopter");

                    lastAttack = Time.time;
                }
                else if (coll.CompareTag("Core")){
                    coll.gameObject.GetComponent<Health>().TakeDamage(damage);

                    animator.Play("helikopter");

                    lastAttack = Time.time;
                }
            }
        }
    }

    public void GetAngry()
    {
        animator.enabled = false;
        animator.enabled = true;
        animator.Play("wiggle");
    }

    void SelectTarget(){
        targetPosition = player.transform.position;

        foreach (Vector3 pos in targets)
        {
            if (Vector3.Distance(transform.position, pos) < Vector3.Distance(transform.position, targetPosition)){
                targetPosition = pos;
            }   
        }

        if (agent.isOnNavMesh && agent.isActiveAndEnabled)
        {
            agent.SetDestination(targetPosition);
        }
    }
}