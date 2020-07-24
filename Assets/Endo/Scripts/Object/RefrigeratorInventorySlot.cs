using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

public class RefrigeratorInventorySlot : InventorySlot
{
    private GameObject[] _playerInvSlotObjs;
    private GameObject[] _refInvSlotObjs;

    // Start is called before the first frame update
    void Start()
    {
        _playerInvSlotObjs = GameObject.FindGameObjectsWithTag("PlayerInventorySlot");
        _refInvSlotObjs    = transform.parent.gameObject.GetComponentsInChildren<RefrigeratorInventorySlot>().Select(t => t.gameObject).ToArray();
    }

    /// <summary>
    /// 選択アイテムをプレイヤーインベントリにぶち込む
    /// TODO: プレイヤーインベントリがいっぱいなら交換させる
    /// </summary>
    public override void OnClick()
    {
        PlayerInventorySlot   _playerInvSlot;
        RefrigeratorInventory _refInv = RefrigeratorManager.Instance.currentNearObj.GetComponentInChildren<RefrigeratorInventory>();

        // スロットにアイテムがなければ弾く
        if (SelfItem == null) return;

        // プレイヤーインベントリを回す
        for (int i = 0; i < _playerInvSlotObjs.Length; i++)
        {
            _playerInvSlot = _playerInvSlotObjs[i].GetComponent<PlayerInventorySlot>();

            // プレイヤーインベントリに空きがあったら
            if (_playerInvSlot.SelfItem == null)
            {
                // そのスロットにアイテムを配置
                _playerInvSlot.SetItem(SelfItem);

                // 冷蔵庫スロットは空にする
                RemoveItem();
                return;
            }
        }

        // プレイヤーインベントリに空きがなければ交換させる

        // 交換アイテムに現在スロットのアイテムを指定
        _refInv.itemToSwap = SelfItem;
        // 冷蔵庫に交換元のインデックスを指定
        _refInv.indexToSwap = GetSelfIndex(_refInvSlotObjs, gameObject);
        // プレイヤーインベントリをアイテム交換モードに変更
        PlayerInventory.Instance.isSwapMode = true;
        // 冷蔵庫Invを無効化し、プレイヤーInvにフォーカス
        _refInv.DisableAllSlot();
        PlayerInventory.Instance.EnableAllSlot();
        PlayerInventory.Instance.SelectSlot();
        // ここから先はPlayerInventorySlot.OnClick()
    }
}
