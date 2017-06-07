using GameProtocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace spacial_dogfight_protocol.Messages {
    public class JoinMessage : Message {
        public Dictionary<int, int> playerSlots;

        public JoinMessage() {
            playerSlots = new Dictionary<int, int>();
        }

        public byte[] ReadFields() {
            var intRepresentation = sizeof(int);
            var count = playerSlots.Count;
            byte[] buffer;
            if (count > 0) {
                buffer = new byte[intRepresentation * (2 * playerSlots.Count + 1)];
                var cmpt = 0;
                byte[] playerIDRep, playerSlotRep;
                // Write count first
                var countRep = BitConverter.GetBytes(count);
                Buffer.BlockCopy(countRep, 0, buffer, 0, countRep.Length);
                cmpt++;

                foreach (var slot in playerSlots) {
                    playerSlotRep = BitConverter.GetBytes(slot.Key);
                    playerIDRep = BitConverter.GetBytes(slot.Value);
                    Buffer.BlockCopy(playerSlotRep, 0, buffer, cmpt * intRepresentation, playerSlotRep.Length);
                    cmpt++;
                    Buffer.BlockCopy(playerIDRep, 0, buffer, cmpt * intRepresentation, playerIDRep.Length);
                    cmpt++;
                }
            }
            else {
                buffer = new byte[sizeof(int)];
                var countRep = BitConverter.GetBytes(0);
                Buffer.BlockCopy(countRep, 0, buffer, 0, countRep.Length);
            }
            
            return buffer;
        }

        public bool WriteFields(byte[] fields) {
            playerSlots.Clear();
            var cmpt = 0;
            var intRepresentation = sizeof(int);
            var count = BitConverter.ToInt32(fields, 0);
            cmpt++;
            int playerID, playerSlot;
            for(var i = 0; i < count; i++) {
                playerSlot = BitConverter.ToInt32(fields, cmpt * intRepresentation);
                cmpt++;
                playerID = BitConverter.ToInt32(fields, cmpt * intRepresentation);
                cmpt++;

                playerSlots[playerSlot] = playerID;
            }

            return true;
        }
    }
}
