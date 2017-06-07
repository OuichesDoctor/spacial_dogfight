using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class OldShip : NetworkBehaviour {

    [SyncVar]
    public Vector2 moveDirection;
    [SyncVar]
    public Vector2 lookDirection;
    [SyncVar]
    public Vector2 turretLook;

    public GameObject turret;
    public GameObject bullet;
    public GameObject moveDisplay;
    public float moveSpeed = 5f;
    public float rotationSpeed = 180f;
    public float turretRotationSpeed = 180f;
    private Rigidbody2D _rb2D;

    public void Start() {
        _rb2D = GetComponent<Rigidbody2D>();
    }

    public void FixedUpdate() {
        var currentPos = _rb2D.transform.position;
        _rb2D.transform.position =  currentPos + (Vector3)(moveDirection * moveSpeed * Time.fixedDeltaTime);

        if (lookDirection != Vector2.zero) {
            var currentTransform = gameObject.transform;
            var zAngle = Vector2.Angle(lookDirection, currentTransform.up);
            var rightAngle = Vector2.Angle(lookDirection, currentTransform.right);
            var sign = -1;
            if (rightAngle > 90)
                sign = 1;

            if (Mathf.Abs(currentTransform.rotation.z - zAngle) > 2) {
                _rb2D.rotation = currentTransform.rotation.eulerAngles.z + rotationSpeed * Time.fixedDeltaTime * sign;
            }
        }

        var degreeDirection = Vector2.Angle(gameObject.transform.up, moveDirection);
        var correctionAngle = Vector2.Angle(gameObject.transform.right, moveDirection);
        var disSign = -1;
        if (correctionAngle > 90)
            disSign = 1;

        moveDisplay.gameObject.transform.rotation = Quaternion.Euler(0, 0, degreeDirection * disSign);

        if (turretLook != Vector2.zero) {
            var currentTransform = turret.transform;
            var zAngle = Vector2.Angle(turretLook, currentTransform.up);
            var rightAngle = Vector2.Angle(turretLook, currentTransform.right);
            var sign = -1;
            if (rightAngle > 90)
                sign = 1;

            if (Mathf.Abs(currentTransform.rotation.z - zAngle) > 2) {
                turret.transform.rotation = Quaternion.Euler(0, 0, currentTransform.rotation.eulerAngles.z + rotationSpeed * Time.fixedDeltaTime * sign);
            }
        }
    }

    public void Fire() {
        var proj = GameObject.Instantiate(bullet, turret.transform.position, turret.transform.rotation);
        var projData = GetComponent<Projectile>();
        projData.direction = turretLook;
        NetworkServer.Spawn(proj);
    }
}