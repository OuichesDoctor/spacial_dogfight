using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ship : AbstractShip {

    public Vector2 moveDirection = Vector2.zero;
    public Vector2 lookDirection = Vector2.zero;

    public override void Fire(Vector2 target) {
        var currentTime = Time.realtimeSinceStartup;
        if (currentTime < _nextShot)
            return;

        var currentX = turretLook.x;
        var currentY = turretLook.y;
        var absX = Mathf.Abs(currentX);
        var absY = Mathf.Abs(currentY);

        GameObject[] cannons;
        if (absX > absY) {
            if(currentX > 0) {
                cannons = rightTurrets;
            }
            else {
                cannons = leftTurrets;
            }
        }
        else {
            if(currentY > 0) {
                cannons = frontTurrets;
            }
            else {
                cannons = backTurrets;
            }
        }
        
        foreach(var go in cannons) {
            var proj = GameObject.Instantiate(bullet, go.transform.position, go.transform.rotation);
            var projData = proj.GetComponent<Projectile>();
            projData.direction = go.transform.up;
        }

        _nextShot = currentTime + fireRate;
    }

    public override void FixedUpdate() {
        var currentPos = gameObject.transform.position;
        gameObject.transform.position = currentPos + (Vector3)(moveDirection * moveSpeed * Time.fixedDeltaTime);

        if (lookDirection != Vector2.zero) {
            var currentTransform = gameObject.transform;
            var zAngle = Vector2.Angle(lookDirection, currentTransform.up);
            var rightAngle = Vector2.Angle(lookDirection, currentTransform.right);
            var sign = -1;
            if (rightAngle > 90)
                sign = 1;

            if (Mathf.Abs(currentTransform.rotation.z - zAngle) > 2) {
                gameObject.transform.localRotation = Quaternion.Euler(0, 0, currentTransform.rotation.eulerAngles.z + rotationSpeed * Time.fixedDeltaTime * sign);
            }
        }

        var degreeDirection = Vector2.Angle(gameObject.transform.up, moveDirection);
        var correctionAngle = Vector2.Angle(gameObject.transform.right, moveDirection);
        var disSign = -1;
        if (correctionAngle > 90)
            disSign = 1;

        moveDisplay.gameObject.transform.rotation = Quaternion.Euler(0, 0, degreeDirection * disSign);
    }

    public override void SetLookDirection(Vector2 vector) {
        lookDirection = vector;
    }

    public override void SetMoveDirection(Vector2 vector) {
        moveDirection = vector;
    }

    public override void SetTurretLook(Vector2 vector) {
        turretLook = vector;
    }
}