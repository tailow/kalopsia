using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriangleCollision : MonoBehaviour
{
    List<Collider> colliders = new List<Collider>();

    public void DealDamage(){
        for(int i = 0; i < colliders.Count; i++)
        {
            if (colliders[i].gameObject.tag == "Enemy")
            {
                colliders[i].gameObject.GetComponent<Health>().TakeDamage(50);
                colliders.Remove(colliders[i]);
            }
        }
    }

    void OnTriggerEnter(Collider other) {
        colliders.Add(other);
    }

    void OnTriggerExit(Collider other) {
        colliders.Remove(other);
    }
}
