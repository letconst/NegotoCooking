using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RefrigeratorController : MonoBehaviour
{
    [SerializeField, Tooltip("冷蔵庫内に初期配置させるアイテム（3個まで）")]
    private Item[] _defaultItems = new Item[3];

    // 近くにいるか否か
    private bool _isNear = false;

    private GameObject _playerInvObj;
    private GameObject _refInvObj;

    private InventoryRenderer                  _playerInvRenderer;
    private InventoryRenderer                  _selfInvRenderer;
    private RefrigeratorInventoryContainers    _refContainers;
    private RefrigeratorInventoryContainerBase _selfContainer;

    public Item[] DefaultItems { get => _defaultItems; private set => _defaultItems = value; }

    // Start is called before the first frame update
    void Start()
    {
        _playerInvObj = GameObject.FindGameObjectWithTag("PlayerInventory");
        _refInvObj    = GameObject.FindGameObjectWithTag("RefrigeratorInventory");

        _playerInvRenderer = _playerInvObj.GetComponent<InventoryRenderer>();
        _selfInvRenderer   = _refInvObj.GetComponent<InventoryRenderer>();
        _refContainers     = TmpInventoryManager.Instance.RefContainers;

        CreateContainer();
    }

    // Update is called once per frame
    void Update()
    {
        // X押下でインベントリ開閉
        if (_isNear && (Input.GetKeyDown("joystick button 2") ||
                        Input.GetKeyDown(KeyCode.E)))
        {
            // 開閉切り替え
            _refInvObj.SetActive(!_refInvObj.activeSelf);
            // 閉じるときは表示リセット
            _selfInvRenderer.ClearRender();

            // 開いたとき
            if (_refInvObj.activeSelf)
            {
                // 冷蔵庫インベントリにフォーカス
                _selfInvRenderer.SelectSlot();

                // プレイヤーインベントリを無効化
                _playerInvRenderer.DisableAllSlot();
            }
            // 閉じたとき
            else
            {
                // プレイヤーインベントリを有効化
                _playerInvRenderer.EnableAllSlot();
                _playerInvRenderer.SelectSlot();
            }
        }
    }

    /// <summary>
    /// 冷蔵庫のコンテナがなければ新規追加し、初期アイテムをコンテナに設定する
    /// </summary>
    private void CreateContainer()
    {
        // コンテナがすでに作成されていたら追加を行わない
        if (_refContainers.GetContainer(gameObject.name) != null) return;

        _refContainers.AddContainer(gameObject.name);

        _selfContainer = _refContainers.GetContainer(gameObject.name);

        for (int i = 0; i < RefrigeratorManager.Instance.slotSize; i++)
        {
            Item selfSlotDefaultItem = DefaultItems[i];

            // スロットインデックスがアイテム数以上の場合は空アイテムを追加
            if (i >= DefaultItems.Length)
            {
                _selfContainer.AddItem(null);
                continue;
            }

            // スロットインデックスがコンテナのアイテム数以上の場合は追加処理
            if (i >= _selfContainer.Container.Count)
            {
                _selfContainer.AddItem(selfSlotDefaultItem,
                                       // アイテムが存在するかによって状態変化
                                       (selfSlotDefaultItem == null)
                                           ? FoodState.None
                                           : FoodState.Raw);
            }
            // すでにコンテナに同一インデックスのスロットが存在するなら内容更新
            else
            {
                _selfContainer.UpdateItem(i, selfSlotDefaultItem,
                                          (selfSlotDefaultItem == null)
                                              ? FoodState.None
                                              : FoodState.Raw);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _isNear = true;
            RefrigeratorManager.Instance.currentNearObj = gameObject;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _isNear = false;
            RefrigeratorManager.Instance.currentNearObj = null;

            // インベントリが開いている時
            if (_refInvObj.activeSelf)
            {
                // 閉じる
                _refInvObj.SetActive(false);
                _selfInvRenderer.ClearRender();

                // プレイヤーインベントリ有効化
                _playerInvRenderer.EnableAllSlot();
                _playerInvRenderer.SelectSlot();
            }

            // アイテム交換モードだったら解除し、冷蔵庫をenabledに戻す
            if (TmpInventoryManager.Instance.isSwapMode)
            {
                TmpInventoryManager.Instance.isSwapMode = false;
                _selfInvRenderer.EnableAllSlot();
            }
        }
    }
}
