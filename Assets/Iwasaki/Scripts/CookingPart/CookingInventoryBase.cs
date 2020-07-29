using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CookingInventoryBase : InventoryManager
{
    // インベントリアセット
    public PlayerInventoryContainer container;

    // 投入した食材があったスロットのインデックス
    public int lastPuttedSlotIndex = 0;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public override void SelectSlot(int index)
    {

    }

    public override void DisableAllSlot()
    {

    }

    public override void EnableAllSlot()
    {

    }
}
