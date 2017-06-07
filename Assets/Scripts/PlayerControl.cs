using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour {

    public float moveSpeed;

    private Vector2 _directionMove;

    private Rigidbody2D _rb2D;

	void Start () {
        _rb2D = GetComponent<Rigidbody2D>();
    }
	
	void Update () {
        var xAcceleration = Input.GetAxis("Horizontal");
        var yAcceleration = Input.GetAxis("Vertical");

        var currentTransform = gameObject.transform;
        _directionMove = xAcceleration * currentTransform.up - yAcceleration * currentTransform.right;
    }

    void FixedUpdate() {
        if (_directionMove == Vector2.zero)
            return;

        _rb2D.position = (Vector2)gameObject.transform.position + Time.fixedDeltaTime * _directionMove.normalized * moveSpeed;
    }

    void OnDisable() {
        _rb2D.simulated = false;
    }

    void OnEnable() {
        if (_rb2D != null)
            _rb2D.simulated = true;
    }
}
