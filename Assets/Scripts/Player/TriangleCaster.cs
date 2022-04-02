using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriangleCaster : MonoBehaviour
{
    public float cooldown;

    public Material triangleMaterial;

    Transform playerCamera;
    LineRenderer lineRenderer;
    GameObject triangle;
    Mesh triangleMesh;

    int layerMask;
    int pointIndex;

    float lastTriangleTime;

    void Start(){
        playerCamera = Camera.main.transform;

        lineRenderer = GetComponent<LineRenderer>();

        triangle = new GameObject("Triangle");

        triangle.AddComponent<MeshFilter>();
        triangle.AddComponent<MeshRenderer>();

        triangleMesh = triangle.GetComponent<MeshFilter>().mesh;

        triangleMesh.Clear();

        triangle.GetComponent<MeshRenderer>().material = triangleMaterial;
        triangle.GetComponent<MeshRenderer>().shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
        triangle.GetComponent<MeshRenderer>().allowOcclusionWhenDynamic = false;

        triangleMesh.vertices = new Vector3[] {Vector3.zero, Vector3.zero, Vector3.zero};
        triangleMesh.triangles = new int[] {0, 1, 2};

        triangle.SetActive(false);
    }

    void Update(){
        RaycastHit hit;

        if (Physics.Raycast(playerCamera.position, playerCamera.forward, out hit, Mathf.Infinity) && pointIndex < 3){
            lineRenderer.SetPosition(pointIndex, hit.point);

            if (pointIndex == 2)
            {
                triangleMesh.SetVertices(new Vector3[] {lineRenderer.GetPosition(0), lineRenderer.GetPosition(1), lineRenderer.GetPosition(2)});

                triangleMesh.RecalculateBounds();
            }
        }

        if (Input.GetMouseButtonDown(0) && pointIndex < 3 && Time.time - lastTriangleTime > cooldown){
            lineRenderer.enabled = true;

            pointIndex++;

            if (pointIndex == 2){
                triangle.SetActive(true);
            }

            // Triangle done
            if (pointIndex == 3){
                triangleMesh.RecalculateBounds();
                lastTriangleTime = Time.time;
            }

            else {
                lineRenderer.positionCount++;
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
        triangle.SetActive(false);

        pointIndex = 0;
    }
}
