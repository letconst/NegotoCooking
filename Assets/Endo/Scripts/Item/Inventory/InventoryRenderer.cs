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

    // 最大スロット数
    private int _selfSlotSize;

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
        // 冷蔵庫ならまず中身を取得
        if (isForRefrigerator)
        {
            var i             = 0;
            var curNearRef    = RefrigeratorManager.Instance.currentNearObj;
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

        foreach (var slot in _itemsDisplayed)
        {
            var slotText = slot.Key.GetComponentInChildren<Text>();
            var slotImage = slot.Key.transform.Find("ItemImage")?.GetComponent<Image>();

            // アイテムがあればアイテム名表示
            slotText.text = (slot.Value.Item != null)
                                ? slot.Value.FullItemName
                                : "";
            if(slotImage!=null && slot.Value.Item!=null)
            {
                slotImage.sprite = slot.Value.Item.ItemIcon;
                slotImage.color = new Color(slotImage.color.r, slotImage.color.g, slotImage.color.b, 1);
            }
        }
    }

    private void UpdateDogToyCountRender()
    {
        // メインシーンでのみ動作
        if (SceneManager.GetActiveScene().name != "GameScenes") return;

        var dogToyCanvas = GameObject.FindGameObjectWithTag("DogToyCount").GetComponentInChildren<Text>();
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
            var slotText = slot.GetComponentInChildren<Text>();
            var slotImage = slot.transform.Find("ItemImage")?.GetComponent<Image>();
            slotText.text            = "";
            _itemsDisplayed[slot] = new InventorySlotBase();
            if(slotImage!=null)
            {
                slotImage.sprite = null;
                slotImage.color = new Color(slotImage.color.r, slotImage.color.g, slotImage.color.b, 0);
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
