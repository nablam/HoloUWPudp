
using System.Text;
using UnityEngine;

public class UdpKeyboardSender : MonoBehaviour {

    public UDPCommunication UdpCOMM;
   // public TextDisplay TextDisplayer;

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

            if (cnt % 1 == 0)
            {
                UdpCOMM.SendUDPMessage(UdpCOMM.GetExternalIP(), UdpCOMM.GetExternalPort(), Encoding.UTF8.GetBytes("auto send " + cnt.ToString()));
               // TextDisplayer.DisplayToTextMesh("auto send " + cnt.ToString());

            }

        }
        else {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                cnt++;
                UdpCOMM.SendUDPMessage(UdpCOMM.GetExternalIP(), UdpCOMM.GetExternalPort(), Encoding.UTF8.GetBytes("auto send " + cnt.ToString()));

               // TextDisplayer.DisplayToTextMesh("auto send " + cnt.ToString());
            }
        }

    }
}
