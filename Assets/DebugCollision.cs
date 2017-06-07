using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugCollision : MonoBehaviour {


    public void OnTriggerEnter2D(Collider2D collision) {
        Debug.Log("Trigger 2D");
    }

    public void OnCollisionEnter2D(Collision2D collision) {
        Debug.Log("Collision 2D");
    }

    public void OnCollisionEnter(Collision collision) {
        Debug.Log("Collision");
    }
}
