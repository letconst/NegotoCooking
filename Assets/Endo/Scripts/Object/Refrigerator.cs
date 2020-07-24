using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;

public class Refrigerator : MonoBehaviour
{
    // 冷蔵庫のインベントリオブジェクト
    private GameObject _refInvObj;

    private RefrigeratorInventory _refInv;

    // Start is called before the first frame update
    void Start()
    {
        _refInvObj = transform.Find("RefrigeratorInventoryCanvas").gameObject;
        _refInv    = _refInvObj.GetComponentInChildren<RefrigeratorInventory>();

        // 冷蔵庫インベントリを非表示
        _refInvObj.SetActive(false);
    }

    private void OnTriggerEnter()
    {
        RefrigeratorManager.Instance.currentNearObj = gameObject;
    }

    private void OnTriggerStay()
    {
        // X押下でインベントリを開閉
        if (Input.GetKeyDown("joystick button 2") || Input.GetKeyDown(KeyCode.E))
        {
            // 開閉切り替え
            _refInvObj.SetActive(!_refInvObj.activeSelf);

            // 開いたとき
            if (_refInvObj.activeSelf)
            {
                // 冷蔵庫インベントリにフォーカス
                _refInv.SelectSlot(0);
                // プレイヤーインベントリを無効化
                PlayerInventory.Instance.DisableAllSlot();
            }
            // 閉じたとき
            else
            {
                // プレイヤーインベントリを有効化
                PlayerInventory.Instance.EnableAllSlot();
                // プレイヤーインベントリにフォーカスを戻す
                PlayerInventory.Instance.SelectSlot();

                // アイテム交換モードの最中だったら解除し、冷蔵庫をenabledに戻す
                if (PlayerInventory.Instance.isSwapMode)
                {
                    PlayerInventory.Instance.isSwapMode = false;
                    _refInv.EnableAllSlot();
                }
            }
        }
    }

    private void OnTriggerExit()
    {
        RefrigeratorManager.Instance.currentNearObj = null;

        // インベントリが開いているなら閉じる
        if (_refInvObj.activeSelf) _refInvObj.SetActive(!_refInvObj.activeSelf);
        // プレイヤーインベントリを有効化
        PlayerInventory.Instance.EnableAllSlot();
        // プレイヤーインベントリにフォーカスを戻す
        PlayerInventory.Instance.SelectSlot();

        // アイテム交換モードだったら解除し、冷蔵庫をenabledに戻す
        if (PlayerInventory.Instance.isSwapMode)
        {
            PlayerInventory.Instance.isSwapMode = false;
            _refInv.EnableAllSlot();
        }
    }
}
