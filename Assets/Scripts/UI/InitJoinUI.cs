using spacial_dogfight_protocol.Commands;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitJoinUI : MonoBehaviour {

	void Start () {
        var cmd = new PingCommand();
        NetworkManager.Instance.SendCommand(cmd);
    }
}
