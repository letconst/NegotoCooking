using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LargePlateController : MonoBehaviour
{
    // インベントリアセット
    public InventoryContainerBase container;
    // 近くにいるか否か
    private bool _isNear = false;

    private GameObject      _soup;
    private GameObject      _playerInvObj;
    private GameObject[]    _playerInvSlotObjs;
    private PlayerInventory _playerInv;

    // Start is called before the first frame update
    void Start()
    {
        _soup         = transform.Find("Soup").gameObject;
        _playerInvObj = GameObject.FindGameObjectWithTag("PlayerInventory");
        _playerInv    = _playerInvObj.GetComponent<PlayerInventory>();

        _soup.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (_playerInvSlotObjs == null)
        {
            _playerInvSlotObjs = GameObject.FindGameObjectsWithTag("PlayerInventorySlot");
        }

        if ((Input.GetKeyDown("joystick button 2") || Input.GetKeyDown(KeyCode.E)) &&
            _isNear)
        {
            // 焼けてるときだけ大皿にぶち込む
            if (_playerInv.container.GetState(_playerInv.lastSelectedIndex) != FoodState.Cooked) return;

            // 大皿に現在選択しているアイテムをぶち込む
            container.AddItem(_playerInv.container.GetItem(_playerInv.lastSelectedIndex),
                              _playerInv.container.GetState(_playerInv.lastSelectedIndex));

            // プレイヤーのアイテムを削除
            _playerInvSlotObjs[_playerInv.lastSelectedIndex]
                .GetComponent<PlayerInventorySlot>().RemoveItem(_playerInv);
        }

        // コンテナにアイテムが1つでも入ったらスープを表示
        if (container.Container.Count > 0) _soup.SetActive(true);
    }

    private void OnTriggerEnter() => _isNear = true;

    private void OnTriggerExit() => _isNear = false;

    /// <summary>
    /// ゲーム終了時に大皿の中身を削除する
    /// </summary>
    private void OnApplicationQuit()
    {
        container.Container.Clear();
    }
}
