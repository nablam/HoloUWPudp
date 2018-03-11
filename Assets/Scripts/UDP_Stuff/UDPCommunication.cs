using UnityEngine;
using System;
using System.IO;
using System.Text;
using System.Linq;
using HoloToolkit.Unity;
using System.Collections.Generic;
using UnityEngine.Events;

#if !UNITY_EDITOR
using Windows.Networking.Sockets;
using Windows.Networking.Connectivity;
using Windows.Networking;
#endif

[System.Serializable]
public class UDPMessageEvent : UnityEvent<string, string, byte[]>
{

}

public class UDPCommunication : Singleton<UDPCommunication>
{

    public TextMesh MyInfo;
     string internalPort = "";

     string externalIP = "";

    
     string externalPort = "";

    UDPInitSctuct _MyUdpTructure;

    public string GetExternalPort() { return externalPort; }
    public string GetExternalIP() { return externalIP; }

    [Tooltip("Conten of Ping")]
    public string PingMessage = "cliked ";

    [Tooltip("Function to invoke at incoming packet")]
    public UDPMessageEvent udpEvent = null;

    private readonly Queue<Action> ExecuteOnMainThread = new Queue<Action>();

    public bool MeIsServer;

    //my other is a
    public UDPmachine otherMachineType;

    void BuildMyStructure()
    {
        string _MyEAR="";
        string _AudienceIP = "";
        string _AudienceEAR = "";
        //if im a client , all i need to specify is my listenport which will always be 12345
        if (!MeIsServer)
        {
            _MyEAR = "12345";
            _AudienceIP = "192.168.1.7";
            _AudienceEAR = "12346";
        }
        else
        {
            _MyEAR = "12346";
            _AudienceEAR = "12345";
            //who am i calling 
            switch (otherMachineType)
            {
                case UDPmachine.MSI_2:
                    _AudienceIP = "192.168.1.7";
                    break;
                case UDPmachine.Jalt_6:
                    _AudienceIP = "192.168.1.14";
                    break;
                case UDPmachine.Holo_06:
                    _AudienceIP = "192.168.1.15";
                    break;
                case UDPmachine.Holo_15:
                    _AudienceIP = "192.168.1.10";
                    break;
            }

        }

        internalPort = _MyEAR;
        externalIP = _AudienceIP;
        externalPort = _AudienceEAR;

        UpdateInfoText();
    }

    void UpdateInfoText() {
        if (!MeIsServer)
        {
           
            MyInfo.text = "listening to " + otherMachineType.ToString() + " on my internal port " + internalPort;
        }
        else {
            MyInfo.text = "Serving" + otherMachineType.ToString() + " at  " + externalIP+  " to its port "+ externalPort;
        }
    }
 

    void SetupClientInternalPort() { }

  

    private void Awake()
    {
        BuildMyStructure();
    }

#if !UNITY_EDITOR

      private void OnEnable()
    {

        if (udpEvent == null)
        {
            udpEvent = new UDPMessageEvent();
            udpEvent.AddListener(UDPMessageReceived);
        }
    }

    private void OnDisable()
    {
        if (udpEvent != null)
        {
            
            udpEvent.RemoveAllListeners();
        }
    }



    //we've got a message (data[]) from (host) in case of not assigned an event
    void UDPMessageReceived(string host, string port, byte[] data)
    {
        Debug.Log("GOT MESSAGE FROM: " + host + " on port " + port + " " + data.Length.ToString() + " bytes ");
     Console3D.Instance.LOGit("GOT MESSAGE FROM: " + host + " on port " + port + " " + data.Length.ToString() + " bytes ");
    }

    //Send an UDP-Packet
    public async void SendUDPMessage(string HostIP, string HostPort, byte[] data)
    {
        await _SendUDPMessage(HostIP, HostPort, data);
    }



    DatagramSocket socket;

    async void Start()
    {
    
      


        Debug.Log("Waiting for a connection...");

        socket = new DatagramSocket();
        socket.MessageReceived += Socket_MessageReceived;

        HostName IP = null;
        try
        {
            var icp = NetworkInformation.GetInternetConnectionProfile();

         Console3D.Instance.LOGit("icp.netadaptor id = " + icp.NetworkAdapter.NetworkAdapterId.ToString());

            IP = Windows.Networking.Connectivity.NetworkInformation.GetHostNames()
            .SingleOrDefault(
                hn =>
                    hn.IPInformation?.NetworkAdapter != null && hn.IPInformation.NetworkAdapter.NetworkAdapterId
                    == icp.NetworkAdapter.NetworkAdapterId);
         Console3D.Instance.LOGit("IP is= " + IP.ToString());
         Console3D.Instance.LOGit("my socket is = " + IP.ToString() + " " + internalPort);
            await socket.BindEndpointAsync(IP, internalPort);
        }
        catch (Exception e)
        {
            Debug.Log(e.ToString());
            Debug.Log(SocketError.GetStatus(e.HResult).ToString());
            return;
        }
        //SendUDPMessage(externalIP, externalPort, Encoding.UTF8.GetBytes(PingMessage));
    }




    private async System.Threading.Tasks.Task _SendUDPMessage(string argexternalIP, string argexternalPort, byte[] data)
    {
        using (var stream = await socket.GetOutputStreamAsync(new Windows.Networking.HostName(argexternalIP), argexternalPort))
        {
            using (var writer = new Windows.Storage.Streams.DataWriter(stream))
            {
                writer.WriteBytes(data);
                await writer.StoreAsync();

            }
        }
    }


#else


    // to make Unity-Editor happy :-)
    void Start()
    {

    }

    public void SendUDPMessage(string HostIP, string HostPort, byte[] data)
    {

    }

#endif


    static MemoryStream ToMemoryStream(Stream input)
    {
        try
        {                                         // Read and write in
            byte[] block = new byte[0x1000];       // blocks of 4K. 1000
            MemoryStream ms = new MemoryStream();
            while (true)
            {
                int bytesRead = input.Read(block, 0, block.Length);
                if (bytesRead == 0) return ms;
                ms.Write(block, 0, bytesRead);
            }
        }
        finally { }
    }

    // Update is called once per frame
    void Update()
    {
        while (ExecuteOnMainThread.Count > 0)
        {
            ExecuteOnMainThread.Dequeue().Invoke();

        }
    }

#if !UNITY_EDITOR
    private void Socket_MessageReceived(Windows.Networking.Sockets.DatagramSocket sender,
        Windows.Networking.Sockets.DatagramSocketMessageReceivedEventArgs args)
    {
     //   Console3D.Instance.LOGit("GOT MESSAGE FROM: " + args.RemoteAddress.DisplayName);
        //Read the message that was received from the UDP  client.
        Stream streamIn = args.GetDataStream().AsStreamForRead();
        MemoryStream ms = ToMemoryStream(streamIn);
        byte[] msgData = ms.ToArray();


        if (ExecuteOnMainThread.Count == 0)
        {
            ExecuteOnMainThread.Enqueue(() =>
            {
          //      Console3D.Instance.LOGit("ENQEUED ");
                if (udpEvent != null)
                    udpEvent.Invoke(args.RemoteAddress.DisplayName, internalPort, msgData);
            });
        }
    }


#endif
}
