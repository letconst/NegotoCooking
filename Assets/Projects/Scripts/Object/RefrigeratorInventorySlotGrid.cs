using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RefrigeratorInventorySlotGrid : InventorySlotGrid
{
    // 冷蔵庫インベントリのインスタンス
    private RefrigeratorInventory _inv;

    // Start is called before the first frame update
    void Start()
    {
        _invObj = GameObject.FindGameObjectWithTag("RefrigeratorInventory");
        _inv    = _invObj.GetComponent<RefrigeratorInventory>();

        for (int i = 0; i < _inv.SlotSize; i++)
        {
            // スロットのインスタンス
            GameObject                slotObj = Instantiate(_slotPrefab, this.transform);
            RefrigeratorInventorySlot slot    = slotObj.GetComponent<RefrigeratorInventorySlot>();

            // インスペクタでアイテム指定がある場合はそれをスロットに配置する
            // 指定がなければnullを持たせる
            if (i < _inv.AllItems.Length)
            {
                slot.SetItem(_inv.AllItems[i]);
            }
            else
            {
                slot.SetItem(null);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
