using GameProtocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace spacial_dogfight_protocol.Messages {
    public class PlayerIDMessage : Message {
        public int playerID;

        public byte[] ReadFields() {
            var playerIDRepresentation = BitConverter.GetBytes(playerID);
            var floatRepresentation = sizeof(float);
            var buffer = new byte[floatRepresentation];
            Buffer.BlockCopy(playerIDRepresentation, 0, buffer, 0, playerIDRepresentation.Length);
            return buffer;
        }

        public bool WriteFields(byte[] fields) {
            playerID = BitConverter.ToInt32(fields, 0);
            return true;
        }
    }
}
