using GameProtocol;
using spacial_dogfight_protocol.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace spacial_dogfight_protocol.Updates {
    public class TestUpdate : Update {

        public TestUpdate() {
            Category = 0;
            Code = 1;
            Data = new TestMessage();
        }

        public override string DebugString() {
            return "Received update TestUpdate()";
        }

        public override byte[] BuildUpdate(PlayerSession player) {
            var dataBytes = Data.ReadFields();
            var byteCount = 2 + dataBytes.Length;
            var resultBytes = new byte[byteCount];
            resultBytes[0] = (byte)Category;
            resultBytes[1] = (byte)Code;
            Buffer.BlockCopy(dataBytes, 0, resultBytes, 2, dataBytes.Length);

            return resultBytes;
        }

        public override void ProcessUpdate() {
        }
    }
}
