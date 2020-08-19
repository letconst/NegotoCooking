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
        _playerInvContainer = TmpInventoryManager.Instance.PlayerContainer;
        _playerInvRenderer  = _playerInvObj.GetComponent<InventoryRenderer>();

        _soup.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (_isNear && (Input.GetKeyDown("joystick button 2") ||
                        Input.GetKeyDown(KeyCode.E)))
        {
            Item      selectedFood      = _playerInvContainer.GetItem(_playerInvRenderer.LastSelectedIndex);
            FoodState selectedFoodState = _playerInvContainer.GetState(_playerInvRenderer.LastSelectedIndex);

            // 調味料か調理済みの食材のみ受け付ける
            if (selectedFood.KindOfItem1 != Item.KindOfItem.Seasoning && (selectedFoodState == FoodState.None ||
                                                                          selectedFoodState == FoodState.Raw) ) return;

            // 大皿に現在選択しているアイテムをぶち込む
            selfContainer.AddItem(_playerInvContainer.GetItem(_playerInvRenderer.LastSelectedIndex),
                                  selectedFoodState);

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
