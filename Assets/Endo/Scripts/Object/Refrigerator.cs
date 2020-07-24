using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;

// TODO: 複数冷蔵庫を配置することを考慮し、インベントリのようにマネージャを作成する

public class Refrigerator : MonoBehaviour
{
    // 冷蔵庫のインベントリオブジェクト
    private GameObject _refInvObj;
    // オブジェクト付近にプレイヤーがいるか否か
    private bool _isNear = false;

    private RefrigeratorInventory _refInv;

    // Start is called before the first frame update
    void Start()
    {
        _refInvObj = transform.Find("RefrigeratorInventoryCanvas").gameObject;
        _refInv = _refInvObj.GetComponentInChildren<RefrigeratorInventory>();

        // 冷蔵庫インベントリを非表示
        _refInvObj.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        // X押下でインベントリを開閉
        if (Input.GetKeyDown("joystick button 2") && _isNear)
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

    /// <summary>
    /// 冷蔵庫に近づいた際の処理
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter(Collider other)
    {
        _isNear = true;
        RefrigeratorManager.Instance.currentNearObj = gameObject;
    }

    /// <summary>
    /// 冷蔵庫から離れた際の処理
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerExit(Collider other)
    {
        _isNear = false;
        RefrigeratorManager.Instance.currentNearObj = null;

        // インベントリが開いているなら閉じる
        if (_refInvObj.activeSelf) _refInvObj.SetActive(!_refInvObj.activeSelf);
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
