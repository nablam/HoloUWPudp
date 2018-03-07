// @Author Nabil Lamriben ©2018

using System.Text;
using UnityEngine;

public class UdpKeyboardHelper : MonoBehaviour {

    public UdpComm UdpCOMM;
    public TextDisplay TextDisplayer;

    int cnt = 0;
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            cnt++;
            UdpCOMM.SendMessageUDP(UdpCOMM.ExternalIP_aka_IpofMyAudience, UdpCOMM.ExternalPortTosendTo, Encoding.UTF8.GetBytes(UdpCOMM.PingMessage + cnt.ToString()));
        }
    }
}
