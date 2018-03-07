using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class udpKeyboardSend : MonoBehaviour {

    public UDPCommunication _COMM;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.Space)) {
            _COMM.SendUDPMessage(_COMM.externalIP, _COMM.externalPort, Encoding.UTF8.GetBytes(_COMM.PingMessage));
            }
	}
}
