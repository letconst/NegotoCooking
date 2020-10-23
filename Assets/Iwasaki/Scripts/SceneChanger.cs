using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class SceneChanger : SingletonMonoBehaviour<SceneChanger>
{
    [SerializeField]
    private float waittime = 1;

    [SerializeField]
    private GameObject fadeObj;

    private GameObject  _fadePanelObj;
    private Canvas      _fadeCanvas;
    private CanvasGroup _fadeCanvasGroup;
    private bool        _isFaded;

    public enum SceneName
    {
        TitleScenes = 0,
        BakeScenes,
        GameOverScenes,
        GameScenes,
        GameClear,
        BoilScenes,
        CutScenes,
        returngame,
        Result
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

    //連打対策
    private bool doOnceSceneChange = true;

    public void SceneLoad(SceneName sceneName, bool isFade = false)
    {
        if (doOnceSceneChange == false) return;

        // リザルト遷移時、タイムカウンターを止める
        if (sceneName == SceneName.Result) TimeCounter.IsStopped = true;

        if (doOnceSceneChange)
        {
            StartCoroutine(LoadSceneCor(sceneName, isFade));
        }
    }

    private IEnumerator LoadSceneCor(SceneName sceneName, bool isFade)
    {
        // フェード指定があったらアニメーションする
        if (isFade)
        {
            yield return StartCoroutine(FadeIn());
        }

        doOnceSceneChange = false;
        var async = SceneManager.LoadSceneAsync(sceneName.ToString());
        async.allowSceneActivation = false;

        yield return new WaitForSeconds(waittime);

        doOnceSceneChange          = true;
        async.allowSceneActivation = true;

        if (isFade) StartCoroutine(FadeOut());
    }

    /// <summary>
    /// フェードに必要なオブジェクトを生成する
    /// </summary>
    private void InitFadeObj()
    {
        // Canvas生成
        _fadePanelObj            = new GameObject("FadePanel");
        _fadeCanvas              = _fadePanelObj.AddComponent<Canvas>();
        _fadeCanvas.renderMode   = RenderMode.ScreenSpaceOverlay;
        _fadeCanvas.sortingOrder = 99;

        // Fadeオブジェクト生成
        var fadeBody = Instantiate(fadeObj, _fadeCanvas.transform);
        _fadeCanvasGroup = fadeBody.GetComponent<CanvasGroup>();
    }

    /// <summary>
    /// フェードインを実行する
    /// </summary>
    /// <returns></returns>
    private IEnumerator FadeIn()
    {
        if (_fadeCanvasGroup == null) InitFadeObj();

        _fadeCanvasGroup.alpha = 0;

        while (_fadeCanvasGroup.alpha < 1)
        {
            _fadeCanvasGroup.alpha += Time.deltaTime / waittime;

            yield return null;
        }
    }

    /// <summary>
    /// フェードアウトを実行する
    /// </summary>
    /// <returns></returns>
    private IEnumerator FadeOut()
    {
        // シーンが遷移してフェードオブジェクトが破棄されるのを待機
        yield return new WaitUntil(() => _fadeCanvasGroup == null);

        // フェードオブジェクト再生成
        InitFadeObj();

        _fadeCanvasGroup.alpha = 1;

        while (_fadeCanvasGroup.alpha > 0)
        {
            _fadeCanvasGroup.alpha -= Time.deltaTime / waittime;

            yield return null;
        }
    }
}
