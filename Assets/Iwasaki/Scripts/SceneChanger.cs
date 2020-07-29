﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : SingletonMonoBehaviour<SceneChanger>
{
    [SerializeField]
    private float waittime = 1;

    public enum SceneName
    {
        TitleScenes = 0,
        CookingScenes,
        GameOverScenes,
        GameScenes,

    }
    public void Awake()
    {
        if (this != Instance)
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);
    }
    private void Start()
    {
            
    }
    //連打対策
    private bool doOnceSceneChange = true;

    public void SceneLoad(SceneName sceneName)
    {
        if (doOnceSceneChange == false) return;

        if (doOnceSceneChange)
        {
            StartCoroutine(LoadSceneCor(sceneName));
        }
    }

    private IEnumerator LoadSceneCor(SceneName sceneName)
    {
        doOnceSceneChange = false;
        var async = SceneManager.LoadSceneAsync(sceneName.ToString());
        async.allowSceneActivation = false;
        yield return new WaitForSeconds(waittime);
        doOnceSceneChange = true;
        async.allowSceneActivation = true;
    }
}