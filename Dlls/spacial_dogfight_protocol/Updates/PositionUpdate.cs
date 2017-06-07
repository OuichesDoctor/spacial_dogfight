using GameProtocol;
using spacial_dogfight_protocol.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace spacial_dogfight_protocol.Updates {
    public class PositionUpdate : Update {

        public PositionUpdate() {
            Category = 1;
            Code = 1;
            Data = new PositionMessage();
            BroadCasted = false;
        }

        public override string DebugString() {
            return "Send position : " + ((PositionMessage)Data).x + ":" + ((PositionMessage)Data).y;
        }

        public override byte[] BuildUpdate(PlayerSession player) {
            var positionMsg = (PositionMessage)Data;
            var dataBytes = Data.ReadFields();
            var byteCount = 2 + dataBytes.Length;
            var resultBytes = new byte[byteCount];
            resultBytes[0] = (byte)Category;
            resultBytes[1] = (byte)Code;
            Buffer.BlockCopy(dataBytes, 0, resultBytes, 2, dataBytes.Length);

            return resultBytes;
        }
    }
}

