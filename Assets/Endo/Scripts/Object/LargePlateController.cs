using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LargePlateController : MonoBehaviour
{
    // インベントリアセット
    public InventoryContainerBase selfContainer;
    // 近くにいるか否か
    private bool _isNear = false;

    private GameObject               _soup;
    private GameObject               _playerInvObj;
    private PlayerInventoryContainer _playerInvContainer;
    private InventoryRenderer        _playerInvRenderer;

    // Start is called before the first frame update
    void Start()
    {
        _soup               = transform.Find("Soup").gameObject;
        _playerInvObj       = GameObject.FindGameObjectWithTag("PlayerInventory");
        _playerInvContainer = TmpInventoryManager.Instance.playerContainer;
        _playerInvRenderer  = _playerInvObj.GetComponent<InventoryRenderer>();

        _soup.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if ((Input.GetKeyDown("joystick button 2") || Input.GetKeyDown(KeyCode.E)) &&
            _isNear)
        {
            // 焼けてるときだけ大皿にぶち込む
            if (_playerInvContainer.GetState(_playerInvRenderer.LastSelectedIndex) != FoodState.Cooked) return;

            // 大皿に現在選択しているアイテムをぶち込む
            selfContainer.AddItem(_playerInvContainer.GetItem(_playerInvRenderer.LastSelectedIndex),
                              _playerInvContainer.GetState(_playerInvRenderer.LastSelectedIndex));

            // プレイヤーのアイテムを削除
            _playerInvContainer.RemoveItem(_playerInvRenderer.LastSelectedIndex);
        }

        // コンテナにアイテムが1つでも入ったらスープを表示
        if (selfContainer.Container.Count > 0) _soup.SetActive(true);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) _isNear = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player")) _isNear = false;
    }
}
