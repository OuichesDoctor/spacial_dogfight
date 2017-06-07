using GameProtocol;
using spacial_dogfight_protocol.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace spacial_dogfight_protocol.Updates {
    public class PingUpdate : Update {

        public PingUpdate() {
            Category = 1;
            Code = 3;
            Data = new PingMessage();
            BroadCasted = false;
        }

        public override byte[] BuildUpdate(PlayerSession player) {
            var byteCount = 2;
            var resultBytes = new byte[byteCount];
            resultBytes[0] = (byte)Category;
            resultBytes[1] = (byte)Code;

            return resultBytes;
        }

        public override string DebugString() {
            return "PONG";
        }
    }
}
