using System.Collections;
using UnityEngine;

public class GarbageCanController : MonoBehaviour
{
    private PlayerInventoryContainer _playerContainer;
    private InventoryRenderer        _playerInvRenderer;
    private ChoicePopup              _choicePopup;

    // プレイヤーが近くにいるか否か
    private bool _isNear;

    // 確認ウィンドウが表示中か否か
    private bool _isShowingWindow;

    // 確認ウィンドウのコルーチン
    private IEnumerator _windowCor;

    // Start is called before the first frame update
    private void Start()
    {
        _playerContainer   = InventoryManager.Instance.PlayerContainer;
        _playerInvRenderer = GameObject.FindGameObjectWithTag("PlayerInventory").GetComponent<InventoryRenderer>();
        _choicePopup       = GameObject.FindGameObjectWithTag("SelectWindow").GetComponent<ChoicePopup>();
    }

    // Update is called once per frame
    private void Update()
    {
        // インタラクトで選択アイテムを捨てる
        if (_isNear                          &&                                     // インタラクト範囲内にいる
            !_isShowingWindow                &&                                     // 確認ウィンドウ表示中ではない
            Input.GetButtonDown("Interact")  &&                                     // インタラクトボタン押下
            !PushPause.Instance.IsNowPausing &&                                     // ポーズ中ではない
            _playerContainer.GetItem(_playerInvRenderer.LastSelectedIndex) != null) // 食材を選択している
        {
            StartCoroutine(nameof(InputHandler));
        }
    }

    /// <summary>
    /// ボタン入力を処理する
    /// </summary>
    /// <returns></returns>
    private IEnumerator InputHandler()
    {
        var selectedFoodName = _playerContainer.GetItem(_playerInvRenderer.LastSelectedIndex).ItemName;
        _windowCor       = _choicePopup.ShowWindow($"{selectedFoodName}を捨てますか？", SE.ThrowOutFood);
        _isShowingWindow = true;

        // ボタン入力を待機
        yield return _windowCor;

        // はい選択だったら食材を捨てる
        if (_windowCor.Current != null &&
            (bool) _windowCor.Current)
        {
            // プレイヤーインベントリから選択食材を削除
            _playerContainer.RemoveItem(_playerInvRenderer.LastSelectedIndex);

            // 捨てた回数をカウント
            GameManager.Instance.StatisticsManager.throwInCount++;
        }

        _choicePopup.HideWindow();
        _isShowingWindow = false;
        _windowCor       = null;
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
