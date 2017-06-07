using GameProtocol;
using spacial_dogfight_protocol.Messages;
using spacial_dogfight_protocol.States;
using spacial_dogfight_protocol.Updates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace spacial_dogfight_protocol.Commands {
   public class PositionCommand : Command {

        public PositionCommand() {
            Category = 1;
            Code = 1;
            Data = new PositionMessage();
        }

        public override string DebugString() {
            return "Position Command";
        }

        public override Update ProcessCommand(PlayerSession player) {
            var data = (PositionMessage)Data;
            var dataStore = Datastore.Instance;
            var exist = dataStore.RetrieveGameObject(data.playerID, data.gid);
            if(null == exist) {
                var state = new GameObjectState();
                state.playerID = data.playerID;
                state.gid = data.gid;
                state.x = data.x;
                state.y = data.y;
                state.rotation = data.rotation;
                dataStore.StoreGameObject(state);
            }
            else {
                exist.x = data.x;
                exist.y = data.y;
                exist.rotation = data.rotation;
            }

            var update = new PositionUpdate();
            ((PositionMessage)update.Data).playerID = data.playerID;
            ((PositionMessage)update.Data).gid = data.gid;
            ((PositionMessage)update.Data).x = data.x;
            ((PositionMessage)update.Data).y = data.y;
            ((PositionMessage)update.Data).rotation = data.rotation;

            return update;
        }
    }
}
