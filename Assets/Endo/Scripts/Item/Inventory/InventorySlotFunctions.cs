using UnityEngine;
using UnityEngine.SceneManagement;

public class InventorySlotFunctions : MonoBehaviour
{
    private GameObject                         _refInvObj;
    private InventoryManager                   _invManager;
    private PlayerInventoryContainer           _playerContainer;
    private RefrigeratorInventoryContainerBase _nearRefContainer;
    private InventoryRenderer                  _playerInvRenderer;
    private InventoryRenderer                  _refInvRenderer;

    // Start is called before the first frame update
    private void Start()
    {
        _refInvObj         = GameObject.FindGameObjectWithTag("RefrigeratorInventory");
        _invManager        = InventoryManager.Instance;
        _playerContainer   = _invManager.PlayerContainer;
        _playerInvRenderer = GameObject.FindGameObjectWithTag("PlayerInventory").GetComponent<InventoryRenderer>();
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
    /// 同じ階層のオブジェクトから自身を探し、見つかったインデックスを返す
    /// 返り値が-1なら該当なし
    /// </summary>
    /// <returns>インデックス</returns>
    public int GetSelfIndex()
    {
        var result = -1;

        for (var i = 0; i < transform.parent.childCount; i++)
        {
            if (transform.parent.GetChild(i).gameObject == gameObject)
            {
                result = i;
            }
        }

        return result;
    }

    /// <summary>
    /// プレイヤーインベントリスロットをクリックした際の動作
    /// </summary>
    public void OnClickForPlayer()
    {
        _nearRefContainer = RefrigeratorManager.Instance.NearRefrigeratorContainer;

        // 交換モードなら
        if (!_invManager.IsSwapMode) return;

        var selfIndex = GetSelfIndex();

        // 交換先にアイテムを渡す
        _nearRefContainer.UpdateItem(_refInvRenderer.LastSelectedIndex,
                                     _playerContainer.GetItem(selfIndex),
                                     _playerContainer.GetState(selfIndex));

        // 選択スロットに交換先のアイテムを配置
        _playerContainer.UpdateItem(selfIndex,
                                    _invManager.ItemToSwapFromRef,
                                    _invManager.ItemStateToSwap);

        // フォーカスを冷蔵庫に戻す
        _playerInvRenderer.DisableAllSlot();
        _refInvRenderer.EnableAllSlot();
        _refInvRenderer.SelectSlot();

        // キャッシュアイテムクリア
        _invManager.ItemToSwapFromRef = null;
        _invManager.ItemStateToSwap   = FoodState.None;
    }

    /// <summary>
    /// 冷蔵庫インベントリスロットをクリックした際の動作
    /// </summary>
    public void OnClickForRefrigerator()
    {
        _nearRefContainer             = RefrigeratorManager.Instance.NearRefrigeratorContainer;
        var selfIndex   = GetSelfIndex();
        var nearRefSlot = _nearRefContainer.Container[selfIndex];
        var selfItem    = nearRefSlot.Item;
        var selfState   = nearRefSlot.State;

        // スロットにアイテムがなければ弾く
        if (selfItem == null) return;

        for (int i = 0; i < _playerContainer.Container.Count; i++)
        {
            // プレイヤーインベントリに空きがあったら
            if (_playerContainer.Container[i].Item != null) continue;

            // そのスロットにアイテムを配置
            _playerContainer.UpdateItem(i, selfItem, selfState);

            // 冷蔵庫スロットは空にする
            // 無限獲得可能への仕様変更につきCO
            // _nearRefContainer.RemoveItem(selfIndex);

            return;
        }

        // プレイヤーとアイテム交換
        // 交換アイテムをキャッシュ
        _invManager.ItemToSwapFromRef = selfItem;
        _invManager.ItemStateToSwap   = selfState;
        // 交換モード発動
        _invManager.IsSwapMode = true;
        // プレイヤースロットにフォーカス
        _refInvRenderer.DisableAllSlot();
        _playerInvRenderer.EnableAllSlot();
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

        var selfIndex = GetSelfIndex();

        var playerContainer = InventoryManager.Instance.PlayerContainer;
        var selfSlot        = playerContainer.Container[selfIndex];

        // 現在のシーン名
        var curSceneName = SceneManager.GetActiveScene().name;

        // 投入できない食材の状態
        var disallowState = FoodState.None;

        switch (curSceneName)
        {
            case "BakeScenes":
                disallowState = FoodState.Cooked;
                break;

            case "BoilScenes":
                disallowState = FoodState.Boil;
                break;

            case "CutScenes":
                disallowState = FoodState.Cut;
                break;

            default:
                Debug.LogWarning($"シーン「{curSceneName}」の投入不可状態の指定がありません");
                break;
        }

        // 条件に満たない食材は投入できない
        if (!FireControl.clickBool          ||
            !FireControl_boil.clickBool     ||
            !CutGauge.clickBool             ||
            selfSlot.Item == null           ||
            selfSlot.State == disallowState ||
            selfSlot.Item.KindOfItem1 == Item.KindOfItem.Seasoning) return;
        
        if (curSceneName == "CutScenes")
        {
            CutGauge.clickBool = false;
            CutGauge.cantBackBool = false;
        }
        else if(curSceneName == "BoilScenes")
        {
            GameManager.Instance.BubblePoint = 0;
            FireControl_boil.clickBool = false;
        }
        else if (curSceneName == "BakeScenes")
        {
            FireControl.clickBool = false;
        }

            // 食材を表示
            var foodRotation = foodTrf.rotation;
        var foodChild    = Instantiate(selfSlot.Item.FoodObj,
                                       foodTrf.position,
                                       new Quaternion(foodRotation.x,
                                                      foodRotation.y,
                                                      foodRotation.z,
                                                      foodRotation.w));
        foodChild.transform.parent = foodParent.transform;

        switch (curSceneName)
        {
            case "BakeScenes":
                BakeController.FoodBeingBaked = selfSlot.Item;
                break;

            case "BoilScenes":
                BoilController.FoodBeingBoiled = selfSlot.Item;
                break;

            case "CutScenes":
                CutController.FoodBeingCut = selfSlot.Item;
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
