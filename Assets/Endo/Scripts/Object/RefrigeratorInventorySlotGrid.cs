using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RefrigeratorInventorySlotGrid : InventorySlotGrid
{
    // Start is called before the first frame update
    void Start()
    {
        GameObject            _refInvObj = gameObject.transform.parent.gameObject;
        RefrigeratorInventory _refInv    = _refInvObj.GetComponent<RefrigeratorInventory>();

        for (int i = 0; i < _refInv.SlotSize; i++)
        {
            // スロットのインスタンス
            GameObject                slotObj = Instantiate(_slotPrefab, transform);
            RefrigeratorInventorySlot slot    = slotObj.GetComponent<RefrigeratorInventorySlot>();

            // インスペクタでアイテム指定がある場合はそれをスロットに配置する
            // 指定がなければnullを持たせる
            if (i < _refInv.AllItems.Length)
            {
                slot.SetItem(_refInv.AllItems[i]);
            }
            else
            {
                slot.SetItem(null);
            }
        }
    }
}
