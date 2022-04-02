using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriangleCaster : MonoBehaviour
{
    public float cooldown;

    Transform playerCamera;
    LineRenderer lineRenderer;

    int layerMask;
    int pointIndex;

    float lastTriangleTime;

    void Start(){
        playerCamera = Camera.main.transform;

        lineRenderer = GetComponent<LineRenderer>();
    }

    void Update(){
        RaycastHit hit;

        if (Physics.Raycast(playerCamera.position, playerCamera.forward, out hit, Mathf.Infinity)){
            lineRenderer.SetPosition(pointIndex, hit.point);
        }

        if (Input.GetMouseButtonDown(0) && Time.time - lastTriangleTime > cooldown){
            lineRenderer.enabled = true;

            lineRenderer.positionCount++;

            pointIndex++;

            if (pointIndex == 3){
                lastTriangleTime = Time.time;

                ResetTriangle();
            }
        }

        if (Input.GetMouseButtonDown(1)){
            ResetTriangle();
        }
    }

    void ResetTriangle(){
        lineRenderer.positionCount = 1;

        lineRenderer.SetPosition(0, Vector3.zero);

        lineRenderer.enabled = false;

        pointIndex = 0;
    }
}
