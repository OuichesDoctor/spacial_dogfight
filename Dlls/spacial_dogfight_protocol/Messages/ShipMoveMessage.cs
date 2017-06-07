using GameProtocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace spacial_dogfight_protocol.Messages {
    public class ShipMoveMessage : Message {

        public int shipId;
        public float x;
        public float y;
        public float rotation;

        public byte[] ReadFields() {
            var shipIDRepresentation = BitConverter.GetBytes(shipId);
            var xRepresentation = BitConverter.GetBytes(x);
            var yRepresentation = BitConverter.GetBytes(y);
            var rotationRepresentation = BitConverter.GetBytes(rotation);
            var floatRepresentation = sizeof(float);
            var buffer = new byte[floatRepresentation * 4];
            Buffer.BlockCopy(shipIDRepresentation, 0, buffer, 0, shipIDRepresentation.Length);
            Buffer.BlockCopy(xRepresentation, 0, buffer, floatRepresentation, xRepresentation.Length);
            Buffer.BlockCopy(yRepresentation, 0, buffer, floatRepresentation * 2, yRepresentation.Length);
            Buffer.BlockCopy(rotationRepresentation, 0, buffer, floatRepresentation * 3, rotationRepresentation.Length);
            return buffer;
        }

        public bool WriteFields(byte[] fields) {
            var floatRepresentation = sizeof(float);
            shipId = BitConverter.ToInt32(fields, 0);
            x = BitConverter.ToSingle(fields, floatRepresentation);
            y = BitConverter.ToSingle(fields, floatRepresentation * 2);
            rotation = BitConverter.ToSingle(fields, floatRepresentation * 3);
            return true;
        }
    }
}
