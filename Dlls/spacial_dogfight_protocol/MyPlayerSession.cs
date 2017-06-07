using GameProtocol;
using spacial_dogfight_protocol.Messages;
using spacial_dogfight_protocol.States;
using spacial_dogfight_protocol.Updates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace spacial_dogfight_protocol {
    public class MyPlayerSession : PlayerSession {

        static private int sNextPlayerID = 1;

        public int playerID;

        public MyPlayerSession(IPEndPoint remoteEndPoint) : base(remoteEndPoint) {
            playerID = sNextPlayerID;
            sNextPlayerID++;
        }

    }
}

