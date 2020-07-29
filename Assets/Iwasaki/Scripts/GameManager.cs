using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : SingletonMonoBehaviour<GameManager>
{
    public PlayerInventoryContainer playerContainer;
    public InventoryContainerBase largePlateContainer;

    // 現在のシーン
    private Scene _currentScene;
    private Scene _tmpScene;

    private float noiseValue;

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
        playerContainer.Container.Clear();
        largePlateContainer.Container.Clear();
        TimeCounter.currentTime = TimeCounter.countup;
        NoiseMator = 0;
    }
}
