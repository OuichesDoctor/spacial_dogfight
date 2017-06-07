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
            var transform = gameObject.transform;
            var pos = transform.position;
            ((PositionMessage)cmd.Data).playerID = NetworkManager.Instance.playerID;
            ((PositionMessage)cmd.Data).gid = gameObject.GetInstanceID();
            ((PositionMessage)cmd.Data).x = pos.x;
            ((PositionMessage)cmd.Data).y = pos.y;
            ((PositionMessage)cmd.Data).rotation = transform.rotation.eulerAngles.z;

            NetworkManager.Instance.SendCommand(cmd);
            _nextPosUpdate = currentTime + networkUpdateRate;
        }
	}
}
