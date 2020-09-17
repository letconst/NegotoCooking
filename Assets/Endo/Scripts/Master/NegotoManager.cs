using UnityEngine;

public class NegotoManager : SingletonMonoBehaviour<NegotoManager>
{
    [SerializeField, Tooltip("プレイヤーオブジェクト")]
    private GameObject playerObj;

    [SerializeField, Tooltip("寝言が表示される距離")]
    private float distanceDisplayed;

    [SerializeField, Tooltip("寝言の初期表示数")]
    private int defaultDisplayCount;

    [SerializeField, Tooltip("現在の寝言の表示数")]
    private int _curDisplayCount;

    private int CurDisplayCount { get => _curDisplayCount; set => _curDisplayCount = Mathf.Clamp(value, 0, 3); }

    public bool IsPlayerNeared { get; private set; }

    private void Awake()
    {
        if (this != Instance)
        {
            Destroy(gameObject);

            return;
        }

        DontDestroyOnLoad(gameObject);
    }

    // Start is called before the first frame update
    private void Start()
    {
        // 現在の寝言表示数を初期表示数に設定
        CurDisplayCount = defaultDisplayCount;
    }

    // Update is called once per frame
    private void Update()
    {
        // 寝言表示距離内にプレイヤーがいるか否かを判定
        IsPlayerNeared =
            Vector3.Distance(MasterController.Instance.transform.position, playerObj.transform.position) <
            distanceDisplayed;
    }
}
