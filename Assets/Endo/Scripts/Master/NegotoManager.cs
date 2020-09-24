using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NegotoManager : SingletonMonoBehaviour<NegotoManager>
{
    [SerializeField, Tooltip("寝言管理用のScriptableObject")]
    private NegotoData negotoData;

    [SerializeField, Tooltip("寝言が表示される距離")]
    private float distanceDisplayed;

    [SerializeField, Tooltip("寝言の初期表示数")]
    private int defaultDisplayCount;

    [SerializeField, Tooltip("寝言の最大表示数")]
    private int maxDisplayCount;

    [SerializeField, Tooltip("現在の寝言の表示数")]
    private int curDisplayCount;

    private GameObject _playerObj;

    // 寝言の全表示位置
    private IEnumerable<GameObject> _negotos;

    private RecipeDatabase _recipeDB;

    // 現在のシーン名
    private string _curSceneName;

    // 1フレーム前のシーン名
    private string _tmpSceneName;

    public List<RequireFoods> Recipe { get; private set; }

    /// <summary>
    /// 現在の寝言表示数
    /// </summary>
    public int CurDisplayCount
    {
        get => curDisplayCount;
        private set => curDisplayCount = Mathf.Clamp(value, 0, maxDisplayCount);
    }

    /// <summary>
    /// プレイヤーが寝言表示範囲にいるか否か
    /// </summary>
    public bool IsPlayerNeared { get; private set; }

    public NegotoData NegotoData => negotoData;

    private void Awake()
    {
        if (this != Instance)
        {
            Destroy(gameObject);

            return;
        }

        DontDestroyOnLoad(gameObject);

        _recipeDB = InventoryManager.Instance.RecipeDatabase;
        // レシピを複数用意する場合は、ランダム選択に変更する必要あり
        Recipe = new List<RequireFoods>(_recipeDB.GetRecipeByName("エビのスープ").RequireFoods);
    }

    // Start is called before the first frame update
    private void Start()
    {
        // 現在の寝言表示数を初期表示数に設定
        CurDisplayCount = defaultDisplayCount;
        _negotos        = GameObject.FindGameObjectsWithTag("Negoto");

        StartCoroutine(nameof(SetAllNegotoIndex), true);
    }

    // Update is called once per frame
    private void Update()
    {
        _curSceneName = SceneManager.GetActiveScene().name;

        // メインシーンでのみ動作
        if (SceneManager.GetActiveScene().name != "GameScenes")
        {
            _tmpSceneName = _curSceneName;

            return;
        }

        // シーン遷移後、各寝言にインデックスを再設定してあげる
        if (_tmpSceneName != null &&
            _curSceneName != _tmpSceneName)
        {
            StartCoroutine(nameof(SetAllNegotoIndex), false);
        }

        // プレイヤー取得（シーンを跨いだ際にも対応できるようにNullチェックで）
        if (_playerObj == null) _playerObj = GameObject.FindWithTag("Player");

        // 寝言表示距離内にプレイヤーがいるか否かを判定
        IsPlayerNeared =
            Vector3.Distance(MasterController.Instance.transform.position, _playerObj.transform.position) <
            distanceDisplayed;

        _tmpSceneName = _curSceneName;
    }

    private IEnumerator SetAllNegotoIndex(bool isInit)
    {
        // 寝言が破棄されていたら再取得
        if (_negotos.Any(n => n == null))
        {
            _negotos = GameObject.FindGameObjectsWithTag("Negoto");
        }

        // 初期表示数の寝言のみを表示
        foreach (var negoto in _negotos.Select((v, i) => new
        {
            v,
            i
        }))
        {
            var negotoInstance = negoto.v.GetComponent<NegotoController>();
            // 各寝言にインデックスを設定
            negotoInstance.selfIndex = negoto.i;

            // 初期化時、初期表示数内の寝言は非表示処理を行わない
            if (isInit && negoto.i == defaultDisplayCount - 1) continue;

            // 各寝言の初期化を待機
            yield return new WaitWhile(() => negotoInstance.requireFood == null);

            // 初期表示数より上のインデックスの寝言は非表示にする
            if (isInit) negotoInstance.SetActive(false);
        }
    }

    /// <summary>
    /// 寝言の表示数を増加させる
    /// </summary>
    private void IncreaseNegotoDisplay()
    {
        var hasPendingNegoto = false;

        // 表示待機中の寝言があるかチェック
        foreach (var negoto in _negotos)
        {
            var negotoInstance = negoto.GetComponent<NegotoController>();

            if (!negotoInstance.IsActive &&
                negotoInstance.requireFood != null) hasPendingNegoto = true;
        }

        // 寝言が最大表示数に達しているか、さらに残りのレシピ材料および表示待機中の寝言がない場合は弾く
        if (CurDisplayCount == maxDisplayCount ||
            (Recipe.Count == 0 && !hasPendingNegoto))
        {
            return;
        }

        CurDisplayCount++;

        _negotos.First(n => n.GetComponent<NegotoController>().selfIndex == CurDisplayCount - 1)
                .GetComponent<NegotoController>()
                .SetActive(true);
    }

    /// <summary>
    /// 渡されたアイテムが寝言の必須食材と一致した際、その寝言の内容を更新する
    /// </summary>
    /// <param name="food">検索させる食材</param>
    public void CheckNegotoAchieved(Item food)
    {
        // 寝言が破棄されていたら再取得
        if (_negotos.Any(n => n == null))
        {
            _negotos = GameObject.FindGameObjectsWithTag("Negoto");
        }

        // アクティブな寝言から比較食材と一致するものを検索
        foreach (var negoto in _negotos)
        {
            var negotoInstance = negoto.GetComponent<NegotoController>();

            // 寝言が非アクティブか比較食材と一致しない場合は弾く
            if (!negotoInstance.IsActive ||
                negotoInstance.requireFood != food) continue;

            negotoInstance.UpdateContent();
            IncreaseNegotoDisplay();

            break;
        }
    }

    private void OnApplicationQuit()
    {
        negotoData.Entries.Clear();
    }
}
