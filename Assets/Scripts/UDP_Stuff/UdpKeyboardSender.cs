
using System.Text;
using UnityEngine;

public class UdpKeyboardSender : MonoBehaviour {

    public UDPCommunication UdpCOMM;
    public TextDisplay TextDisplayer;

    int cnt = 0;
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            cnt++;
            UdpCOMM.SendUDPMessage(UdpCOMM.externalIP, UdpCOMM.externalPort, Encoding.UTF8.GetBytes(UdpCOMM.PingMessage + cnt.ToString()));
        }
    }
}
