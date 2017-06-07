using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using spacial_dogfight_protocol.Commands;

public class StartGameButton : MonoBehaviour {

    private Button _button;

	void Start () {
        _button = GetComponent<Button>();
	}
	
	void Update () {
        _button.interactable = false;
        var netMgr = NetworkManager.Instance;
        var slots = netMgr.playerSlots;
        var nbPlayer = 0;
        foreach(var s in slots) {
            if (s.Value != 0)
                nbPlayer++;
        }

        if (nbPlayer == 4) {
            _button.interactable = true;
        }
	}

    public void LaunchGame() {
        var cmd = new StartCommand();
        NetworkManager.Instance.SendCommand(cmd);
    }
}
