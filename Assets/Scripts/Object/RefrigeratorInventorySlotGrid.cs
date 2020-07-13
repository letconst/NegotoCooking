using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RefrigeratorInventorySlotGrid : InventorySlotGrid
{
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < RefrigeratorInventory.Instance.SlotSize; i++)
        {
            // スロットのインスタンス
            GameObject                slotObj = Instantiate(_slotPrefab, this.transform);
            RefrigeratorInventorySlot slot    = slotObj.GetComponent<RefrigeratorInventorySlot>();

            // インスペクタでアイテム指定がある場合はそれをスロットに配置する
            // 指定がなければnullを持たせる
            Debug.Log(RefrigeratorInventory.Instance.AllItems);
            if (i < RefrigeratorInventory.Instance.AllItems.Length)
            {
                slot.SetItem(RefrigeratorInventory.Instance.AllItems[i]);
            }
            else
            {
                slot.SetItem(null);
            }
        }
    }
}
