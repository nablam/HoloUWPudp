using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class MoveWithMouse : MonoBehaviour {

    Vector3 originaPos;
    float BoundariesOffset =2.5f;
    float movedownY = 0.0f;
    float sensitivityY = 0.08f;

    float movedownX = 0.0f;
    float sensitivityX =0.08f;
    // Use this for initialization
    void Start () {
        originaPos = transform.position;

    }

    void DoMoveY()
    {
        movedownY += Input.GetAxis("Mouse Y") * sensitivityY;
        if (Input.GetAxis("Mouse Y") > 0)
        {
            transform.Translate(Vector3.up * movedownY);
        }
        if (Input.GetAxis("Mouse Y") < 0)
        {
            transform.Translate(Vector3.up * movedownY);
        }
      //  print(movedownY);
        movedownY = 0.0f;
    }
    void DoMoveX()
    {
        movedownX += Input.GetAxis("Mouse X") * sensitivityX;
        if (Input.GetAxis("Mouse X") > 0)
        {
            transform.Translate(Vector3.right * movedownX);
        }
        if (Input.GetAxis("Mouse X") < 0)
        {
            transform.Translate(Vector3.right * movedownX);
        }
       // print(movedownX);
        movedownX = 0.0f;
    }

    void DoMoveZ()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow)) { transform.Translate(Vector3.forward * Time.deltaTime); }
        if (Input.GetKeyDown(KeyCode.DownArrow)) { transform.Translate(Vector3.back * Time.deltaTime); }
    }


    void lockBoundaries() {

        if (transform.position.x > originaPos.x + BoundariesOffset) { transform.position = new Vector3(originaPos.x + BoundariesOffset, transform.position.y, transform.position.z) ; }
        if (transform.position.x < originaPos.x - BoundariesOffset) { transform.position = new Vector3(originaPos.x - BoundariesOffset, transform.position.y, transform.position.z); }

        if (transform.position.y > originaPos.y + BoundariesOffset) { transform.position = new Vector3(transform.position.x, originaPos.y + BoundariesOffset, originaPos.z ); }
        if (transform.position.y < originaPos.y - BoundariesOffset) { transform.position = new Vector3(transform.position.x, originaPos.y - BoundariesOffset, originaPos.z ); }

        if (transform.position.z > originaPos.z + BoundariesOffset) { transform.position = new Vector3(transform.position.x, transform.position.y, originaPos.z + BoundariesOffset); }
        if (transform.position.z < originaPos.z - BoundariesOffset) { transform.position = new Vector3(transform.position.x, transform.position.y, originaPos.z - BoundariesOffset); }
    }
    // Update is called once per frame
    void Update () {

        string posstr = "" + this.transform.position.x + "|" + this.transform.position.y + "|" + this.transform.position.z;
    //   UDPCommunication.Instance.SendUDPMessage(UDPCommunication.Instance.GetExternalIP(), UDPCommunication.Instance.GetExternalPort(), Encoding.UTF8.GetBytes(posstr));
        lockBoundaries();
            DoMoveX();
            DoMoveY();
            DoMoveZ();

            if (Input.GetKeyDown(KeyCode.KeypadEnter)) { transform.position = originaPos; }
       
	}
}



