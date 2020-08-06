using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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

    // Start is called before the first frame update
    void Start()
    {
        CreateDisplay();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateDisplay();
    }

    /// <summary>
    /// インベントリ表示の初期化
    /// </summary>
    private void CreateDisplay()
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
    private void UpdateDisplay()
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
}
