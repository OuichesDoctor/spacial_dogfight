using GameProtocol;
using spacial_dogfight_protocol.Messages;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class PlayerIDUpdateCallback : NetworkCallback {

    public override void ProcessUpdate(Update update) {
        var data = (PlayerIDMessage)update.Data;

        NetworkManager.Instance.playerID = data.playerID;
        SceneManager.LoadScene("Join");
    }

    public override void Register() {
        NetworkManager.Instance.AddUpdateCallback(1, 2, this);
    }
}
