using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

[NetworkSettings(sendInterval = 0.01f)]
public class NetShip : AbstractShip {

    public static List<NetShip> ships;

    [SyncVar]
    public Vector2 moveDirection = Vector2.zero;

    [SyncVar]
    public Vector2 lookDirection = Vector2.zero;

    [SyncVar]
    public Vector2 serverPosition;

    [SyncVar]
    public int shipId = -1;

    [SyncVar]
    public float health = 100;

    public Vector2 turretLook = Vector2.zero;
    public List<GameObject> startPos = new List<GameObject>();
    public ParticleSystem shipBoom;

    //protected PhysicObject _po;
    protected Rigidbody2D _rb2D;

    public void Awake() {
        serverPosition = gameObject.transform.localPosition;

        if (NetShip.ships == null)
            NetShip.ships = new List<NetShip>();

        NetShip.ships.Insert(shipId, this);
        //_po = GetComponent<PhysicObject>();
        _rb2D = GetComponent<Rigidbody2D>();
    }

    public override void Fire() {
        if (!isServer)
            return;

        var currentTime = Time.realtimeSinceStartup;
        if (currentTime < _nextShot)
            return;

        var currentX = turretLook.x;
        var currentY = turretLook.y;
        var absX = Mathf.Abs(currentX);
        var absY = Mathf.Abs(currentY);

        GameObject[] cannons;
        if (absX > absY) {
            if (currentX > 0) {
                cannons = rightTurrets;
            }
            else {
                cannons = leftTurrets;
            }
        }
        else {
            if (currentY > 0) {
                cannons = frontTurrets;
            }
            else {
                cannons = backTurrets;
            }
        }

        foreach (var go in cannons) {
            var proj = GameObject.Instantiate(bullet, go.transform.position + go.transform.up * 1, go.transform.rotation);
            proj.GetComponent<Projectile>().myShip = gameObject;
            NetworkServer.Spawn(proj);
        }

        _nextShot = currentTime + fireRate;
    }

    public override void FixedUpdate() {
        if (isServer) {
            var currentPos = gameObject.transform.localPosition;
            gameObject.transform.localPosition = currentPos + (Vector3)moveDirection * moveSpeed * Time.fixedDeltaTime;
            //po.SetDirection(moveDirection);
            serverPosition = gameObject.transform.localPosition;
        }
        else {
            gameObject.transform.localPosition = serverPosition;
        }

        if (lookDirection != Vector2.zero) {
            var currentTransform = gameObject.transform;
            var zAngle = Vector2.Angle(lookDirection, currentTransform.up);
            var rightAngle = Vector2.Angle(lookDirection, currentTransform.right);
            var sign = -1;
            if (rightAngle > 90)
                sign = 1;

            if (Mathf.Abs(currentTransform.localRotation.z - zAngle) > 2) {
                gameObject.transform.localRotation = Quaternion.Euler(0, 0, currentTransform.localRotation.eulerAngles.z + rotationSpeed * Time.fixedDeltaTime * sign);
            }
        }

        var degreeDirection = Vector2.Angle(gameObject.transform.up, moveDirection);
        var correctionAngle = Vector2.Angle(gameObject.transform.right, moveDirection);
        var disSign = -1;
        if (correctionAngle > 90)
            disSign = 1;

        moveDisplay.gameObject.transform.localRotation = Quaternion.Euler(0, 0, degreeDirection * disSign);
    }

    public void OnPOCollisionEnter(object[] result) {
        if (!isServer)
            return;

        var other = (Collider2D)result[1];
        if (other.gameObject.layer == LayerMask.NameToLayer("projectiles")) {
            if (health <= 0)
                return;

            health -= 1;
        }
    }

    public void Update() {
        if (health <= 0) {
            shipBoom.gameObject.SetActive(true);
        }
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

    public GameObject GetSpawnPoint() {
        GameObject result = null;
        foreach (var go in startPos) {
            result = go;
            break;
        }

        startPos.Remove(result);

        return result;
    }
}
