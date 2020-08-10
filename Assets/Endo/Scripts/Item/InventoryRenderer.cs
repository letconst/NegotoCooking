using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventoryRenderer : MonoBehaviour
{
    [SerializeField, Tooltip("表示させる対象のインベントリコンテナ")]
    private InventoryContainerBase _inventory;

    [SerializeField, Tooltip("表示させるスロットプレハブ")]
    private GameObject _slotPrefab;

    [SerializeField, Tooltip("生成する場所の親オブジェクト")]
    private GameObject _slotWrapper;

    private Dictionary<GameObject, InventorySlotBase> _itemsDisplayed = new Dictionary<GameObject, InventorySlotBase>();
    // 最後に選択していたスロットのインデックス
    private int _lastSelectedIndex = 0;

    // Start is called before the first frame update
    void Start()
    {
        InitRender();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateRender();
    }

    /// <summary>
    /// インベントリ表示の初期化
    /// </summary>
    private void InitRender()
    {
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
    }

    /// <summary>
    /// インベントリの表示を更新する
    /// </summary>
    private void UpdateRender()
    {
        foreach (KeyValuePair<GameObject, InventorySlotBase> slot in _itemsDisplayed)
        {
            Text slotText = slot.Key.GetComponentInChildren<Text>();

            // アイテムがあればアイテム名表示
            if (slot.Value.Item != null)
            {
                // 状態に応じて名称変化
                switch (slot.Value.State)
                {
                    case FoodState.None:
                        Debug.LogWarning($"アイテム「{slot.Value.Item.ItemName}」の状態がNoneです");
                        slotText.text = slot.Value.Item.ItemName;
                        break;

                    case FoodState.Raw:
                        slotText.text = slot.Value.Item.ItemName;
                        break;

                    case FoodState.Cooked:
                        slotText.text = $"焼けた{slot.Value.Item.ItemName}";
                        break;

                    case FoodState.Burnt:
                        slotText.text = $"焦げた{slot.Value.Item.ItemName}";
                        break;

                    default:
                        Debug.LogWarning($"「{slot.Value.State}」の接頭辞が未設定です");
                        slotText.text = slot.Value.Item.ItemName;
                        break;
                }
            }
            else
            {
                slotText.text = "";
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
        index = (index == -1) ? _lastSelectedIndex : index;

        // 指定インデックスがスロットサイズを超過してたら1番目を選択させる
        if (index > _inventory.SlotSize) index = 0;

        EventSystem.current.SetSelectedGameObject(_itemsDisplayed.Keys.ElementAt(index));
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
