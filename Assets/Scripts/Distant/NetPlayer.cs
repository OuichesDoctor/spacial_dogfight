using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

[NetworkSettings(sendInterval = 0.01f)]
public class NetPlayer : AbstractPlayer {

    public static NetPlayer local;

    [SyncVar]
    public Vector2 moveDirection = Vector2.zero;

    [SyncVar]
    public Vector2 lookDirection = Vector2.zero;

    [SyncVar]
    public Vector2 serverPosition = Vector2.zero;

    [SyncVar]
    public int shipId = -1;

    public NetShip ship;
    public bool usingJoypad;
    public float rotationSpeed = 180f;

    public GameObject target;

    private Seat _mySeat;

    public new void Start() {
        _mySeat = Seat.None;
        if (isLocalPlayer) {
            playerCamera.enabled = true;
            local = this;
        }
        base.Start();
    }

    public void Update() {
        if (ship == null && shipId != -1 && NetShip.ships != null && NetShip.ships.Count == 2) {
            ship = NetShip.ships[shipId];
            gameObject.transform.parent = ship.transform;
            var shipCam = GameObject.FindGameObjectWithTag("ShipCamera");
            ship.shipCamera = shipCam.GetComponent<Camera>();
            ship.shipCamera.enabled = false;
            shipCam.GetComponent<FollowCam>().target = ship.gameObject;
            var spawn = ship.GetSpawnPoint();
            gameObject.transform.position = spawn.transform.position;
            serverPosition = gameObject.transform.localPosition;
            gameObject.transform.rotation = Quaternion.Euler(0, 0, 0);
        }   

        if (isLocalPlayer) {
            var xAxis = Input.GetAxis("Horizontal");
            if (Mathf.Abs(xAxis) < 0.3f)
                xAxis = 0;
            var yAxis = Input.GetAxis("Vertical");
            if (Mathf.Abs(yAxis) < 0.3f)
                yAxis = 0;

            var directionMove = new Vector2(xAxis, yAxis).normalized;

            Vector2 directionLook = Vector2.zero;
            if (usingJoypad) {
                var xLookAt = Input.GetAxis("HorizontalZ");
                var yLookAt = Input.GetAxis("VerticalZ");

                directionLook = new Vector2(xLookAt, yLookAt);
                if (directionLook.magnitude < 0.5)
                    directionLook = Vector2.zero;
            }
            else {
                var mousePos = Input.mousePosition;
                if(Camera.main != null) {
                    mousePos = Camera.main.ScreenToWorldPoint(mousePos);

                    directionLook = (Vector2)mousePos - (Vector2)gameObject.transform.position;
                    if (directionLook.magnitude < 0.5)
                        directionLook = Vector2.zero;
                }
            }

            CmdSendDirection(directionMove, directionLook);

            if (Input.GetButtonDown("Fire1")) {
                CmdFire1();
            }

            if (Input.GetButtonDown("Fire2")) {
                CmdFire2();
            }
        }
    }

    public void FixedUpdate() {
        if (ship == null || _mySeat != Seat.None)
            return;

        var currentRotation = ship.gameObject.transform.localRotation;
        gameObject.transform.localRotation = Quaternion.Euler(0, 0, 0);
            if (isServer) {
                var rotatedMoveDirection = Quaternion.Euler(0, 0, currentRotation.eulerAngles.z + 90) * moveDirection;
                //_po.SetDirection(rotatedMoveDirection);
                _rb2D.MovePosition(gameObject.transform.position + (Vector3)(rotatedMoveDirection * moveSpeed * Time.fixedDeltaTime));
                serverPosition = gameObject.transform.localPosition;
            }
            else {
                gameObject.transform.localPosition = serverPosition;
            }

        if (lookDirection.magnitude >= .5) {
            var currentTransform = target.gameObject.transform;
            var rotatedLookDirection = Quaternion.Euler(0, 0, gameObject.transform.rotation.eulerAngles.z - 90) * lookDirection;
            currentTransform.position = gameObject.transform.position + (Vector3)rotatedLookDirection.normalized;
        }

    }

    [ClientRpc]
    public void RpcSetSeat(int newSeatIndex, int oldSeatIndex) {
        if (!isServer)
            SetSeat(newSeatIndex, oldSeatIndex);
    }

    public void SetSeat(int newSeatIndex, int oldSeatIndex = -1) {

        if(newSeatIndex == -1) {
            Seat currentSeat;
            if (isLocalPlayer) {
                _mySeat.ship.shipCamera.enabled = false;
                playerCamera.enabled = true;
                currentSeat = _mySeat;
            }
            else {
                currentSeat = ship.seats[oldSeatIndex];
            }
            _mySeat = Seat.None;
            currentSeat.nextAvailable = Time.realtimeSinceStartup + 1;
            currentSeat.available = true;
            currentSeat.ProcessDirection(Vector2.zero);
            currentSeat.ProcessLook(Vector2.zero);
            gameObject.transform.parent = ship.transform;
        }
        else {
            var newSeat = ship.seats[newSeatIndex];
            _mySeat = newSeat;
            newSeat.available = false;
            gameObject.transform.position = newSeat.gameObject.transform.position;
            gameObject.transform.parent = newSeat.gameObject.transform;
            newSeat.nextAvailable = Time.realtimeSinceStartup + 1;
            if (isLocalPlayer) {
                playerCamera.enabled = false;
                newSeat.ship.shipCamera.enabled = true;
            }
            else if (isServer) {
                serverPosition = gameObject.transform.position;
            }
        }

        if(isServer) {
            RpcSetSeat(newSeatIndex, oldSeatIndex);
        }
    }

    [Command]
    void CmdSendDirection(Vector2 moveDirection, Vector2 lookDirection) {
        if (_mySeat != Seat.None) {
            _mySeat.ProcessDirection(moveDirection);
            _mySeat.ProcessLook(lookDirection);
        }
        else {
            this.moveDirection = moveDirection;
            this.lookDirection = lookDirection;
        }
    }

    [Command]
    void CmdFire1() {
        if(_mySeat != Seat.None)
            _mySeat.ProcessFire();
    }

    [Command]
    void CmdFire2() {
        if (_mySeat != Seat.None && Time.realtimeSinceStartup > _mySeat.nextAvailable) {
            SetSeat(-1, _mySeat.index);
        }
        else if (_nearSeat != null && _nearSeat.available && Time.realtimeSinceStartup > _nearSeat.nextAvailable) {
            SetSeat(_nearSeat.index);
        }
    }
}
