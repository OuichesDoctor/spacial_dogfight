using System;
using System.Collections;
using System.Collections.Generic;
using GameProtocol;
using UnityEngine;
using spacial_dogfight_protocol.Messages;

public class PositionUpdateCallback : NetworkCallback {

    public override void ProcessUpdate(Update update) {
        var data = (PositionMessage)update.Data;

        if (data.playerID != NetworkManager.Instance.playerID)
            return;

        if (data.gid != gameObject.GetInstanceID())
            return;

        var currentPos = gameObject.transform.position;
        var newPos = new Vector3(data.x, data.y, currentPos.z);
        gameObject.transform.position = newPos;
        gameObject.transform.rotation = Quaternion.Euler(0, 0, data.rotation);
    }

    public override void Register() {
        NetworkManager.Instance.AddUpdateCallback(1, 1, this);
    }
}
