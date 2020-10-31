using System.Collections;
using UnityEngine;

public class LargePlateController : MonoBehaviour
{
    // インベントリアセット
    [SerializeField]
    private InventoryContainerBase selfContainer;

    private GameObject               _soup;
    private GameObject               _playerInvObj;
    private PlayerInventoryContainer _playerInvContainer;
    private InventoryRenderer        _playerInvRenderer;
    private ChoicePopup              _choicePopup;

    // 近くにいるか否か
    private bool _isNear;

    // 確認ウィンドウが表示中か否か
    private bool _isShowingWindow;

    // 確認ウィンドウのコルーチン
    private IEnumerator _windowCor;

    // Start is called before the first frame update
    private void Start()
    {
        _soup               = transform.Find("Soup").gameObject;
        _playerInvObj       = GameObject.FindGameObjectWithTag("PlayerInventory");
        _playerInvContainer = InventoryManager.Instance.PlayerContainer;
        _playerInvRenderer  = _playerInvObj.GetComponent<InventoryRenderer>();
        _choicePopup        = GameObject.FindGameObjectWithTag("SelectWindow").GetComponent<ChoicePopup>();
        _soup.SetActive(false);
    }

    // Update is called once per frame
    private void Update()
    {
        // インタラクトで食材を投入
        if (_isNear           &&             // インタラクト範囲内にいる
            !_isShowingWindow &&             // 確認ウィンドウ表示中ではない
            Input.GetButtonDown("Interact")) // インタラクトボタン押下
        {
            StartCoroutine(nameof(InputHandler));
        }

        // コンテナにアイテムが1つでも入ったらスープを表示
        if (selfContainer.Container.Count > 0) _soup.SetActive(true);
    }

    /// <summary>
    /// ボタン入力を処理する
    /// </summary>
    /// <returns></returns>
    private IEnumerator InputHandler()
    {
        var selectedFood      = _playerInvContainer.GetItem(_playerInvRenderer.LastSelectedIndex);
        var selectedFoodState = _playerInvContainer.GetStates(_playerInvRenderer.LastSelectedIndex);

        // 調味料か調理済みの食材のみ受け付ける
        if (selectedFood == null ||                                  // 食材を持っていない
            selectedFood.KindOfItem1 != Item.KindOfItem.Seasoning && // 食材が調味料ではなく、
            (selectedFoodState.Contains(FoodState.None) ||           // 状態がNoneまたはRawである
             selectedFoodState.Contains(FoodState.Raw))) yield break;

        _windowCor       = _choicePopup.ShowWindow($"{selectedFood.ItemName}を入れますか？", SE.PutFood);
        _isShowingWindow = true;

        // ボタン入力を待機
        yield return _windowCor;

        // ボタン入力結果がはいだったら大皿に食材を入れる
        if (_windowCor.Current != null &&
            (bool) _windowCor.Current)
        {
            var targetFoodIndex = _playerInvRenderer.LastSelectedIndex;

            // 大皿に現在選択しているアイテムをぶち込む
            selfContainer.AddSlot(_playerInvContainer.GetItem(targetFoodIndex),
                                  selectedFoodState);

            // 表示中の寝言の達成をチェック
            NegotoManager.Instance.CheckNegotoAchieved(_playerInvContainer.GetItem(targetFoodIndex));

            // プレイヤーのアイテムを削除
            _playerInvContainer.RemoveItem(targetFoodIndex);
        }

        _choicePopup.HideWindow();
        _isShowingWindow = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) _isNear = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        _choicePopup.HideWindow();
        _isNear          = false;
        _isShowingWindow = false;

        // 確認ウィンドウ表示中だったら入力待機を解除して非表示に
        if (_windowCor == null) return;

        StopCoroutine(nameof(InputHandler));
        _windowCor = null;
    }
}
