using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;

public class Refrigerator : MonoBehaviour
{
    // プレイヤーが近くにいるか否か
    private bool _isNear = false;
    // 冷蔵庫のインベントリオブジェクト
    private GameObject _selfInvObj;
    private GameObject _playerInvObj;

    private RefrigeratorInventory _refInv;
    private PlayerInventory       _playerInv;

    // Start is called before the first frame update
    void Start()
    {
        _selfInvObj   = transform.Find("RefrigeratorInventoryCanvas").gameObject;
        _playerInvObj = GameObject.FindGameObjectWithTag("PlayerInventory");

        _refInv    = _selfInvObj.GetComponentInChildren<RefrigeratorInventory>();
        _playerInv = _playerInvObj.GetComponent<PlayerInventory>();

        // 冷蔵庫インベントリを非表示
        _selfInvObj.SetActive(false);
    }

    private void Update()
    {
        // X押下でインベントリを開閉
        if ((Input.GetKeyDown("joystick button 2") || Input.GetKeyDown(KeyCode.E)) && _isNear)
        {
            // 開閉切り替え
            _selfInvObj.SetActive(!_selfInvObj.activeSelf);

            // 開いたとき
            if (_selfInvObj.activeSelf)
            {
                // 冷蔵庫インベントリにフォーカス
                _refInv.SelectSlot(0);
                // プレイヤーインベントリを無効化
                _playerInv.DisableAllSlot();
            }
            // 閉じたとき
            else
            {
                // プレイヤーインベントリを有効化
                _playerInv.EnableAllSlot();
                // プレイヤーインベントリにフォーカスを戻す
                _playerInv.SelectSlot();

                // アイテム交換モードの最中だったら解除し、冷蔵庫をenabledに戻す
                if (_playerInv.isSwapMode)
                {
                    _playerInv.isSwapMode = false;
                    _refInv.EnableAllSlot();
                }
            }
        }
    }

    private void OnTriggerEnter()
    {
        _isNear = true;
        RefrigeratorManager.Instance.currentNearObj = gameObject;
    }

    private void OnTriggerExit()
    {
        _isNear = false;
        RefrigeratorManager.Instance.currentNearObj = null;

        // インベントリが開いているなら閉じる
        if (_selfInvObj.activeSelf) _selfInvObj.SetActive(!_selfInvObj.activeSelf);
        // プレイヤーインベントリを有効化
        _playerInv.EnableAllSlot();
        // プレイヤーインベントリにフォーカスを戻す
        _playerInv.SelectSlot();

        // アイテム交換モードだったら解除し、冷蔵庫をenabledに戻す
        if (_playerInv.isSwapMode)
        {
            _playerInv.isSwapMode = false;
            _refInv.EnableAllSlot();
        }
    }
}
