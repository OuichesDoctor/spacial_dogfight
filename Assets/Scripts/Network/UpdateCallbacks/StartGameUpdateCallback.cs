using GameProtocol;
using spacial_dogfight_protocol.Messages;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartGameUpdateCallback : NetworkCallback {

    public override void ProcessUpdate(Update update) {
        SceneManager.LoadScene("Main");
    }

    public override void Register() {
        NetworkManager.Instance.AddUpdateCallback(1, 6, this);
    }
}
