using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public abstract class AbstractPlayer : NetworkBehaviour {

    public Camera playerCamera;
    public float moveSpeed = 4f;

        protected Seat _nearSeat;
    //protected PhysicObject _po;
    protected Rigidbody2D _rb2D;

    public void Start() {
        //_po = GetComponent<PhysicObject>();
        _rb2D = GetComponent<Rigidbody2D>();
    }

    public void OnPOCollisionEnter(object[] result) {
        if (!isServer)
            return;

        var other = (Collider2D)result[1];
        if (other.gameObject.layer == LayerMask.NameToLayer("rooms")) {
        }
        else if(other.gameObject.layer == LayerMask.NameToLayer("seat") && _nearSeat == null) {
            _nearSeat = other.gameObject.GetComponent<Seat>();
        }
    }

    public void OnPOCollisionLeave(object[] result) {
        if (!isServer)
            return;

        if (_nearSeat == null)
            return;

        var other = (Collider2D)result[0];
        if (other.gameObject == _nearSeat.gameObject) {
            _nearSeat = null;
        }
    }

    public void OnTriggerEnter2D(Collider2D collision) {
        if (!isServer)
            return;

        var go = collision.gameObject;
        if (go.layer == LayerMask.NameToLayer("seat") && _nearSeat == null) {
            _nearSeat = go.GetComponent<Seat>();
        }
    }

    public void OnTriggerExit2D(Collider2D collision) {
        if (!isServer)
            return;

        if (_nearSeat == null)
            return;

        var go = collision.gameObject;
        if (go == _nearSeat.gameObject) {
            _nearSeat = null;
        }
    }

    public void OnCollisionEnter2D(Collision2D collision) {
        var hit = collision.contacts[0];
        Debug.DrawLine(hit.point, hit.point + hit.normal, Color.green);
        //var displacement = moveSpeed * Time.deltaTime * hit.normal;
        //var previousPos = gameObject.transform.position + (Vector3)displacement;
        //gameObject.transform.position = hit.point + hit.normal * 0.01f;
        gameObject.transform.position = hit.point + hit.normal * 0.05f;
    }
}
