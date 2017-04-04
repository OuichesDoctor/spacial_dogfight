using GameProtocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace spacial_dogfight_protocol.Messages {
    public class PositionMessage : Message {

        public float x;
        public float y;

        public byte[] ReadFields() {
            var xRepresentation = BitConverter.GetBytes(x);
            var yRepresentation = BitConverter.GetBytes(y);
            var floatRepresentation = sizeof(float);
            var buffer = new byte[floatRepresentation * 2];
            Buffer.BlockCopy(xRepresentation, 0, buffer, 0, xRepresentation.Length);
            Buffer.BlockCopy(yRepresentation, 0, buffer, floatRepresentation, yRepresentation.Length);
            return buffer;
        }

        public bool WriteFields(byte[] fields) {
            x = BitConverter.ToSingle(fields, 0);
            y = BitConverter.ToSingle(fields, sizeof(float));
            return true;
        }
    }
}
