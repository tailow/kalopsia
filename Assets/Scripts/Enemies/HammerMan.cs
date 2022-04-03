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

    void Start()
    {
        timeStart = Time.time;
        player = GameObject.FindGameObjectWithTag("Player");
    }

    void Update()
    {
        Vector3 p1 = transform.position, p2 = player.transform.position;

        timeNow = Time.time;
        transform.LookAt(p2, Vector3.up);

        if (windupTime > Mathf.Abs(timeStart - timeNow)) { return; }

        float d = Vector3.Distance(p1, p2);
        //Debug.Log(d);

        if (d > 2.5f)
        {
            //look at player and move towards him
            transform.position = Vector3.MoveTowards( p1, p2, ms * Time.deltaTime);
            //transform.position += transform.forward * ms * Time.deltaTime;
        }
    }
}