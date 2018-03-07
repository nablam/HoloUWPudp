
using System.Text;
using UnityEngine;

public class UdpKeyboardSender : MonoBehaviour {

    public UDPCommunication UdpCOMM;
    public TextDisplay TextDisplayer;

    int cnt = 0;

    bool toggleAutosend = false;
    void Update()
    {
        
        if (Input.GetKeyDown(KeyCode.A))
        {
            toggleAutosend = !toggleAutosend;
        }

        if (toggleAutosend)
        {
            cnt++;
            UdpCOMM.SendUDPMessage(UdpCOMM.GetExternalIP(), UdpCOMM.GetExternalPort(), Encoding.UTF8.GetBytes("auto send " + cnt.ToString()));

            TextDisplayer.DisplayToTextMesh("auto send " + cnt.ToString());
        }
        else {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                cnt++;
                UdpCOMM.SendUDPMessage(UdpCOMM.GetExternalIP(), UdpCOMM.GetExternalPort(), Encoding.UTF8.GetBytes(UdpCOMM.PingMessage + cnt.ToString()));

                TextDisplayer.DisplayToTextMesh("sending " + UdpCOMM.PingMessage + cnt.ToString());
            }
        }

    }
}
