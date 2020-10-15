using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class InventoryRenderer : MonoBehaviour
{
    [SerializeField, Tooltip("表示させる対象のインベントリコンテナ。冷蔵庫用の場合は未指定で良い")]
    private InventoryContainerBase inventory;

    [SerializeField, Tooltip("表示させるスロットプレハブ")]
    private GameObject slotPrefab;

    [SerializeField, Tooltip("生成する場所の親オブジェクト")]
    private GameObject slotWrapper;

    [SerializeField, Tooltip("冷蔵庫用のインベントリか否か")]
    private bool isForRefrigerator;

    [SerializeField, Tooltip("食材の状態アイコンがグレーアウト時の色（プレイヤーインベントリでのみ使用）")]
    private Color stateGrayOutColor;

    // 最大スロット数
    private int _selfSlotSize;

    // スロットの通常色
    private Color _normalSlotColor = Color.white;

    // 現在選択しているスロット
    private GameObject _currentSelectedObj;

    // 最後に選択していたスロット
    private GameObject _lastSelectedObj;

    // 現在表示されているスロット情報
    private readonly Dictionary<GameObject, InventorySlotBase> _itemsDisplayed =
        new Dictionary<GameObject, InventorySlotBase>();

    // 最後に選択していたスロットのインデックス
    public int LastSelectedIndex { get; private set; }

    // Start is called before the first frame update
    private void Start()
    {
        _selfSlotSize = (inventory != null) ? inventory.SlotSize :
                        (isForRefrigerator) ? RefrigeratorManager.Instance.slotSize : 0;

        InitRender();
    }

    // Update is called once per frame
    private void Update()
    {
        UpdateRender();
        UpdatePlayerInventoryRender();
        UpdateRefrigeratorInventoryRender();
        UpdateLastSelectedIndex();
        UpdateDogToyCountRender();
    }

    /// <summary>
    /// インベントリ表示の初期化
    /// </summary>
    private void InitRender()
    {
        // 例外および冷蔵庫かの判定
        if (inventory == null)
        {
            if (!isForRefrigerator)
            {
                Debug.LogError("表示対象のインベントリコンテナが未指定です");

                return;
            }

            // 冷蔵庫用の処理
            for (var i = 0; i < RefrigeratorManager.Instance.slotSize; i++)
            {
                var slotObj = Instantiate(slotPrefab, transform.position, Quaternion.identity, slotWrapper.transform);

                _itemsDisplayed.Add(slotObj, new InventorySlotBase());
            }

            return;
        }

        for (var i = 0; i < inventory.SlotSize; i++)
        {
            // インベントリコンテナのスロット数がスロットサイズに満たなければスロット確保
            if (inventory.Container.Count < inventory.SlotSize)
            {
                inventory.AddSlot(null);
            }

            var slotObj = Instantiate(slotPrefab, transform.position, Quaternion.identity, slotWrapper.transform);

            _itemsDisplayed.Add(slotObj, inventory.Container[i]);
        }

        SelectSlot(0);
    }

    /// <summary>
    /// インベントリの表示を更新する
    /// </summary>
    private void UpdateRender()
    {
        foreach (var slot in _itemsDisplayed)
        {
            var slotText  = slot.Key.GetComponentInChildren<Text>();
            var slotImage = slot.Key.transform.Find("ItemImage")?.GetComponent<Image>();

            // アイテムがあればアイテム名表示
            slotText.text = (slot.Value.Item != null)
                                ? slot.Value.Item.ItemName
                                : "";

            if (slotImage       != null &&
                slot.Value.Item != null)
            {
                slotImage.sprite = slot.Value.Item.ItemIcon;
                slotImage.color  = new Color(slotImage.color.r, slotImage.color.g, slotImage.color.b, 1);
            }
        }
    }

    /// <summary>
    /// プレイヤーインベントリの描画を更新する
    /// </summary>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    private void UpdatePlayerInventoryRender()
    {
        // プレイヤーインベントリでのみ実行
        if (isForRefrigerator) return;

        foreach (var slot in _itemsDisplayed)
        {
            var bakedIcon  = slot.Key.transform.Find("StateIcons/BakedIcon").gameObject.GetComponent<Image>();
            var boiledIcon = slot.Key.transform.Find("StateIcons/BoiledIcon").gameObject.GetComponent<Image>();
            var cutIcon    = slot.Key.transform.Find("StateIcons/CutIcon").gameObject.GetComponent<Image>();

            // スロットにアイテムがなければアイコンを表示しない
            if (slot.Value.Item == null)
            {
                SetAllColor(Color.clear);

                continue;
            }

            // 一旦グレーアウト
            SetAllColor(stateGrayOutColor);

            foreach (var state in slot.Value.States)
            {
                // 各アイコンの表示設定
                switch (state)
                {
                    case FoodState.Cooked:
                        bakedIcon.color = Color.white;

                        break;

                    case FoodState.Boil:
                        boiledIcon.color = Color.white;

                        break;

                    case FoodState.Cut:
                        cutIcon.color = Color.white;

                        break;

                    // Noneの食材は調味料であり、調理できないためアイコンは表示しない
                    case FoodState.None:
                        SetAllColor(Color.clear);

                        break;

                    case FoodState.Raw:
                    case FoodState.Burnt:
                        break;

                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

            void SetAllColor(Color colorToSet)
            {
                bakedIcon.color  = colorToSet;
                boiledIcon.color = colorToSet;
                cutIcon.color    = colorToSet;
            }
        }
    }

    /// <summary>
    /// 冷蔵庫インベントリの描画を更新する
    /// </summary>
    private void UpdateRefrigeratorInventoryRender()
    {
        // 冷蔵庫インベントリでのみ動作
        if (!isForRefrigerator) return;

        var i          = 0;
        var curNearRef = RefrigeratorManager.Instance.currentNearObj;

        // 近くに冷蔵庫がなければ終了
        if (curNearRef == null) return;

        var refController = curNearRef.GetComponent<RefrigeratorController>();

        // 初回表示時は初期アイテム表示
        if (InventoryManager.Instance.RefContainers.GetContainer(curNearRef.name) == null)
        {
            foreach (var slotObj in _itemsDisplayed.Keys.ToArray())
            {
                _itemsDisplayed[slotObj] = new InventorySlotBase(refController.DefaultItems[i].Item,
                                                                 refController.DefaultItems[i].State);

                i++;
            }
        }
        // 通常はコンテナアイテムを表示
        else
        {
            var nearRefContainer = RefrigeratorManager.Instance.NearRefrigeratorContainer;

            foreach (var slotObj in _itemsDisplayed.Keys.ToArray())
            {
                _itemsDisplayed[slotObj] =
                    new InventorySlotBase(nearRefContainer.GetItem(i), nearRefContainer.GetState(i));

                i++;
            }
        }
    }

    private void UpdateDogToyCountRender()
    {
        // メインシーンでのみ動作
        if (SceneManager.GetActiveScene().name != "GameScenes") return;

        var dogToyCanvas    = GameObject.FindGameObjectWithTag("DogToyCount").GetComponentInChildren<Text>();
        var playerContainer = InventoryManager.Instance.PlayerContainer;

        dogToyCanvas.text = playerContainer.DogFoodCount + "/" + playerContainer.MaxDogFoodCount;
    }

    /// <summary>
    /// インベントリスロットの表示を消去する
    /// </summary>
    public void ClearRender()
    {
        foreach (var slot in _itemsDisplayed.Keys.ToArray())
        {
            var slotText  = slot.GetComponentInChildren<Text>();
            var slotImage = slot.transform.Find("ItemImage")?.GetComponent<Image>();
            slotText.text         = "";
            _itemsDisplayed[slot] = new InventorySlotBase();

            if (slotImage != null)
            {
                slotImage.sprite = null;
                slotImage.color  = new Color(slotImage.color.r, slotImage.color.g, slotImage.color.b, 0);
            }
        }
    }

    /// <summary>
    /// 指定したインデックスのスロットを選択する
    /// 引数がない場合はラストインデックスを使用
    /// </summary>
    /// <param name="index">選択するインデックス</param>
    public void SelectSlot(int index = -1)
    {
        index = (index == -1) ? LastSelectedIndex : index;

        // 指定インデックスがスロットサイズを超過してたら1番目を選択させる
        if (index > _selfSlotSize) index = 0;

        EventSystem.current.SetSelectedGameObject(_itemsDisplayed.Keys.ElementAt(index));
    }

    /// <summary>
    /// 選択スロットのインデックスを取得する
    /// </summary>
    private void UpdateLastSelectedIndex()
    {
        if (EventSystem.current == null) return;

        _currentSelectedObj = EventSystem.current.currentSelectedGameObject;

        if (_currentSelectedObj != _lastSelectedObj)
        {
            var slotWrapperTrf = transform.Find("SlotWrapper");

            if (slotWrapperTrf == null)
            {
                slotWrapperTrf = transform.Find("InventoryWrapper/SlotWrapper");
            }

            for (var i = 0; i < slotWrapperTrf.childCount; i++)
            {
                if (slotWrapperTrf.GetChild(i).gameObject == _currentSelectedObj)
                {
                    LastSelectedIndex = i;
                }
            }
        }

        _lastSelectedObj = _currentSelectedObj;
    }

    /// <summary>
    /// 指定したスロットを取得する
    /// </summary>
    /// <param name="index">スロットのインデックス</param>
    /// <returns>指定スロットのゲームオブジェクト</returns>
    public GameObject GetSlotAt(int index)
    {
        return _itemsDisplayed.Keys.ElementAt(index);
    }

    /// <summary>
    /// 指定したスロットをハイライトする
    /// </summary>
    /// <param name="index">スロットのインデックス</param>
    public void HighlightSlotAt(int index)
    {
        if (index >= _selfSlotSize)
        {
            Debug.LogWarning($"インデックス「{index}」のスロットは存在しません");

            return;
        }

        var targetSlotObj = _itemsDisplayed.Keys.ElementAt(index);
        var selectedCol   = targetSlotObj.GetComponent<Button>().colors.selectedColor;

        targetSlotObj.GetComponent<Image>().color = selectedCol;
    }

    /// <summary>
    /// 指定したスロットのハイライトを解除する
    /// </summary>
    /// <param name="index">スロットのインデックス</param>
    public void UnhighlightSlotAt(int index)
    {
        if (index >= _selfSlotSize)
        {
            Debug.LogWarning($"インデックス「{index}」のスロットは存在しません");

            return;
        }

        _itemsDisplayed.Keys.ElementAt(index).GetComponent<Image>().color = _normalSlotColor;
    }

    /// <summary>
    /// 全スロットを選択可能にする
    /// </summary>
    public void EnableAllSlot()
    {
        foreach (var slot in _itemsDisplayed.Keys)
        {
            slot.GetComponent<Button>().enabled = true;
        }
    }

    /// <summary>
    /// 全スロットを選択不可にする
    /// </summary>
    public void DisableAllSlot()
    {
        foreach (var slot in _itemsDisplayed.Keys)
        {
            slot.GetComponent<Button>().enabled = false;
        }
    }
}
