using GameProtocol;
using spacial_dogfight_protocol.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace spacial_dogfight_protocol.Updates {
    public class JoinUpdate : Update {

        public JoinUpdate() {
            Category = 1;
            Code = 4;
            Data = new JoinMessage();
            BroadCasted = true;
        }

        public override byte[] BuildUpdate(PlayerSession player) {
            var session = (MyPlayerSession)player;

            var dataBytes = Data.ReadFields();
            var byteCount = 2 + dataBytes.Length;
            var resultBytes = new byte[byteCount];
            resultBytes[0] = (byte)Category;
            resultBytes[1] = (byte)Code;
            Buffer.BlockCopy(dataBytes, 0, resultBytes, 2, dataBytes.Length);

            return resultBytes;
        }

        public override string DebugString() {
            var jMessage = (JoinMessage)Data;
            return "JoinUpdate";
        }

    }
}
