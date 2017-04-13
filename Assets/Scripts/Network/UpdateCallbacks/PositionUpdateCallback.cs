using System;
using System.Collections;
using System.Collections.Generic;
using GameProtocol;
using UnityEngine;
using spacial_dogfight_protocol.Messages;

public class PositionUpdateCallback : NetworkCallback {
    public override void ProcessUpdate(Update update) {
        var data = (PositionMessage)update.Data;
        var currentPos = gameObject.transform.position;
        var newPos = new Vector3(data.x, data.y, currentPos.z);
        gameObject.transform.position = newPos;
    }

    public override void Register() {
        NetworkManager.Instance.AddUpdateCallback(0, 1, this);
    }
}
