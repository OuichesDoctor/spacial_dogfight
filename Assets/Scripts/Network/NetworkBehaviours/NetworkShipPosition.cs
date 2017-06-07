using spacial_dogfight_protocol.Commands;
using spacial_dogfight_protocol.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class NetworkShipPosition : MonoBehaviour {

    public float networkUpdateRate = 0.1f;

    private int shipID;
    private float _nextPosUpdate = 0;

    void Start() {
        var cb = gameObject.GetComponent<ShipMoveUpdateCallback>();
        shipID = cb.shipID;
    }

    void Update() {
        var currentTime = Time.realtimeSinceStartup;
        if (currentTime > _nextPosUpdate) {
            var cmd = new ShipMoveCommand();
            var transform = gameObject.transform;
            var pos = transform.position;
            ((ShipMoveMessage)cmd.Data).shipId = shipID;
            ((ShipMoveMessage)cmd.Data).x = pos.x;
            ((ShipMoveMessage)cmd.Data).y = pos.y;
            ((ShipMoveMessage)cmd.Data).rotation = transform.rotation.eulerAngles.z;

            NetworkManager.Instance.SendCommand(cmd);
            _nextPosUpdate = currentTime + networkUpdateRate;
        }
    }
}
