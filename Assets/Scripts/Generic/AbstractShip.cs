using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public abstract class AbstractShip : NetworkBehaviour {

    public Camera shipCamera;
    public GameObject[] frontTurrets;
    public GameObject[] leftTurrets;
    public GameObject[] rightTurrets;
    public GameObject[] backTurrets;
    public Seat[] seats;
    public GameObject bullet;
    public GameObject moveDisplay;
    public float moveSpeed = 50f;
    public float rotationSpeed = 120f;
    public float turretRotationSpeed = 180f;
    public float fireRate = .5f;

    protected float _nextShot = 0f;

    public abstract void SetMoveDirection(Vector2 vector);
    public abstract void SetLookDirection(Vector2 vector);
    public abstract void SetTurretLook(Vector2 vector);
    public abstract void FixedUpdate();
    public abstract void Fire();
}
