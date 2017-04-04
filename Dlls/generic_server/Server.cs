using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Configuration;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Threading.Tasks;
using System.Threading;
using GameProtocol;

namespace GameServer {
    public enum ServerStatus {
        NotInitialized,
        Initialized,
        Starting,
        Running,
        Terminating,
        Stopped
    }

    /// <summary>
    /// A simple UDP server
    /// Base on exemple here : https://social.msdn.microsoft.com/Forums/en-US/92846ccb-fad3-469a-baf7-bb153ce2d82b/simple-udp-example-code?forum=netfxnetcom
    /// </summary>
    public abstract class Server {

        public ServerStatus Status { get; private set; }

        protected int _listenPort;
        protected UdpClient _listener;
        protected IPEndPoint _endPoint;
        protected Dictionary<string, PlayerSession> _playerList;
        protected DateTime _lastUpdate;

        public Server() {
            Status = ServerStatus.NotInitialized;
        }

        public string Initialize() {
            try {
                _listenPort = int.Parse(ConfigurationManager.AppSettings["listenPort"]);
            }
            catch(ConfigurationErrorsException) {
                return "Something went wrong with the listening port configuration. Please check your App.config file.";
            }

            Status = ServerStatus.Initialized;
            _playerList = new Dictionary<string, PlayerSession>();
            return null;
        }

        public void Start() {
            Task.Run(async () => {
                Status = ServerStatus.Starting;
                _endPoint = new IPEndPoint(IPAddress.Any, _listenPort);
                _listener = new UdpClient(_endPoint);

                try {
                    Status = ServerStatus.Running;
                    while (Status == ServerStatus.Running) {
                        ProcessReceive(await _listener.ReceiveAsync());
                    }
                }
                catch (Exception e) {
                    Console.WriteLine(e.ToString());
                }
                Status = ServerStatus.Terminating;

                _listener.Close();
                Console.WriteLine("Server terminated");

                Status = ServerStatus.Stopped;
            });

            while (Status != ServerStatus.Running) {
                Thread.Sleep(100);
            }

            Task.Run(() => {
                _lastUpdate = DateTime.Now;
                while (Status == ServerStatus.Running) {
                    // Handle timed out player sessions
                    var sessionList = _playerList;
                    var removeCount = 0;
                    var current = DateTime.Now;
                    if (current > _lastUpdate.AddMinutes(1)) {
                        lock(_playerList) {
                            foreach (var session in sessionList) {
                                if (session.Value.Expired) {
                                    _playerList.Remove(session.Key);
                                    removeCount++;
                                }
                            }

                            _lastUpdate = current;
                            Console.WriteLine("Player session cleaning : {0} removed", removeCount);

                            sessionList = _playerList;
                        }
                    }
                    ServerUpdate();
                }
            });
        }

        protected abstract void ServerUpdate();

        protected void ProcessReceive(UdpReceiveResult result) {
            var remoteEndPoint = result.RemoteEndPoint;

            byte[] hash = new SHA256CryptoServiceProvider().ComputeHash(Encoding.UTF8.GetBytes(remoteEndPoint.ToString()));
            string hashString = BitConverter.ToString(hash).Replace("-", String.Empty);

            PlayerSession player;
            if(!_playerList.ContainsKey(hashString)) {
                lock(_playerList) {
                    // New player
                    player = CreatePlayerSession(remoteEndPoint);
                    _playerList.Add(hashString, player);
                }
            }
            else {
                // Already registered player
                player = _playerList[hashString];
                player.MarkActivity();
            }

            var receivedBuffer = result.Buffer;
            Command cmd = Command.GetCommand(ref receivedBuffer);
            if(cmd == null) {
                Console.WriteLine("Command unrecognized.");
                return;
            }

            Console.WriteLine(cmd.DebugString());

            if (!cmd.Data.WriteFields(receivedBuffer)) {
                Console.WriteLine("Error when reading parameters");
                return;
            }

            var update = cmd.ProcessCommand(player);
            if(update != Update.None) {
                var sendBuffer = update.BuildUpdate(player);
                Console.WriteLine(update.DebugString());
                SendData(sendBuffer, player.RemoteEndPoint);
            }
            else {
                Console.WriteLine("No update to send");
            }
            
        }

        public void SendData(byte[] sendBuffer, IPEndPoint remoteEndPoint) {
            
            _listener.SendAsync(sendBuffer, sendBuffer.Length, remoteEndPoint);
        }

        public void BroadCastData(byte[] sendBuffer) {
            lock(_playerList) {
                foreach(var player in _playerList) {
                    _listener.SendAsync(sendBuffer, sendBuffer.Length, player.Value.RemoteEndPoint);
                }
            }
        }

        protected abstract PlayerSession CreatePlayerSession(IPEndPoint remoteEndPoint);
    }
}
