using GameProtocol;
using GameServer;
using spacial_dogfight_protocol;
using spacial_dogfight_protocol.Messages;
using spacial_dogfight_protocol.States;
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

            var session = new MyPlayerSession(remoteEndPoint);
            var response = new PlayerIDUpdate();
            ((PlayerIDMessage)response.Data).playerID = session.playerID;
            var sendBuffer = response.BuildUpdate(session);
            Console.WriteLine(response.DebugString());
            SendData(sendBuffer, session.RemoteEndPoint);

            // Send slots states
            var playerSlots = Datastore.Instance.GetPlayerSlots();
            if(playerSlots.Count > 0) {
                var update = new JoinUpdate();
                ((JoinMessage)update.Data).playerSlots = playerSlots;
                sendBuffer = update.BuildUpdate(session);
                SendData(sendBuffer, session.RemoteEndPoint);
            }

            return session;
        }

        protected override void ServerUpdate() {
            var current = DateTime.Now;
            var random = new Random();
            var dataStore = Datastore.Instance;

            //if (dataStore.gameState == GameState.MAIN && current - _lastPositionUdpate > TimeSpan.FromMilliseconds(int.Parse(ConfigurationManager.AppSettings["updateRate"]))) {
            //    var ships = dataStore.GetShipStates();
            //    if(ships.Count > 0) {
            //        ShipMoveUpdate update;
            //        ShipState state;
            //        lock(ships) {
            //            foreach(var ship in ships) {
            //                update = new ShipMoveUpdate();
            //                state = ship.Value;
            //                ((ShipMoveMessage)update.Data).shipId = state.shipID;
            //                ((ShipMoveMessage)update.Data).x = state.x;
            //                ((ShipMoveMessage)update.Data).y = state.y;
            //                ((ShipMoveMessage)update.Data).rotation = state.rotation;

            //                lock (_playerList) {
            //                    foreach (var player in _playerList) {
            //                        Console.WriteLine("BROADCAST : " + update.DebugString());
            //                        var sendBuffer = update.BuildUpdate(player.Value);
            //                        BroadCastData(sendBuffer);
            //                    }
            //                }
            //            }
            //        }
            //    }
            //}
        }

    }
}
