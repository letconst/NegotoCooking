using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : SingletonMonoBehaviour<GameManager>
{
    // 現在のシーン
    private Scene _currentScene;
    private Scene _tmpScene;

    private float noiseValue;
    private Vector3 playerPos;
    private Vector3 playerRotate;

    public float NoiseMator
    {
        set
        {
            noiseValue = Mathf.Clamp(value, 0, 1);
        }
        get
        {
            return noiseValue;
        }
    }

    public Vector3 PlayerPos
    {
        set
        {
            playerPos = value;
        }
        get
        {
            return playerPos;
        }
    }

    public Vector3 PlayerRotate
    {
        set
        {
            playerRotate = value;
        }
        get
        {
            return playerRotate;
        }
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

    private void Update()
    {
        _currentScene = SceneManager.GetActiveScene();

        // シーン遷移後、現在のシーンがゲームクリアまたはゲームオーバーなら値をリセットしてあげる
        if (_currentScene.name != _tmpScene.name &&
            (_currentScene.name == "GameClear" ||
             _currentScene.name == "GameOverScenes"))
        {
            ResetAllValues();
        }

        _tmpScene = SceneManager.GetActiveScene();
    }

    /// <summary>
    /// すべての値をリセットする
    /// </summary>
    public void ResetAllValues()
    {
        TmpInventoryManager.Instance.PlayerContainer.Container.Clear();
        TmpInventoryManager.Instance.RefContainers.RefInvContainers.Clear();
        TmpInventoryManager.Instance.LargePlateContainer.Container.Clear();
        TimeCounter.currentTime = TimeCounter.countup;
        NoiseMator = 0;
    }
}
