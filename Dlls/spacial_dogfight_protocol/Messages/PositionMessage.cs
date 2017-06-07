using GameProtocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace spacial_dogfight_protocol.Messages {
    public class PositionMessage : Message {

        public int gid;
        public int playerID;
        public float x;
        public float y;
        public float rotation;

        public byte[] ReadFields() {
            var gidRepresentation = BitConverter.GetBytes(gid);
            var nidRepresentation = BitConverter.GetBytes(playerID);
            var xRepresentation = BitConverter.GetBytes(x);
            var yRepresentation = BitConverter.GetBytes(y);
            var rotationRepresentation = BitConverter.GetBytes(rotation);
            var floatRepresentation = sizeof(float);
            var buffer = new byte[floatRepresentation * 5];
            Buffer.BlockCopy(gidRepresentation, 0, buffer, 0, gidRepresentation.Length);
            Buffer.BlockCopy(nidRepresentation, 0, buffer, floatRepresentation, nidRepresentation.Length);
            Buffer.BlockCopy(xRepresentation, 0, buffer, floatRepresentation * 2, xRepresentation.Length);
            Buffer.BlockCopy(yRepresentation, 0, buffer, floatRepresentation * 3, yRepresentation.Length);
            Buffer.BlockCopy(rotationRepresentation, 0, buffer, floatRepresentation * 4, rotationRepresentation.Length);
            return buffer;
        }

        public bool WriteFields(byte[] fields) {
            var floatRepresentation = sizeof(float);
            gid = BitConverter.ToInt32(fields, 0);
            playerID = BitConverter.ToInt32(fields, floatRepresentation);
            x = BitConverter.ToSingle(fields, floatRepresentation * 2);
            y = BitConverter.ToSingle(fields, floatRepresentation * 3);
            rotation = BitConverter.ToSingle(fields, floatRepresentation * 4);
            return true;
        }
    }
}
