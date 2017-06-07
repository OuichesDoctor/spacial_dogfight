using GameProtocol;
using spacial_dogfight_protocol.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace spacial_dogfight_protocol.Updates {
    public class StartUpdate : Update {

        public StartUpdate() {
            Category = 1;
            Code = 6;
            Data = new NoMessage();
            BroadCasted = true;
        }

        public override byte[] BuildUpdate(PlayerSession player) {
            var byteCount = 2;
            var resultBytes = new byte[byteCount];
            resultBytes[0] = (byte)Category;
            resultBytes[1] = (byte)Code;

            return resultBytes;
        }

        public override string DebugString() {
            return "OK START";
        }

    }
}
