using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cannon : MonoBehaviour {

    public float rotationAngle = 120f;

    private Vector2 _baseUp;
    private float _halfAngle;

	void Start () {
        _halfAngle = rotationAngle / 2;
        _baseUp = gameObject.transform.up;
	}
	
    public void SetSightDirection(Vector2 sightDirection) {
        if (Vector2.Angle(sightDirection, _baseUp) < _halfAngle) 
            gameObject.transform.up = sightDirection;
    }

	void LateUpdate () {
        if (Vector2.Angle(gameObject.transform.up, _baseUp) > _halfAngle) {
            gameObject.transform.up = _baseUp;
        }
    }
}
