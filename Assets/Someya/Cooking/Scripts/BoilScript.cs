using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BoilScript : MonoBehaviour
{
    public void OnClickButton()
    {
        SceneManager.UnloadSceneAsync("BoilScenes");
        Player.Instance._isTouch = false;
    }
}
