using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class RefrigeratorInventory : InventoryManager<RefrigeratorInventory>
{
    [System.NonSerialized]
    public int indexToSwap;
    private GameObject[] _invSlotObjs;

    // Start is called before the first frame update
    void Start()
    {
        _invSlotObjs = GameObject.FindGameObjectsWithTag("RefrigeratorInventorySlot");
    }

    public override void SelectSlot(int index = -1)
    {
        index = index == -1 ? lastSelectedIndex : index;
        EventSystem.current.SetSelectedGameObject(_invSlotObjs[index]);
    }

    public override void DisableAllSlot()
    {
        IsSlotEnabled = false;
        foreach (GameObject slotObj in _invSlotObjs)
        {
            slotObj.GetComponent<Button>().enabled = false;
        }
    }

    public override void EnableAllSlot()
    {
        IsSlotEnabled = true;
        foreach (GameObject slotObj in _invSlotObjs)
        {
            slotObj.GetComponent<Button>().enabled = true;
        }
    }
}
