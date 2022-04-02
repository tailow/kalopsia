using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriangleCaster : MonoBehaviour
{
    public float cooldown;

    public Material triangleMaterial;

    TriangleCollision triangleCollision;

    Transform playerCamera;
    LineRenderer lineRenderer;
    GameObject triangle;
    Mesh triangleMesh;
    Vector3[] positions;

    int layerMask;
    int pointIndex;

    float lastTriangleTime;

    void Start(){
        playerCamera = Camera.main.transform;

        positions = new Vector3[3];

        lineRenderer = GetComponent<LineRenderer>();

        triangle = new GameObject("Triangle");

        triangle.AddComponent<MeshFilter>();
        triangle.AddComponent<MeshRenderer>();
        triangle.AddComponent<MeshCollider>();
        triangle.AddComponent<TriangleCollision>();
        triangle.AddComponent<Rigidbody>();

        triangleMesh = triangle.GetComponent<MeshFilter>().mesh;

        triangleMesh.Clear();

        triangleMesh.vertices = new Vector3[] {Vector3.zero, Vector3.zero, Vector3.zero};
        triangleMesh.triangles = new int[] {0, 1, 2};

        triangle.GetComponent<MeshRenderer>().material = triangleMaterial;
        triangle.GetComponent<MeshRenderer>().shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
        triangle.GetComponent<MeshRenderer>().allowOcclusionWhenDynamic = false;
        triangle.GetComponent<MeshRenderer>().receiveShadows = false;

        triangle.GetComponent<Rigidbody>().isKinematic = true;

        triangle.SetActive(false);
    }

    void Update(){
        RaycastHit hit;

        if (Physics.Raycast(playerCamera.position, playerCamera.forward, out hit, Mathf.Infinity) && pointIndex < 3){
            lineRenderer.SetPosition(pointIndex, hit.point);

            if (pointIndex == 2)
            {
                lineRenderer.GetPositions(positions);

                triangleMesh.SetVertices(positions);

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

                Mesh colliderMesh = triangleMesh;

                colliderMesh.vertices = new Vector3[] {positions[0], positions[1], positions[2], positions[2] + Vector3.up * 0.001f};

                triangle.GetComponent<MeshCollider>().sharedMesh = colliderMesh;
                triangle.GetComponent<MeshCollider>().convex = true;
                triangle.GetComponent<MeshCollider>().isTrigger = true;

                lastTriangleTime = Time.time;

                Invoke("DestroyTriangle", 1f);
            }

            else {
                lineRenderer.positionCount++;
            }
        }

        if (Input.GetMouseButtonDown(1)){
            ResetTriangle();
        }
    }

    void DestroyTriangle(){
        triangle.SendMessage("DealDamage");

        ResetTriangle();
    }

    void ResetTriangle(){
        lineRenderer.positionCount = 1;

        lineRenderer.SetPosition(0, Vector3.zero);

        lineRenderer.enabled = false;
        triangle.SetActive(false);

        pointIndex = 0;
    }
}
