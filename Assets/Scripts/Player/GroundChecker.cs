using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundChecker : MonoBehaviour
{
    PlayerMovement playerMovement;

    private void Start() {
        playerMovement = transform.GetComponentInParent<PlayerMovement>();
    }

    void OnTriggerStay(Collider coll) {
        if (coll.gameObject.CompareTag("Wall") || coll.gameObject.CompareTag("Ground"))
        {
            playerMovement.lastGroundContact = Time.time;
        }
    }

    void OnTriggerEnter(Collider coll){
        if (coll.gameObject.CompareTag("Wall") || coll.gameObject.CompareTag("Ground"))
        {
            playerMovement.PlayLandSound();
        }
    }
}
