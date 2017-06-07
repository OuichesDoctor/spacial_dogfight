using GameProtocol;
using spacial_dogfight_protocol.Messages;

public class JoinUpdateCallback : NetworkCallback {
    public override void ProcessUpdate(Update update) {
        var netMgr = NetworkManager.Instance;
        var data = (JoinMessage)update.Data;
        foreach(var join in data.playerSlots) {
            netMgr.playerSlots[join.Key] = join.Value;
        }
    }

    public override void Register() {
        NetworkManager.Instance.AddUpdateCallback(1, 4, this);
    }
}
