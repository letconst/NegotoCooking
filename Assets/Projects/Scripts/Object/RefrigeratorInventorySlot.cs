using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class RefrigeratorInventorySlot : InventorySlot
{
    private GameObject   _playerInvObj;
    private GameObject[] _playerInvSlotObjs;
    private GameObject[] _refInvSlotObjs;

    private PlayerInventory       _playerInv;
    private RefrigeratorInventory _refInv;

    // Start is called before the first frame update
    void Start()
    {
        _invObj            = GameObject.FindGameObjectWithTag("RefrigeratorInventory");
        _playerInvObj      = GameObject.FindGameObjectWithTag("PlayerInventory");
        _playerInvSlotObjs = GameObject.FindGameObjectsWithTag("PlayerInventorySlot");
        _refInvSlotObjs    = GameObject.FindGameObjectsWithTag("RefrigeratorInventorySlot");

        _playerInv = _playerInvObj.GetComponent<PlayerInventory>();
        _refInv    = _invObj.GetComponent<RefrigeratorInventory>();
    }

    /// <summary>
    /// 選択アイテムをプレイヤーインベントリにぶち込む
    /// プレイヤーインベントリがいっぱいなら交換させる
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
                _playerInvSlot.SelfItem      = SelfItem;
                _playerInvSlot.ItemName.text = ItemName.text;

                // 冷蔵庫スロットは空にする
                SelfItem      = null;
                ItemName.text = null;
                return;
            }
        }

        // プレイヤーインベントリに空きがなければ交換させる

        // 交換アイテムに現在スロットのアイテムを指定
        _refInv.ItemToSwap = SelfItem;
        // 冷蔵庫に交換元のインデックスを指定
        _refInv.IndexToSwap = GetSelfIndex(_refInvSlotObjs, gameObject);
        // プレイヤーインベントリをアイテム交換モードに変更
        _playerInv.IsSwapMode = true;
        // 冷蔵庫Invを無効化し、プレイヤーInvにフォーカス
        _refInv.DisableAllSlot();
        _playerInv.EnableAllSlot();
        _playerInv.SelectSlot();
        // ここから先はPlayerInventorySlot.OnClick()
    }
}
