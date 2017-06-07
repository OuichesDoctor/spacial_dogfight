using GameProtocol;
using spacial_dogfight_protocol.Messages;
using spacial_dogfight_protocol.Updates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace spacial_dogfight_protocol.Commands {
    public class PingCommand : Command {

        public PingCommand() {
            Category = 1;
            Code = 3;
            Data = new PingMessage();
        }

        public override string DebugString() {
            return "PING";
        }

        public override Update ProcessCommand(PlayerSession player) {
            return new PingUpdate();
        }
    }
}
