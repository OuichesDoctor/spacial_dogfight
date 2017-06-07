using GameProtocol;
using spacial_dogfight_protocol.Commands;
using spacial_dogfight_protocol.Messages;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum ClientStatus {
    NotInitialized,
    Initialized,
    Connecting,
    Connected,
    Disconnecting,
    Disconnected
}

public class NetworkManager : MonoBehaviour {

    static public NetworkManager Instance {
        get {
            return _instance;
        }
    }
    static private NetworkManager _instance;

    public int ServerPort;
    public string ServerAddress { get; private set; }
    public int Port { get; private set; }
    public ClientStatus Status { get; private set; }
    public string IpAddress { get; set; }
    public int playerID;
    public Dictionary<int,int> playerSlots;

    public Dictionary<int, List<NetworkCallback>> updateCallbacks;

    private UdpClient _client;
    private List<Update> _updateToProcess;
    protected Process _serverProcess;

    private void OnDestroy() {
        if(_serverProcess != null)
            _serverProcess.Kill();
    }

    public void StartAsServer() {
        var startTime = Time.realtimeSinceStartup;
        _serverProcess = new Process();
        _serverProcess.StartInfo.FileName = Application.dataPath + "\\StreamingAssets\\Server\\spacial_dogfight_server.exe";
        _serverProcess.StartInfo.WindowStyle = ProcessWindowStyle.Minimized;

        if (!_serverProcess.Start())
            UnityEngine.Debug.Log("Cannot start server");
        else
            UnityEngine.Debug.Log("Server launched");

        while(Time.realtimeSinceStartup < startTime + 5);

        Initialize("192.168.0.19", this.ServerPort);
        Connect();
    }

    public void StartAsClient() {
        Initialize(IpAddress, this.ServerPort);
        Connect();
    }

    public void Awake() {
        IpAddress = "192.168.0.19";
        Application.runInBackground = true;
        _updateToProcess = new List<Update>();
        playerSlots = new Dictionary<int, int>();
        updateCallbacks = new Dictionary<int, List<NetworkCallback>>();
        Status = ClientStatus.NotInitialized;
        if(_instance == null)
            _instance = this;
    }

    public void Initialize(string IPaddress, int port) {
        this.ServerAddress = IPaddress;
        this.Port = port;
        Status = ClientStatus.Initialized;
    }

    public void SendCommand(Command commandToSend) {
        try {
            var buffer = commandToSend.BuildCommand();
            _client.Send(buffer, buffer.Length);
        }
        catch (Exception send_exception) {
            UnityEngine.Debug.Log(" Exception " + send_exception.Message);
        }
    }

    public void AddUpdateCallback(int category, int code, NetworkCallback callback) {
        var opcode = category << 8 | code;
        if (!updateCallbacks.ContainsKey(opcode))
            updateCallbacks[opcode] = new List<NetworkCallback>();

        updateCallbacks[opcode].Add(callback);
    }

    public void Connect() {
        Status = ClientStatus.Connecting;

        _client = new UdpClient();
        _client.EnableBroadcast = true;
        _client.Connect(IPAddress.Parse(ServerAddress), Port);
        Status = ClientStatus.Connected;

        StartCoroutine(ProcessUpdates());
        _client.BeginReceive(new AsyncCallback(ReceiveCallback), null);

        while(!_client.Client.IsBound) {
            UnityEngine.Debug.Log("Connecting ...");   
        }

        var cmd = new PingCommand();
        SendCommand(cmd);
    }

    IEnumerator ProcessUpdates() {
        while(Status != ClientStatus.Disconnecting) {
            if(_updateToProcess.Count > 0) {
                var current = _updateToProcess.First();
                if (current == null)
                    continue;
                var opcode = current.Category << 8 | current.Code;
                if(updateCallbacks.ContainsKey(opcode)) {
                    updateCallbacks[opcode].ForEach(x => x.ProcessUpdate(current));
                }

                _updateToProcess.Remove(current);
            }

            yield return null;
        }
    }

    public void ReceiveCallback(IAsyncResult result) {
        IPEndPoint localEndPoint = new IPEndPoint(IPAddress.Parse("192.168.0.19"), Port);

        var receivedBuffer = _client.EndReceive(result, ref localEndPoint);
        Update update = GameProtocol.Update.GetUpdate(ref receivedBuffer);
        if (update == null) {
            UnityEngine.Debug.Log("Update Unrecognized.");
            return;
        }

        if (!update.Data.WriteFields(receivedBuffer)) {
            UnityEngine.Debug.Log("Error when reading parameters");
            return;
        }

        _updateToProcess.Add(update);

        if (Status != ClientStatus.Disconnecting) {
            _client.BeginReceive(new AsyncCallback(ReceiveCallback), null);
        }
        else {
            _client.Close();
            Status = ClientStatus.Disconnected;
        }
    }
}
