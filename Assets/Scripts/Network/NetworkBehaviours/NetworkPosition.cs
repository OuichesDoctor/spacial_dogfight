using spacial_dogfight_protocol.Commands;
using spacial_dogfight_protocol.Messages;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkPosition : MonoBehaviour {

    public float networkUpdateRate = 0.5f;

    private float _nextPosUpdate = 0;

	void Update () {
        var currentTime = Time.realtimeSinceStartup;
        if(currentTime > _nextPosUpdate) {
            var cmd = new PositionCommand();
            var pos = gameObject.transform.position;
            ((PositionMessage)cmd.Data).x = pos.x;
            ((PositionMessage)cmd.Data).y = pos.y;

            NetworkManager.Instance.SendCommand(cmd);
            _nextPosUpdate = currentTime + networkUpdateRate;
        }
	}
}
