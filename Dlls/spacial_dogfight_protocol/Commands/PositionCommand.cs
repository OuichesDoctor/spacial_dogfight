using GameProtocol;
using spacial_dogfight_protocol.Messages;
using spacial_dogfight_protocol.Updates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace spacial_dogfight_protocol.Commands {
   public class PositionCommand : Command {

        public PositionCommand() {
            Category = 0;
            Code = 1;
            Data = new PositionMessage();
        }

        public override string DebugString() {
            return "Position Command";
        }

        public override Update ProcessCommand(PlayerSession player) {
            var mySession = (MyPlayerSession)player;
            var data = (PositionMessage)Data;
            mySession.characterState.x = data.x;
            mySession.characterState.y = data.y;

            var update = new PositionUpdate();
            ((PositionMessage)update.Data).x = data.x;
            ((PositionMessage)update.Data).y = data.y;

            return update;
        }
    }
}
