﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PlayerInventory : InventoryManager
{
    // インベントリアセット
    public PlayerInventoryContainer container;
    // アイテム交換モードか否か
    [System.NonSerialized]
    public bool isSwapMode = false;
    private GameObject[] _playerInvSlotObjs;

    private void Update()
    {
        if (_playerInvSlotObjs == null)
        {
            _playerInvSlotObjs = GameObject.FindGameObjectsWithTag("PlayerInventorySlot");
        }
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
