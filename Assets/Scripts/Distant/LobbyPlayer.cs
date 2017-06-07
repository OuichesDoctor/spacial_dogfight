using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class LobbyPlayer : NetworkBehaviour {

    private NetworkLobbyPlayer _lPlayer;

    void Start() {
        if (!isServer)
            return;

        _lPlayer = GetComponent<NetworkLobbyPlayer>();
    }

    void Update() {
        if (!isLocalPlayer)
            return;

        if (Input.GetButtonDown("Fire1")) {
            _lPlayer.SendReadyToBeginMessage();
        }
    }

}
