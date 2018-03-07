using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class udpSendKeyboardHelper : MonoBehaviour {
    public UDPCommunication _coomm;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)) {
            _coomm.SendUDPMessage(_coomm.externalIP, _coomm.externalPort, Encoding.UTF8.GetBytes(_coomm.PingMessage));
        }
    }

}
