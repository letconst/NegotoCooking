using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class RefrigeratorInventory : InventoryManager<RefrigeratorInventory>
{
    // 交換モード時、どの冷蔵庫スロットが選択されたかのインデックスを保持する変数
    [System.NonSerialized]
    public int indexToSwap;
    private GameObject[] _refInvSlotObjs;

    public override void SelectSlot(int index = -1)
    {
        _refInvSlotObjs = GetCurrentNearRefInvSlots();
        index           = index == -1 ? lastSelectedIndex : index;

        EventSystem.current.SetSelectedGameObject(_refInvSlotObjs[index]);
    }

    public override void DisableAllSlot()
    {
        _refInvSlotObjs = GetCurrentNearRefInvSlots();
        IsSlotEnabled   = false;

        foreach (GameObject slotObj in _refInvSlotObjs)
        {
            slotObj.GetComponent<Button>().enabled = false;
        }
    }

    public override void EnableAllSlot()
    {
        _refInvSlotObjs = GetCurrentNearRefInvSlots();
        IsSlotEnabled   = true;

        foreach (GameObject slotObj in _refInvSlotObjs)
        {
            slotObj.GetComponent<Button>().enabled = true;
        }
    }

    /// <summary>
    /// 現在インタラクト可能な距離にある冷蔵庫のインベントリスロット配列をゲームオブジェクトとして取得
    /// </summary>
    /// <returns>冷蔵庫のインベントリスロット配列</returns>
    private GameObject[] GetCurrentNearRefInvSlots()
    {
        return
            RefrigeratorManager.Instance.currentNearObj
            .GetComponentsInChildren<RefrigeratorInventorySlot>()
            .Select(t => t.gameObject).ToArray();
    }
}
