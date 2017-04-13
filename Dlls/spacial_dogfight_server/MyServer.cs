using GameProtocol;
using GameServer;
using spacial_dogfight_protocol;
using spacial_dogfight_protocol.Updates;
using System;
using System.Configuration;
using System.Net;

namespace MyGameServer {
    public class MyServer : Server {

        private DateTime _lastPositionUdpate;

        public MyServer() {
            _lastPositionUdpate = DateTime.Now.Subtract(TimeSpan.FromHours(1));
        }

        protected override PlayerSession CreatePlayerSession(IPEndPoint remoteEndPoint) {
            Console.WriteLine("New Player on : " + remoteEndPoint.Address.ToString());
            return new MyPlayerSession(remoteEndPoint);
        }

        protected override void ServerUpdate() {
            var current = DateTime.Now;
            var random = new Random();

            if (current - _lastPositionUdpate > TimeSpan.FromMilliseconds(int.Parse(ConfigurationManager.AppSettings["updateRate"]))) {
                Update positionUpdate = null;
                lock(_playerList) {
                    foreach(var player in _playerList) {
                        positionUpdate = new PositionUpdate();
                        Console.WriteLine("BROADCAST : " + positionUpdate.DebugString());
                        var sendBuffer = positionUpdate.BuildUpdate(player.Value);
                        BroadCastData(sendBuffer);
                    }
                }
            }
        }

    }
}
