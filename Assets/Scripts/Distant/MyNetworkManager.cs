using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class MyNetworkManager : NetworkLobbyManager {

    public const byte SLOT1 = 0x00;
    public const byte SLOT2 = 0x01;
    public const byte SLOT3 = 0x02;
    public const byte SLOT4 = 0x03;

    public GameObject shipPrefab;

    public override bool OnLobbyServerSceneLoadedForPlayer(GameObject lobbyPlayer, GameObject gamePlayer) {

        var player = gamePlayer.GetComponent<NetPlayer>();
        var lobbyData = lobbyPlayer.GetComponent<NetworkLobbyPlayer>();
        //player.joypadId = lobbyData.joypadID;

        switch(lobbyData.slot) {
            case SLOT1:
                player.shipId = 0;
                break;
            case SLOT2:
                player.shipId = 0;
                break;
            case SLOT3:
                player.shipId = 1;
                break;
            case SLOT4:
                player.shipId = 1;
                break;
            default:
                Debug.Log(lobbyData.slot);
                break;
        }

        return true;
    }

}