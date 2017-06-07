using GameProtocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace spacial_dogfight_protocol.Messages {
    public class LeaveSlotMessage : Message {
        public int playerSlot;
        public int playerID;

        public byte[] ReadFields() {
            var playerIDRepresentation = BitConverter.GetBytes(playerID);
            var playerSlotRepresentation = BitConverter.GetBytes(playerSlot);
            var intRepresentation = sizeof(int);
            var buffer = new byte[intRepresentation * 2];
            Buffer.BlockCopy(playerIDRepresentation, 0, buffer, 0, playerIDRepresentation.Length);
            Buffer.BlockCopy(playerSlotRepresentation, 0, buffer, intRepresentation, playerSlotRepresentation.Length);
            return buffer;
        }

        public bool WriteFields(byte[] fields) {
            playerID = BitConverter.ToInt32(fields, 0);
            playerSlot = BitConverter.ToInt32(fields, sizeof(int));
            return true;
        }
    }
}
