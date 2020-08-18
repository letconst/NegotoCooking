using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventoryRenderer : MonoBehaviour
{
    [SerializeField, Tooltip("表示させる対象のインベントリコンテナ。冷蔵庫用の場合は未指定で良い")]
    private InventoryContainerBase _inventory;

    [SerializeField, Tooltip("表示させるスロットプレハブ")]
    private GameObject _slotPrefab;

    [SerializeField, Tooltip("生成する場所の親オブジェクト")]
    private GameObject _slotWrapper;

    [SerializeField, Tooltip("冷蔵庫用のインベントリか否か")]
    private bool _isForRefrigerator;

    // 最後に選択していたスロットのインデックス
    private int _lastSelectedIndex = 0;

    // 最大スロット数
    private int _selfSlotSize;

    // 現在選択しているスロット
    private GameObject _currentSelectedObj;

    // 最後に選択していたスロット
    private GameObject _lastSelectedObj;

    // 現在表示されているスロット情報
    private Dictionary<GameObject, InventorySlotBase> _itemsDisplayed = new Dictionary<GameObject, InventorySlotBase>();

    public int LastSelectedIndex { get => _lastSelectedIndex; private set => _lastSelectedIndex = value; }

    // Start is called before the first frame update
    void Start()
    {
        _selfSlotSize = (_inventory != null)
            ? _inventory.SlotSize
            : (_isForRefrigerator)
            ? RefrigeratorManager.Instance.slotSize
            : 0;

        InitRender();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateRender();
        UpdateLastSelectedIndex();
    }

    /// <summary>
    /// インベントリ表示の初期化
    /// </summary>
    private void InitRender()
    {
        // 例外および冷蔵庫かの判定
        if (_inventory == null)
        {
            if (!_isForRefrigerator)
            {
                Debug.LogError("表示対象のインベントリコンテナが未指定です");
                return;
            }

            // 冷蔵庫用の処理
            for (int i = 0; i < RefrigeratorManager.Instance.slotSize; i++)
            {
                GameObject slotObj = Instantiate(_slotPrefab, transform.position, Quaternion.identity, _slotWrapper.transform);

                _itemsDisplayed.Add(slotObj, new InventorySlotBase());
            }

            return;
        }

        for (int i = 0; i < _inventory.SlotSize; i++)
        {
            // インベントリコンテナのスロット数がスロットサイズに満たなければスロット確保
            if (_inventory.Container.Count < _inventory.SlotSize)
            {
                _inventory.AddItem(null);
            }

            GameObject slotObj = Instantiate(_slotPrefab, transform.position, Quaternion.identity, _slotWrapper.transform);

            _itemsDisplayed.Add(slotObj, _inventory.Container[i]);
        }

        SelectSlot(0);
    }

    /// <summary>
    /// インベントリの表示を更新する
    /// </summary>
    private void UpdateRender()
    {
        // 冷蔵庫ならまず中身を取得
        if (_isForRefrigerator)
        {
            int i = 0;
            GameObject             curNearRef = RefrigeratorManager.Instance.currentNearObj;
            RefrigeratorController refCtrlr   = curNearRef.GetComponent<RefrigeratorController>();

            // 初回表示時は初期アイテム表示
            if (TmpInventoryManager.Instance.refContainers.GetContainer(curNearRef.name) == null)
            {
                foreach (GameObject slotObj in _itemsDisplayed.Keys.ToArray())
                {
                    _itemsDisplayed[slotObj] = new InventorySlotBase(refCtrlr.DefaultItems[i], FoodState.Raw);

                    i++;
                }
            }
            // 通常はコンテナアイテムを表示
            else
            {
                var nearRefContainer = RefrigeratorManager.Instance.NearRefrigeratorContainer;

                foreach (GameObject slotObj in _itemsDisplayed.Keys.ToArray())
                {
                    _itemsDisplayed[slotObj] = new InventorySlotBase(nearRefContainer.GetItem(i), nearRefContainer.GetState(i));

                    i++;
                }
            }

        }

        foreach (KeyValuePair<GameObject, InventorySlotBase> slot in _itemsDisplayed)
        {
            Text slotText = slot.Key.GetComponentInChildren<Text>();

            // アイテムがあればアイテム名表示
            if (slot.Value.Item != null)
            {
                slotText.text = slot.Value.FullItemName;
            }
            else
            {
                slotText.text = "";
            }
        }
    }

    /// <summary>
    /// インベントリスロットの表示を消去する
    /// </summary>
    public void ClearRender()
    {
        foreach (GameObject slotObj in _itemsDisplayed.Keys.ToArray())
        {
            Text slotText            = slotObj.GetComponentInChildren<Text>();
            slotText.text            = "";
            _itemsDisplayed[slotObj] = new InventorySlotBase();
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
    public void UpdateLastSelectedIndex()
    {
        if (EventSystem.current == null) return;

        _currentSelectedObj = EventSystem.current.currentSelectedGameObject;

        if (_currentSelectedObj != _lastSelectedObj)
        {
            Transform slotWrapperTrf = transform.Find("SlotWrapper");

            if (slotWrapperTrf == null)
            {
                slotWrapperTrf = transform.Find("InventoryWrapper/SlotWrapper");
            }

            for (int i = 0; i < slotWrapperTrf.childCount; i++)
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
        foreach (GameObject slot in _itemsDisplayed.Keys)
        {
            slot.GetComponent<Button>().enabled = true;
        }
    }

    /// <summary>
    /// 全スロットを選択不可にする
    /// </summary>
    public void DisableAllSlot()
    {
        foreach (GameObject slot in _itemsDisplayed.Keys)
        {
            slot.GetComponent<Button>().enabled = false;
        }
    }
}
