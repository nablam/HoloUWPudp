
using System.Text;
using UnityEngine;

public class UdpKeyboardSender : MonoBehaviour {

    int cnt = 0;

    bool toggleAutosend = false;
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Space))
        {
            cnt++;
            UDPcommMNGR.Instance.HelpSendMEssage("yo hello there");
        }
       
    }
}
