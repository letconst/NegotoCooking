﻿using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : SingletonMonoBehaviour<GameManager>
{
    [SerializeField]
    private StatisticsManager statisticsManager;

    [SerializeField]
    private DogToyData dogToyData;

    // 現在のシーン
    private Scene _currentScene;
    private Scene _tmpScene;

    private float noiseValue;
    private int   bubblePoint;
    private int   fireChange;

    [HideInInspector]
    public bool alertBool;

    private int  bakePoint;
    private int  failCount;
    public  bool cutOperationBool  { get; set; }
    public  bool bakeOperationBool { get; set; }
    public  bool boilOperationBool { get; set; }
    public  bool menuBool          { get; set; }

    public bool IsReachedResult       { get; private set; }
    public bool IsReachedTitle        { get; private set; }
    public bool IsReachedNavOfNegoto  { get; set; }
    public bool IsReachedNavOfStairs  { get; set; }
    public bool IsReachedNavOfKitchen { get; set; }
    public bool doOnce                { get; set; }

    public float NoiseMator { set { noiseValue = Mathf.Clamp(value, 0, 1); } get => noiseValue; }

    public int BubblePoint { set { bubblePoint = Mathf.Clamp(value, 0, 100); } get => bubblePoint; }

    public int BakePoint { set { bakePoint = Mathf.Clamp(value, 0, 100); } get => bakePoint; }

    public int FireChange { set { fireChange = Mathf.Clamp(value, 0, 2); } get => fireChange; }

    //条件を満たせなかった回数
    public int FailCount { set { failCount = Mathf.Clamp(value, 0, 3); } get => failCount; }

    public Vector3           PlayerPos         { set; get; }
    public Vector3           PlayerRotate      { set; get; }
    public StatisticsManager StatisticsManager => statisticsManager;

    public DogToyData DogToyData => dogToyData;

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

        if (_currentScene.name != _tmpScene.name &&
            _currentScene.name == "TitleScenes")
        {
            IsReachedTitle = true;
        }

        // タイトル到達後、現在のシーンがタイトルなら値をリセットしてあげる
        if (IsReachedTitle &&
            _currentScene.name == "TitleScenes")
        {
            ResetAllValues();
        }

        // タイトル到達後にメインシーンに遷移した際は寝言を初期化する
        if (IsReachedTitle &&
            _currentScene.name == "GameScenes")
        {
            IsReachedTitle  = false;
            IsReachedResult = false;
            NegotoManager.Instance.Init();
        }

        _tmpScene = (_currentScene.isLoaded)
                        ? _currentScene
                        : _tmpScene;
    }

    /// <summary>
    /// すべての値をリセットする
    /// </summary>
    public void ResetAllValues()
    {
        InventoryManager.Instance.ResetValues();
        TimeCounter.ResetValues();
        NegotoManager.Instance.NegotoData.Entries.Clear();
        dogToyData.Entries.Clear();
        SoundManager.Instance.FadeOutBgm(.2f);
        NoiseMator                     = 0;
        FailCount                      = 0;
        statisticsManager.throwInCount = 0;
        PlayerPos                      = Vector3.zero;
        PlayerRotate                   = Vector3.zero;
        cutOperationBool               = false;
        bakeOperationBool              = false;
        boilOperationBool              = false;
        IsReachedNavOfNegoto           = false;
        IsReachedNavOfStairs           = false;
        IsReachedNavOfKitchen          = false;
        doOnce                         = false;

        // 調理フラグ
        CookingManager.Instance.ResetValues();
        FireControl.ResetValues();
        FireControl_boil.ResetValues();
        BakeController.ResetValues();
        BoilController.ResetValues();
        CutController.ResetValues();
        CutGauge.ResetValues();
    }

    private void OnApplicationQuit()
    {
        ResetAllValues();
    }
}
