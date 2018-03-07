// @Author Nabil Lamriben ©2018
using UnityEngine;
using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using HoloToolkit.Unity;
using UnityEngine.Events;

#if !UNITY_EDITOR
using Windows.Networking.Sockets;
using Windows.Networking.Connectivity;
using Windows.Networking;
#endif


[System.Serializable]
public class EventUDPMessage : UnityEvent<string, string, byte[]>
{

}

public class UdpComm : Singleton<UdpComm>
{
    [Tooltip("port to listen for incoming data my ear")]
    public string MyInternalPort_aka_MyEar = "12345";

    [Tooltip("IP-Address for sending")]
    public string ExternalIP_aka_IpofMyAudience = "192.168.1.2";

    [Tooltip("Port for sending Ear of my Audience")]
    public string ExternalPortTosendTo = "12346";

    [Tooltip("Send a message at Startup")]
    public bool sendPingAtStart = true;

    [Tooltip("Content of Ping")]
    public string PingMessage = "cliked ";

    [Tooltip("Function to invoke at incoming packet")]
    public EventUDPMessage udpEvent = null;

    private readonly Queue<Action> ExecuteOnMainThread = new Queue<Action>();


    public bool IsMSI_Server;
    public bool IsMSI_Client;

    public bool IsJalt_Server;
    public bool IsJalt_Client;
    public bool IsHolo_Client;




#if !UNITY_EDITOR

    //we've got a message (data[]) from (host) in case of not assigned an event
    void UDPMessageReceived(string host, string port, byte[] data)
    {
        Debug.Log("GOT MESSAGE FROM: " + host + " on port " + port + " " + data.Length.ToString() + " bytes ");
    }

    //Send an UDP-Packet
    public async void SendMessageUDP(string HostIP, string HostPort, byte[] data)
    {
        await TASK_SendMessageUDP(HostIP, HostPort, data);
    }



    DatagramSocket socket;

    async void Start()
    {
        if (udpEvent == null)
        {
            udpEvent = new EventUDPMessage();
            udpEvent.AddListener(UDPMessageReceived);
        }


        Debug.Log("Waiting for a connection...");

        socket = new DatagramSocket();
        socket.MessageReceived += Socket_MessageReceived;

        HostName IP = null;
        try
        {
            var icp = NetworkInformation.GetInternetConnectionProfile();

            IP = Windows.Networking.Connectivity.NetworkInformation.GetHostNames()
            .SingleOrDefault(
                hn =>
                    hn.IPInformation?.NetworkAdapter != null && hn.IPInformation.NetworkAdapter.NetworkAdapterId
                    == icp.NetworkAdapter.NetworkAdapterId);

            await socket.BindEndpointAsync(IP, MyInternalPort_aka_MyEar);
        }
        catch (Exception e)
        {
            Debug.Log(e.ToString());
            Debug.Log(SocketError.GetStatus(e.HResult).ToString());
            return;
        }

        if(sendPingAtStart)
            SendMessageUDP(ExternalIP_aka_IpofMyAudience, ExternalPortTosendTo, Encoding.UTF8.GetBytes(PingMessage));

    }




    private async System.Threading.Tasks.Task TASK_SendMessageUDP(string argExternalIP, string argExternalPort, byte[] data)
    {
        using (var stream = await socket.GetOutputStreamAsync(new Windows.Networking.HostName(argExternalIP), argExternalPort))
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

    public void SendMessageUDP(string HostIP, string HostPort, byte[] data)
    {

    }

#endif


    static MemoryStream ToMemoryStream(Stream input)
    {
        try
        {                                         // Read and write in
            byte[] block = new byte[0x1000];       // blocks of 4K.
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
        Stream incommingStream = args.GetDataStream().AsStreamForRead();
        MemoryStream memstream = ToMemoryStream(incommingStream);
        byte[] msgData = memstream.ToArray();

        if (ExecuteOnMainThread.Count == 0)
        {
            ExecuteOnMainThread.Enqueue(() =>
            {
                Debug.Log("nq ");
                if (udpEvent != null)
                    udpEvent.Invoke(args.RemoteAddress.DisplayName, MyInternalPort_aka_MyEar, msgData);
            });
        }
    }


#endif
}