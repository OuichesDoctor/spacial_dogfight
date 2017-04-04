using GameProtocol;
using System;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
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

    public string ServerIPAddress;
    public int ServerPort;

    public string ServerAddress { get; private set; }
    public int Port { get; private set; }
    public ClientStatus Status { get; private set; }
    public string IpAddress { get; set; }

    private UdpClient _client;
    private UdpClient _broadcastClient;


    protected Process _serverProcess;

	void Start () {
		
	}
	
	void Update () {
		
	}

    private void OnDestroy() {
        _serverProcess.Kill();
    }

    public void StartAsServer() {
        _serverProcess = new Process();
        _serverProcess.StartInfo.FileName = "E:\\repositories\\spacial_dogfight\\Assets\\Externals\\Server\\spacial_dogfight_server.exe";
        if (!_serverProcess.Start())
            UnityEngine.Debug.Log("Cannot start server");
        else
            UnityEngine.Debug.Log("Server launched");

        Initialize(IPAddress.Loopback.ToString(), 1100);
        SceneManager.LoadScene("Main");
        GetComponent<CameraManager>().enabled = true;
        Connect();
    }

    public void StartAsClient() {
        Initialize(IpAddress, 1100);
        Connect();
    }

    public NetworkManager() {
        Status = ClientStatus.NotInitialized;
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

    public void Connect() {
        Status = ClientStatus.Connecting;

        _client = new UdpClient();
        _client.EnableBroadcast = true;
        _client.Connect(IPAddress.Parse(ServerAddress), Port);
        _broadcastClient = new UdpClient();
        _broadcastClient.EnableBroadcast = true;
        _broadcastClient.Connect(new IPEndPoint(IPAddress.Parse("192.168.0.255"), Port));
        Status = ClientStatus.Connected;

        if (Status != ClientStatus.Disconnecting) {
            _client.BeginReceive(new AsyncCallback(ReceiveCallback), null);
            _broadcastClient.BeginReceive(new AsyncCallback(BroadCastCallback), null);
        }
        else {
            _broadcastClient.Close();
            _client.Close();
            Status = ClientStatus.Disconnected;
        }
    }

    public void ReceiveCallback(IAsyncResult result) {
        IPEndPoint localEndPoint = new IPEndPoint(IPAddress.Any, Port);
        var receivedBuffer = _client.EndReceive(result, ref localEndPoint);
        Update update = GameProtocol.Update.GetUpdate(ref receivedBuffer);
        if (update == null) {
            UnityEngine.Debug.Log("Update Unrecognized.");
            return;
        }

        UnityEngine.Debug.Log(update.DebugString());

        if (!update.Data.WriteFields(receivedBuffer)) {
            UnityEngine.Debug.Log("Error when reading parameters");
            return;
        }

        update.ProcessUpdate();

        if (Status != ClientStatus.Disconnecting) {
            _client.BeginReceive(new AsyncCallback(ReceiveCallback), null);
        }
        else {
            _broadcastClient.Close();
            _client.Close();
            Status = ClientStatus.Disconnected;
        }
    }

    public void BroadCastCallback(IAsyncResult result) {
        UnityEngine.Debug.Log("Receive Broadcast");
        IPEndPoint localEndPoint = new IPEndPoint(IPAddress.Any, Port);
        var receivedBuffer = _broadcastClient.EndReceive(result, ref localEndPoint);
        Update update = GameProtocol.Update.GetUpdate(ref receivedBuffer);
        if (update == null) {
            UnityEngine.Debug.Log("Update Unrecognized.");
            return;
        }

        UnityEngine.Debug.Log(update.DebugString());

        if (!update.Data.WriteFields(receivedBuffer)) {
            UnityEngine.Debug.Log("Error when reading parameters");
            return;
        }

        update.ProcessUpdate();

        if (Status != ClientStatus.Disconnecting) {
            _broadcastClient.BeginReceive(new AsyncCallback(ReceiveCallback), null);
        }
        else {
            _broadcastClient.Close();
            _client.Close();
            Status = ClientStatus.Disconnected;
        }
    }
}
