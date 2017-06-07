using GameProtocol;
using spacial_dogfight_protocol.Messages;
using spacial_dogfight_protocol.States;
using spacial_dogfight_protocol.Updates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace spacial_dogfight_protocol.Commands {
    public class StartCommand : Command {

        public StartCommand() {
            Category = 1;
            Code = 6;
            Data = new NoMessage();
        }

        public override string DebugString() {
            return "START GAME";
        }

        public override Update ProcessCommand(PlayerSession player) {
            var dataStore = Datastore.Instance;
            dataStore.gameState = GameState.MAIN;

            return new StartUpdate();
        }
    }
}
