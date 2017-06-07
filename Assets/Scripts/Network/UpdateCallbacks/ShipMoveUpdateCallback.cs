using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using spacial_dogfight_protocol.Messages;
using GameProtocol;
using Assets.Scripts.Ships.Seats;
using System;

public class ShipMoveUpdateCallback : NetworkCallback {

    public int shipID;
    public float moveSpeed;

    private Vector3 _lastDirection = Vector3.zero;
    private float _lastUpdate;
    private bool _isOrigin;
    private Rigidbody2D _rb2d;

    public new void Start() {
        base.Start();
        var mainSceneMgr = MainSceneManager.Instance;
        if (mainSceneMgr.mySeat.GetType() == typeof(PilotSeat) && mainSceneMgr.myShip == gameObject) {
            _isOrigin = true;
            return;
        }

        _rb2d = GetComponent<Rigidbody2D>();
    }

    public override void ProcessUpdate(Update update) {
        var mainSceneMgr = MainSceneManager.Instance;
        if (_isOrigin) {
            return;
        }

        if (mainSceneMgr.mySeat.GetType() == typeof(PilotSeat) && mainSceneMgr.myShip == gameObject) {
            _isOrigin = true;
            return;
        }

        var data = (ShipMoveMessage)update.Data;
        if (data.shipId != shipID)
            return;

        var currentPos = gameObject.transform.position;
        var newPos = new Vector3(data.x, data.y, currentPos.z);
        _lastDirection = (newPos - currentPos).normalized;
        if (_lastDirection.magnitude > 0.1f)
            gameObject.transform.position = newPos;

        //var interPolated = ComputeInterpolation(newPos);
        //gameObject.transform.position = interPolated;
        gameObject.transform.rotation = Quaternion.Euler(0, 0, data.rotation);

        _lastUpdate = Time.realtimeSinceStartup;
    }

    public override void Register() {
        NetworkManager.Instance.AddUpdateCallback(1, 7, this);
    }

    public void Update() {
        if(_lastDirection != Vector3.zero)
            Debug.DrawLine(gameObject.transform.position, gameObject.transform.position + _lastDirection * 5);
    }

    public void FixedUpdate() {
        var mainSceneMgr = MainSceneManager.Instance;
        if (_isOrigin) {
            return;
        }

        if (mainSceneMgr.mySeat.GetType() == typeof(PilotSeat) && mainSceneMgr.myShip == gameObject) {
            _isOrigin = true;
            return;
        }

        if (_lastDirection.magnitude < 0.1f)
            return;

        Debug.Log("FixedUpdate Ship " + shipID);

        var currentPos = gameObject.transform.position;
        var newPos = currentPos + _lastDirection * moveSpeed * Time.fixedDeltaTime;
        gameObject.transform.position = newPos;
    }

    private Vector3 ComputeInterpolation(Vector3 real) {
        var currentTime = Time.realtimeSinceStartup;
        var deltaTime = currentTime - _lastUpdate;
        var currentPos = gameObject.transform.position;
        var predicted = currentPos + _lastDirection * moveSpeed * deltaTime;
        return (predicted + real) / 2;
    }

    //public void Update() {
    //    var currentPos = gameObject.transform.position;
    //    Debug.DrawLine(currentPos, currentPos + _lastDirection * 5);
    //}

    //public void FixedUpdate() {
    //    if (_lastDirection == Vector3.zero)
    //        return;

    //    var current = Time.realtimeSinceStartup;
    //    var elapsed = current - _lastUpdate;
    //    var currentPos = gameObject.transform.position;
    //    gameObject.transform.position = currentPos + _lastDirection * moveSpeed * elapsed;
    //}
}
