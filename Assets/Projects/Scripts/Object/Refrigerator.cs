using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;

// TODO: 複数冷蔵庫を配置することを考慮し、インベントリのようにマネージャを作成する

public class Refrigerator : MonoBehaviour
{
    // オブジェクト付近にプレイヤーがいるか否か
    private bool _isNear = false;

    private GameObject
        _invObj,
        _refInvObj,
        _playerInvObj;

    private PlayerInventory       _playerInv;
    private RefrigeratorInventory _refInv;

    // Start is called before the first frame update
    void Start()
    {
        // インベントリ初期化
        _invObj       = transform.GetChild(0).gameObject;
        _refInvObj    = GameObject.FindGameObjectWithTag("RefrigeratorInventory");
        _playerInvObj = GameObject.FindGameObjectWithTag("PlayerInventory");

        _refInv    = _refInvObj.GetComponent<RefrigeratorInventory>();
        _playerInv = _playerInvObj.GetComponent<PlayerInventory>();

        // 冷蔵庫インベントリを非表示
        _invObj.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        // X押下でインベントリを開閉
        if (Input.GetKeyDown("joystick button 2") && _isNear)
        {
            // 開閉切り替え
            _invObj.SetActive(!_invObj.activeSelf);

            // 開いたとき
            if (_invObj.activeSelf)
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
                if (_playerInv.IsSwapMode)
                {
                    _playerInv.IsSwapMode = false;
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
    }

    /// <summary>
    /// 冷蔵庫から離れた際の処理
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerExit(Collider other)
    {
        _isNear = false;

        // インベントリが開いているなら閉じる
        // TODO: 開いている間は移動できないようにするか要相談
        if (_invObj.activeSelf) _invObj.SetActive(!_invObj.activeSelf);
        // プレイヤーインベントリを有効化
        _playerInv.EnableAllSlot();
        // プレイヤーインベントリにフォーカスを戻す
        _playerInv.SelectSlot();

        // アイテム交換モードの最中だったら解除し、冷蔵庫をenabledに戻す
        if (_playerInv.IsSwapMode)
        {
            _playerInv.IsSwapMode = false;
            _refInv.EnableAllSlot();
        }
    }
}
