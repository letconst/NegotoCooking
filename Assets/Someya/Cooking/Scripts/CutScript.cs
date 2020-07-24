using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CutScript : MonoBehaviour
{
    public void OnClickButton()
    {
        SceneManager.UnloadSceneAsync("CutScenes");
        Player.Instance._isTouch = false;
    }
}
