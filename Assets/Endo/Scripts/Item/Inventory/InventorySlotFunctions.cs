using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class InventorySlotFunctions : MonoBehaviour
{
    private GameObject                         _playerInvObj;
    private GameObject                         _refInvObj;
    private CanvasGroup                        _playerCanvasGroup;
    private CanvasGroup                        _refCanvasGroup;
    private InventoryManager                   _invManager;
    private PlayerInventoryContainer           _playerContainer;
    private RefrigeratorInventoryContainerBase _nearRefContainer;
    private InventoryRenderer                  _playerInvRenderer;
    private InventoryRenderer                  _refInvRenderer;

    // Start is called before the first frame update
    private void Start()
    {
        _playerInvObj      = GameObject.FindGameObjectWithTag("PlayerInventory");
        _refInvObj         = GameObject.FindGameObjectWithTag("RefrigeratorInventory");
        _playerCanvasGroup = _playerInvObj.GetComponent<CanvasGroup>();
        _invManager        = InventoryManager.Instance;
        _playerContainer   = _invManager.PlayerContainer;
        _playerInvRenderer = _playerInvObj.GetComponent<InventoryRenderer>();

        // 冷蔵庫が存在しないシーンならCanvasGroupを取得しない
        if (_refInvObj == null) return;

        _refCanvasGroup = _refInvObj.GetComponent<CanvasGroup>();
    }

    // Update is called once per frame
    private void Update()
    {
        // メインシーンのときに、冷蔵庫インベントリオブジェが未取得なら取得を試みる
        if (SceneManager.GetActiveScene().name == "GameScenes")
        {
            if (_refInvObj == null)
            {
                _refInvObj = GameObject.FindGameObjectWithTag("RefrigeratorInventory");
            }
            else if (_refInvRenderer == null)
            {
                _refInvRenderer = _refInvObj.GetComponent<InventoryRenderer>();
            }
        }
    }

    /// <summary>
    /// プレイヤーインベントリスロットをクリックした際の動作
    /// </summary>
    public void OnClickForPlayer()
    {
        // 交換モードなら
        if (!_invManager.IsSwapMode) return;

        _nearRefContainer = RefrigeratorManager.Instance.NearRefrigeratorContainer;

        var selfIndex    = _playerInvRenderer.LastSelectedIndex;
        var refLastIndex = _refInvRenderer.LastSelectedIndex;

        // 交換先にアイテムを渡す
        _nearRefContainer.UpdateItem(_refInvRenderer.LastSelectedIndex,
                                     _playerContainer.GetItem(selfIndex),
                                     _playerContainer.GetStates(selfIndex));

        // 選択スロットに交換先のアイテムを配置
        _playerContainer.UpdateItem(selfIndex,
                                    _invManager.ItemToSwapFromRef,
                                    _invManager.ItemStatesToSwap);

        // フォーカスを冷蔵庫に戻す
        _playerCanvasGroup.interactable = false;
        _refCanvasGroup.interactable    = true;
        _refInvRenderer.SelectSlot();

        // 冷蔵庫スロットのハイライトを解除
        _refInvRenderer.GetSlotAt(refLastIndex).GetComponent<Button>().enabled = true;
        _refInvRenderer.UnhighlightSlotAt(_refInvRenderer.LastSelectedIndex);

        // キャッシュアイテムクリア
        _invManager.ItemToSwapFromRef = null;
        _invManager.ItemStatesToSwap  = null;
    }

    /// <summary>
    /// 冷蔵庫インベントリスロットをクリックした際の動作
    /// </summary>
    public void OnClickForRefrigerator()
    {
        _nearRefContainer = RefrigeratorManager.Instance.NearRefrigeratorContainer;
        var selfIndex   = _refInvRenderer.LastSelectedIndex;
        var selfButton  = GetComponent<Button>();
        var nearRefSlot = _nearRefContainer.Container[selfIndex];
        var selfItem    = nearRefSlot.Item;
        var selfStates  = nearRefSlot.States;

        // スロットにアイテムがなければ弾く
        if (selfItem == null) return;

        // プレイヤーの空きスロットにアイテム配置
        if (!_playerContainer.IsFull)
        {
            _playerContainer.AddItem(selfItem, selfStates);

            return;
        }

        // 冷蔵庫スロットは空にする
        // 無限獲得可能への仕様変更につきCO
        // _nearRefContainer.RemoveItem(selfIndex);

        // プレイヤーとアイテム交換
        // 交換アイテムをキャッシュ
        _invManager.ItemToSwapFromRef = selfItem;
        _invManager.ItemStatesToSwap  = selfStates;
        // 交換モード発動
        _invManager.IsSwapMode = true;
        // 冷蔵庫スロット無効化（交換対象はハイライト）
        _refCanvasGroup.interactable = false;
        selfButton.interactable      = true;
        selfButton.enabled           = false;
        _refInvRenderer.HighlightSlotAt(selfIndex);
        // プレイヤースロット有効化
        _playerCanvasGroup.interactable = true;
        _playerInvRenderer.SelectSlot();

        // ここから先はOnClickForPlayer()
    }

    /// <summary>
    /// 調理時のプレイヤーインベントリスロットをクリックした際の動作
    /// </summary>
    public void OnClickForCooking()
    {
        //食料prefabの親
        var foodParent = GameObject.FindGameObjectWithTag("FoodParent");

        var foodPosition = GameObject.FindGameObjectWithTag("FoodPosition");
        var foodTrf      = foodPosition.transform;

        var selfIndex = _playerInvRenderer.LastSelectedIndex;

        var playerContainer = InventoryManager.Instance.PlayerContainer;
        var selfSlot        = playerContainer.Container[selfIndex];

        // 現在のシーン名
        var curSceneName = SceneManager.GetActiveScene().name;

        // 投入できない食材の状態
        var disallowState = FoodState.None;

        // 操作説明表示中か否か
        var isOperationDisplayed = false;

        switch (curSceneName)
        {
            case "BakeScenes":
                disallowState        = FoodState.Cooked;
                isOperationDisplayed = GameManager.Instance.bakeOperationBool;

                break;

            case "BoilScenes":
                disallowState        = FoodState.Boil;
                isOperationDisplayed = GameManager.Instance.boilOperationBool;

                break;

            case "CutScenes":
                disallowState        = FoodState.Cut;
                isOperationDisplayed = GameManager.Instance.cutOperationBool;

                break;

            default:
                Debug.LogWarning($"シーン「{curSceneName}」の投入不可状態の指定がありません");

                break;
        }

        // 条件に満たない食材は投入できない
        if (!FireControl.clickBool                  ||                      // 焼き調理中である
            !FireControl_boil.clickBool             ||                      // 煮込み調理中である
            !CutGauge.clickBool                     ||                      // 切り調理中である
            isOperationDisplayed                    ||                      // 操作説明表示中である
            selfSlot.Item == null                   ||                      // 選択スロットに食材がない
            selfSlot.States.Contains(disallowState) ||                      // 選択スロットの食材が投入可能な状態でない
            selfSlot.Item.KindOfItem1 == Item.KindOfItem.Seasoning) return; // 選択スロットが調味料である

        switch (curSceneName)
        {
            case "CutScenes":
                CutGauge.clickBool    = false;
                CutGauge.cantBackBool = false;

                break;

            case "BoilScenes":
                GameManager.Instance.BubblePoint = 0;
                FireControl_boil.clickBool       = false;

                break;

            case "BakeScenes":
                FireControl.clickBool = false;

                break;
        }

        // 食材を表示
        var foodRotation = foodTrf.rotation;
        var foodChild = Instantiate(selfSlot.Item.FoodObj,
                                    foodTrf.position,
                                    new Quaternion(foodRotation.x,
                                                   foodRotation.y,
                                                   foodRotation.z,
                                                   foodRotation.w));

        foodChild.transform.parent = foodParent.transform;

        switch (curSceneName)
        {
            case "BakeScenes":
                BakeController.FoodSlotBeingBaked = selfSlot.DeepCopy();

                break;

            case "BoilScenes":
                BoilController.FoodSlotBeingBoiled = selfSlot.DeepCopy();

                break;

            case "CutScenes":
                CutController.FoodSlotBeingCut = selfSlot.DeepCopy();

                break;

            default:
                Debug.LogWarning($"シーン「{curSceneName}」の調理中食材投入先の指定がありません");

                break;
        }

        // プレイヤーコンテナから投入アイテムを削除
        playerContainer.RemoveItem(selfIndex);

        // 投入元のスロットインデックスを記憶
        InventoryManager.Instance.PuttedSlotIndex = selfIndex;
    }
}
