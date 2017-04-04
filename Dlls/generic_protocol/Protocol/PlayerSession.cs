using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace GameProtocol {
    public abstract class PlayerSession {

        public IPEndPoint RemoteEndPoint { get; private set; }
        public bool Expired {
            get {
                return _lastActivityTime.AddMinutes(30) < DateTime.Now;
            }
        }

        private DateTime _creationTime;
        private DateTime _lastActivityTime;

        public PlayerSession(IPEndPoint remoteEndPoint) {
            RemoteEndPoint = remoteEndPoint;
            _creationTime = DateTime.Now;
            _lastActivityTime = _creationTime;
        }

        public void MarkActivity() {
            _lastActivityTime = DateTime.Now;
        }
    }
}
