using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

public class RefrigeratorInventorySlot : InventorySlot
{
    private GameObject   _playerInvObj;
    private GameObject[] _playerInvSlotObjs;

    private PlayerInventory _playerInv;

    // Start is called before the first frame update
    void Start()
    {
        _selfInvObj        = GameObject.FindGameObjectWithTag("RefrigeratorInventory");
        _selfInvSlotObjs   = transform.parent.gameObject.GetComponentsInChildren<RefrigeratorInventorySlot>().Select(t => t.gameObject).ToArray();
        _playerInvObj      = GameObject.FindGameObjectWithTag("PlayerInventory");
        _playerInvSlotObjs = GameObject.FindGameObjectsWithTag("PlayerInventorySlot");

        _playerInv = _playerInvObj.GetComponent<PlayerInventory>();
    }

    /// <summary>
    /// 選択アイテムをプレイヤーインベントリにぶち込む
    /// </summary>
    public override void OnClick()
    {
        PlayerInventorySlot   _playerInvSlot;
        RefrigeratorInventory _refInv = RefrigeratorManager.Instance.currentNearObj.GetComponentInChildren<RefrigeratorInventory>();

        // スロットにアイテムがなければ弾く
        if (selfItem == null) return;

        // プレイヤーインベントリを回す
        for (int i = 0; i < _playerInvSlotObjs.Length; i++)
        {
            _playerInvSlot = _playerInvSlotObjs[i].GetComponent<PlayerInventorySlot>();

            // プレイヤーインベントリに空きがあったら
            if (_playerInvSlot.selfItem == null)
            {
                // そのスロットにアイテムを配置
                _playerInvSlot.SetItem(selfItem, _playerInv);

                // 冷蔵庫スロットは空にする
                RemoveItem(_refInv);
                return;
            }
        }

        /* プレイヤーインベントリに空きがなければ交換させる */

        // 交換アイテムに現在スロットのアイテムを指定
        _refInv.itemToSwap = selfItem;
        // 冷蔵庫に交換元のインデックスを指定
        _refInv.indexToSwap = GetSelfIndex(_selfInvSlotObjs, gameObject);
        // プレイヤーインベントリをアイテム交換モードに変更
        _playerInv.isSwapMode = true;
        // 冷蔵庫Invを無効化し、プレイヤーInvにフォーカス
        _refInv.DisableAllSlot();
        _playerInv.EnableAllSlot();
        _playerInv.SelectSlot();
        // ここから先はPlayerInventorySlot.OnClick()
    }
}
