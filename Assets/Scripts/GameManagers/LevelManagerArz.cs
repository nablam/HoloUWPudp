// @Author Jeffrey M. Paquette ©2016

using UnityEngine;
using UnityEngine.SceneManagement; 
using UnityEngine.VR.WSA.Persistence;
using UnityEngine.VR.WSA;
using System.Collections;
using System.Collections.Generic;

public class LevelManagerArz : MonoBehaviour {



    public void GotoGameNoSetm() {GameSettings.Instance.GameMode = ARZGameModes.GameNoStem; LoadScene("Game"); }
    public void GoToGameRight() { GameSettings.Instance.GameMode = ARZGameModes.GameRight; LoadScene("Game"); }
    public void GoToGameLeft() { GameSettings.Instance.GameMode = ARZGameModes.GameLeft; LoadScene("Game"); }

    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}
