﻿using GameProtocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace spacial_dogfight_protocol.Messages {
    class PingMessage : Message {
        public byte[] ReadFields() {
            return new byte[0];
        }

        public bool WriteFields(byte[] fields) {
            return true;
        }
    }
}