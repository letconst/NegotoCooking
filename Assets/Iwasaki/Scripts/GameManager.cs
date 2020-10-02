using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : SingletonMonoBehaviour<GameManager>
{
    [SerializeField]
    private StatisticsManager statisticsManager;

    // 現在のシーン
    private Scene _currentScene;
    private Scene _tmpScene;

    private float noiseValue;
    private int bubblePoint;
    private int fireChange;
    [HideInInspector]
    public bool alertBool;
    private int bakePoint;
    private int failCount;

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

    public int BubblePoint
    {
        set
        {
            bubblePoint = Mathf.Clamp(value, 0, 100);
        }
        get
        {
            return bubblePoint;
        }
    }

    public int BakePoint
    {
        set
        {
            bakePoint = Mathf.Clamp(value, 0, 100);
        }
        get
        {
            return bakePoint;
        }
    }

    public int FireChange
    {
        set
        {
            fireChange = Mathf.Clamp(value, 0, 2);
        }
        get
        {
            return fireChange;
        }
    }

    //条件を満たせなかった回数
    public int FailCount
    {
        set
        {
            failCount = Mathf.Clamp(value, 0, 3);
        }
        get
        {
            return failCount;
        }
    }

    public Vector3           PlayerPos         { set; get; }
    public Vector3           PlayerRotate      { set; get; }
    public StatisticsManager StatisticsManager => statisticsManager;

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

        // シーン遷移後、現在のシーンがタイトルなら値をリセットしてあげる
        if (_currentScene.name != _tmpScene.name &&
            (_currentScene.name == "TitleScenes"))
        {
            ResetAllValues();
        }

        if (_currentScene.name != _tmpScene.name &&
            _currentScene.name == "GameScenes")
        {
            NegotoManager.Instance.Init();
        }

        _tmpScene = SceneManager.GetActiveScene();
    }

    /// <summary>
    /// すべての値をリセットする
    /// </summary>
    public void ResetAllValues()
    {
        InventoryManager.Instance.PlayerContainer.Container.Clear();
        InventoryManager.Instance.PlayerContainer.DogFoodCount =
            InventoryManager.Instance.PlayerContainer.MaxDogFoodCount;
        InventoryManager.Instance.RefContainers.RefInvContainers.Clear();
        InventoryManager.Instance.LargePlateContainer.Container.Clear();
        TimeCounter.CurrentTime = TimeCounter.CountUp;
        NegotoManager.Instance.NegotoData.Entries.Clear();
        NoiseMator   = 0;
        PlayerPos    = Vector3.zero;
        PlayerRotate = Vector3.zero;
    }
    private void OnApplicationQuit()
    {
        statisticsManager.throwInCount = 0;
    }
}
