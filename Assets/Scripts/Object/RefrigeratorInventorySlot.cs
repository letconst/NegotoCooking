using System.Collections;
using System.Collections.Generic;
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
        _refInvSlotObjs    = GameObject.FindGameObjectsWithTag("RefrigeratorInventorySlot");
    }

    /// <summary>
    /// 選択アイテムをプレイヤーインベントリにぶち込む
    /// TODO: プレイヤーインベントリがいっぱいなら交換させる
    /// </summary>
    public override void OnClick()
    {
        PlayerInventorySlot _playerInvSlot;

        // スロットにアイテムがなければ弾く
        if (SelfItem == null) return;

        // プレイヤーインベントリを回す
        for (int i = 0; i < _playerInvSlotObjs.Length; i++)
        {
            _playerInvSlot = _playerInvSlotObjs[i].GetComponent<PlayerInventorySlot>();

            // プレイヤーインベントリに空きがあったら
            if (_playerInvSlot.SelfItem == null)
            {
                // そのスロットにアイテムを配置して名前を表示させる
                _playerInvSlot.SelfItem = SelfItem;
                _playerInvSlot.ItemName.text = ItemName.text;

                // 冷蔵庫スロットは空にする
                SelfItem = null;
                ItemName.text = null;
                return;
            }
        }

        // プレイヤーインベントリに空きがなければ交換させる

        // 交換アイテムに現在スロットのアイテムを指定
        RefrigeratorInventory.Instance.itemToSwap = SelfItem;
        // 冷蔵庫に交換元のインデックスを指定
        RefrigeratorInventory.Instance.indexToSwap = GetSelfIndex(_refInvSlotObjs, gameObject);
        // プレイヤーインベントリをアイテム交換モードに変更
        PlayerInventory.Instance.isSwapMode = true;
        // 冷蔵庫Invを無効化し、プレイヤーInvにフォーカス
        RefrigeratorInventory.Instance.DisableAllSlot();
        PlayerInventory.Instance.EnableAllSlot();
        PlayerInventory.Instance.SelectSlot();
        // ここから先はPlayerInventorySlot.OnClick()
    }
}
