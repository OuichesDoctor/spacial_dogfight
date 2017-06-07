using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class LocalPlayer : AbstractPlayer {

    public Vector2 moveDirection = Vector2.zero;
    public Vector2 lookDirection = Vector2.zero;

    public Ship ship;
    public bool usingJoypad;
    public float rotationSpeed = 180f;

    public GameObject target;

    private Seat _mySeat;
    private Rigidbody2D _rb2D;

    public void Start() {
        _rb2D = GetComponent<Rigidbody2D>();
        _mySeat = Seat.None;
    }

    public void Update() {
        var xAxis = Input.GetAxis("Horizontal");
        var yAxis = Input.GetAxis("Vertical");
        var directionMove = new Vector2(xAxis, yAxis).normalized;

        if(_mySeat != Seat.None)
            _mySeat.ProcessDirection(directionMove);
        else {
            moveDirection = directionMove;
        }

        Vector2 directionLook;
        if(usingJoypad) {
            var xLookAt = Input.GetAxis("HorizontalZ");
            var yLookAt = Input.GetAxis("VerticalZ");

            directionLook = new Vector2(xLookAt, yLookAt);
            if (directionLook.magnitude < 0.5)
                directionLook = Vector2.zero;
        }
        else {

            var mousePos = Input.mousePosition;
            mousePos = Camera.main.ScreenToWorldPoint(mousePos);

            directionLook = (Vector2)mousePos - (Vector2)gameObject.transform.position;
            if (directionLook.magnitude < 0.5)
                directionLook = Vector2.zero;
        }

        if(_mySeat != Seat.None)
            _mySeat.ProcessLook(directionLook);
        else {
            lookDirection = directionLook;
        }

        if (Input.GetButtonDown("Fire1")) {
            if (_mySeat != Seat.None)
                _mySeat.ProcessFire();
        }

        if (Input.GetButtonDown("Fire2")) {
            if (_mySeat != Seat.None && Time.realtimeSinceStartup > _mySeat.nextAvailable) {
                _mySeat.ship.shipCamera.enabled = false;
                playerCamera.enabled = true;
                _mySeat.nextAvailable = Time.realtimeSinceStartup + 1;
                _mySeat.available = true;
                _mySeat.ProcessDirection(Vector2.zero);
                _mySeat.ProcessLook(Vector2.zero);
                _mySeat = Seat.None;
                gameObject.transform.parent = null;
                _rb2D.bodyType = RigidbodyType2D.Dynamic;
            }
            else if (_nearSeat != null && _nearSeat.available && Time.realtimeSinceStartup > _nearSeat.nextAvailable ) {
                _nearSeat.available = false;
                gameObject.transform.position = _nearSeat.gameObject.transform.position;
                this.playerCamera.enabled = false;
                _nearSeat.ship.shipCamera.enabled = true;
                _nearSeat.nextAvailable = Time.realtimeSinceStartup + 1;
                this.SetSeat(_nearSeat.index);
            }
        }
    }

    public void FixedUpdate() {
        if (_mySeat != Seat.None)
            return;

        var currentPos = _rb2D.transform.position;
        var currentRotation = ship.gameObject.transform.rotation;
        gameObject.transform.rotation = currentRotation;
        moveDirection = Quaternion.Euler(0, 0, currentRotation.eulerAngles.z + 90) * moveDirection;

        _rb2D.transform.position = currentPos + (Vector3)(moveDirection * moveSpeed * Time.fixedDeltaTime);

        if (lookDirection.magnitude >= .5) {
            var currentTransform = target.gameObject.transform;
            lookDirection = Quaternion.Euler(0, 0, currentRotation.eulerAngles.z + 90) * lookDirection;
            currentTransform.position = gameObject.transform.position + (Vector3)lookDirection.normalized;
        }

    }

    public void SetSeat(int newSeatIndex) {
        _rb2D.bodyType = RigidbodyType2D.Kinematic;
        var seat = ship.seats[newSeatIndex];
        _mySeat = seat;
        gameObject.transform.parent = _mySeat.gameObject.transform;
    }

}