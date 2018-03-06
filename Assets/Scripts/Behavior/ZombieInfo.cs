using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieInfo : MonoBehaviour {

    #region dependencies
    TextMesh _zinfoMesh;
    ZombieBehavior _ZBEH;
    #endregion

    #region INITandListeners
    void Awake () {
        _ZBEH = GetComponent<ZombieBehavior>();
        _zinfoMesh = GetComponentInChildren<TextMesh>();
    }

    private void OnEnable()
    {
        _ZBEH.OnZombieStateChanged += ShowState;
    }

    private void OnDisable()
    {
        _ZBEH.OnZombieStateChanged -= ShowState;
    }
    #endregion

    #region PrivateMethods
    void ShowState(ZombieState argstate) {
        _zinfoMesh.text = argstate.ToString() ;
    }
    #endregion
}
