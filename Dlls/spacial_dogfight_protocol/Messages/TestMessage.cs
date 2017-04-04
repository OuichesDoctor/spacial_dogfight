using GameProtocol;
using System;
using System.Text;

namespace spacial_dogfight_protocol.Messages {
    public class TestMessage : Message {

        public string myMessage;

        public byte[] ReadFields() {
            return Encoding.UTF8.GetBytes(myMessage);
        }

        public bool WriteFields(byte[] fields) {
            myMessage = Encoding.UTF8.GetString(fields);
            return true;
        }
    }
}
