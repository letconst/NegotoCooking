using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class RefrigeratorInventorySlot : InventorySlot
{
    private GameObject _playerInvCanvasObj;
    private GameObject _playerInvObj;
    private GameObject[] _playerInvSlotObjs;
    private GameObject[] _refInvSlotObjs;

    private Canvas _playerInvCanvas;
    private PlayerInventory _playerInv;
    private PlayerInventorySlot _playerInvSlot;
    private RefrigeratorInventory _refInv;
    private RefrigeratorInventorySlot _refInvSlot;

    // Start is called before the first frame update
    void Start()
    {
        _invObj      = GameObject.FindGameObjectWithTag("RefrigeratorInventory");
        _invSlotObjs = GameObject.FindGameObjectsWithTag("RefrigeratorInventorySlot");
        _playerInvCanvasObj = GameObject.Find("PlayerInventoryCanvas");
        _playerInvObj = GameObject.FindGameObjectWithTag("PlayerInventory");
        _playerInvSlotObjs = GameObject.FindGameObjectsWithTag("PlayerInventorySlot");
        _refInvSlotObjs = GameObject.FindGameObjectsWithTag("RefrigeratorInventorySlot");

        _playerInvCanvas = _playerInvCanvasObj.GetComponent<Canvas>();
        _playerInv = _playerInvObj.GetComponent<PlayerInventory>();
        _refInv = _invObj.GetComponent<RefrigeratorInventory>();

        // インベントリスロットを選択
        //EventSystem.current.SetSelectedGameObject(_invSlots[0]);
    }

    /// <summary>
    /// 選択アイテムをプレイヤーインベントリにぶち込む
    /// TODO: プレイヤーインベントリがいっぱいなら交換させる
    /// </summary>
    public override void OnClick()
    {
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
