using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UdpResponse : MonoBehaviour
{


    public TextMesh tm = null;

    public void ResponseToUDPPacket(string argIncomingIP, string argIncomingPort, byte[] data)
    {

        if (tm != null)
            tm.text = System.Text.Encoding.UTF8.GetString(data);

#if !UNITY_EDITOR

        //ECHO 
        UdpComm comm = UdpComm.Instance;
        comm.SendMessageUDP(argIncomingIP, comm.ExternalPortTosendTo, data);

#endif
    }
}