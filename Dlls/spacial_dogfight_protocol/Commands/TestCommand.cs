using GameProtocol;
using spacial_dogfight_protocol.Messages;
using spacial_dogfight_protocol.Updates;
using System;
using System.Collections;

namespace spacial_dogfight_protocol.Commands {
    public class TestCommand : Command {

        public TestCommand() {
            Category = 0;
            Code = 1;
            Data = new TestMessage();
        }

        public override byte[] BuildCommand() {
            var dataBytes = Data.ReadFields();
            var byteCount = 2 + dataBytes.Length;
            var resultBytes = new byte[byteCount];
            resultBytes[0] = (byte)Category;
            resultBytes[1] = (byte)Code;
            Buffer.BlockCopy(dataBytes, 0, resultBytes, 2, dataBytes.Length);

            return resultBytes;
        }

        public override string DebugString() {
            throw new NotImplementedException();
        }

        public override Update ProcessCommand(PlayerSession player) {
            Console.WriteLine(((TestMessage)Data).myMessage);
            var update = new TestUpdate();
            ((TestMessage)update.Data).myMessage = "Toi même";
            return update;
        }
    }
}
