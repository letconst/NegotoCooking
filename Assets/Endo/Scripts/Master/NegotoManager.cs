using System.Collections.Generic;
using System.Linq;
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
    private int curDisplayCount;

    // 寝言の全表示位置
    private IEnumerable<GameObject> _negotos;

    private RecipeDatabase _recipeDB;

    public List<RequireFoods> Recipe { get; private set; }

    private int CurDisplayCount { get => curDisplayCount; set => curDisplayCount = Mathf.Clamp(value, 0, 3); }

    /// <summary>
    /// プレイヤーが寝言表示範囲にいるか否か
    /// </summary>
    public bool IsPlayerNeared { get; private set; }

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

        // 初期表示数の寝言のみを表示
        foreach (var negoto in _negotos.Select((v, i) => new
        {
            v,
            i
        }))
        {
            if (negoto.i <= defaultDisplayCount - 1) continue;

            negoto.v.SetActive(false);
        }
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
