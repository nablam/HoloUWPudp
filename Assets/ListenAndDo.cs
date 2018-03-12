using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ListenAndDo : MonoBehaviour {


     TextMesh _tm;
    private void OnEnable()
    {
        _tm = GetComponentInChildren<TextMesh>();
        FakeGameManager.OtherPlayerStreakHandeler += JustDO;
    }

    private void OnDisable()
    {
        FakeGameManager.OtherPlayerStreakHandeler -= JustDO;
    }

    int x = 0;

    void JustDO() {
        x++;
        _tm.text = "i heard it " + x;

    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
