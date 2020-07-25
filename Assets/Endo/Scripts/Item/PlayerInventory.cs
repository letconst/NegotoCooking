using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PlayerInventory : InventoryManager
{
    // アイテム交換モードか否か
    [System.NonSerialized]
    public bool isSwapMode = false;
    private GameObject[] _playerInvSlotObjs;

    // Start is called before the first frame update
    void Start()
    {
        _playerInvSlotObjs = GameObject.FindGameObjectsWithTag("PlayerInventorySlot");
    }

    // Update is called once per frame
    void Update()
    {

    }

    public override void SelectSlot(int index = -1)
    {
        index = index == -1 ? lastSelectedIndex : index;
        EventSystem.current.SetSelectedGameObject(_playerInvSlotObjs[index]);
    }

    public override void DisableAllSlot()
    {
        IsSlotEnabled = false;
        foreach (GameObject slotObj in _playerInvSlotObjs)
        {
            slotObj.GetComponent<Button>().enabled = false;
        }
    }

    public override void EnableAllSlot()
    {
        IsSlotEnabled = true;
        foreach (GameObject slotObj in _playerInvSlotObjs)
        {
            slotObj.GetComponent<Button>().enabled = true;
        }
    }
}
