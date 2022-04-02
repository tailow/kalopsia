using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriangleCollision : MonoBehaviour
{
    List<Collider> colliders = new List<Collider>();

    public void DealDamage(){
        foreach (Collider collider in colliders)
        {
            Debug.Log(collider.gameObject.name);
        }
    }

    void OnTriggerEnter(Collider other) {
        colliders.Add(other);
    }

    void OnTriggerExit(Collider other) {
        colliders.Remove(other);
    }
}
