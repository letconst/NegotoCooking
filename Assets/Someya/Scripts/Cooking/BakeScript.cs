﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BakeScript : MonoBehaviour
{
    public void OnClickButton()
    {
        SceneManager.UnloadSceneAsync("BakeScenes");
        Player.Instance._isTouch = false;
    }
}