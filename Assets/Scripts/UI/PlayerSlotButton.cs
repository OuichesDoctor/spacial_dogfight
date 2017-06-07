using spacial_dogfight_protocol.Commands;
using spacial_dogfight_protocol.Messages;
using UnityEngine;
using UnityEngine.UI;

public class PlayerSlotButton : MonoBehaviour {

    public int playerSlot = 1;

    private bool _available = true;
    private Button _button;
    private Text _text;

	void Start () {
        _button = GetComponent<Button>();
        _text = GetComponentInChildren<Text>();
	}
	
	void Update () {
        var netMgr = NetworkManager.Instance;
        if (netMgr.playerSlots.ContainsKey(playerSlot) && netMgr.playerSlots[playerSlot] != 0) {
            _available = false;
            if(netMgr.playerSlots[playerSlot] == netMgr.playerID) {
                _text.text = "RELEASE SLOT";
            }
            else {
                _button.interactable = false;
                _text.text = "SLOT TAKEN";
            }
        }
        else if (!_available) {
            _text.text = "CLICK TO JOIN";
            _available = true;
            _button.interactable = true;
        }
    }

    public void JoinGame() {
        var netMgr = NetworkManager.Instance;
        if(_available && !netMgr.playerSlots.ContainsValue(netMgr.playerID)) {
            var cmd = new JoinCommand();
            ((JoinMessage)cmd.Data).playerSlots[playerSlot] = netMgr.playerID;
            netMgr.SendCommand(cmd);
        }
        else if (netMgr.playerSlots.ContainsKey(playerSlot) && netMgr.playerSlots[playerSlot] == netMgr.playerID) {
            var cmd = new LeaveSlotCommand();
            ((LeaveSlotMessage)cmd.Data).playerID = netMgr.playerID;
            ((LeaveSlotMessage)cmd.Data).playerSlot = playerSlot;
            netMgr.SendCommand(cmd);
        }
    }

}
