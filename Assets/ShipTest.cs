using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipTest : MonoBehaviour {

    public float moveSpeed = 5f;
    public float inputRate = 1f;
    private Vector2 _lastDirection;
    private float _nextUpdate;

	void Update () {
        var currentTime = Time.realtimeSinceStartup;
        if(currentTime > _nextUpdate) {
            var xAxis = Input.GetAxis("Horizontal");
            var yAxis = Input.GetAxis("Vertical");

            _lastDirection = new Vector2(xAxis, yAxis).normalized;
            _nextUpdate = currentTime + inputRate;
        }
    }

    void FixedUpdate() {
        var currentPos = gameObject.transform.position;
        gameObject.transform.position = currentPos + (Vector3)(_lastDirection * moveSpeed);
        _lastDirection = Vector2.zero;
    }
}
