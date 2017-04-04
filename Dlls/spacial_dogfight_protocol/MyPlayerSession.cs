using GameProtocol;
using spacial_dogfight_protocol.States;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace spacial_dogfight_protocol {
    public class MyPlayerSession : PlayerSession {

        public CharacterState characterState;

        public MyPlayerSession(IPEndPoint remoteEndPoint) : base(remoteEndPoint) {
            characterState = new CharacterState(0, 0);
        }

    }
}

