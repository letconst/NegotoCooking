using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RefrigeratorController : MonoBehaviour
{
    [SerializeField, Tooltip("冷蔵庫内に初期配置させるアイテム")]
    private List<DefaultItems> defaultItems = new List<DefaultItems>();

    // 近くにいるか否か
    private bool _isNear;

    private GameObject                         _playerInvObj;
    private GameObject                         _refInvObj;
    private CanvasGroup                        _playerCanvasGroup;
    private CanvasGroup                        _selfCanvasGroup;
    private InventoryRenderer                  _playerInvRenderer;
    private InventoryRenderer                  _selfInvRenderer;
    private RefrigeratorInventoryContainers    _refContainers;
    private RefrigeratorInventoryContainerBase _selfContainer;

    public List<DefaultItems> DefaultItems => defaultItems;

    // Start is called before the first frame update
    private void Start()
    {
        _playerInvObj      = GameObject.FindGameObjectWithTag("PlayerInventory");
        _refInvObj         = GameObject.FindGameObjectWithTag("RefrigeratorInventory");
        _playerCanvasGroup = _playerInvObj.GetComponent<CanvasGroup>();
        _selfCanvasGroup   = _refInvObj.GetComponent<CanvasGroup>();
        _playerInvRenderer = _playerInvObj.GetComponent<InventoryRenderer>();
        _selfInvRenderer   = _refInvObj.GetComponent<InventoryRenderer>();
        _refContainers     = InventoryManager.Instance.RefContainers;

        // コンテナデータ作成
        CreateContainer();
    }

    // Update is called once per frame
    private void Update()
    {
        InteractHandler();
    }

    /// <summary>
    /// インタラクト時の処理
    /// </summary>
    private void InteractHandler()
    {
        // X押下でインベントリ開閉
        if (!_isNear ||
            !Input.GetButtonDown("Interact")) return;

        // 開閉切り替え
        _selfCanvasGroup.alpha = (_selfCanvasGroup.alpha == 0)
                                     ? 1
                                     : 0;

        // 冷蔵庫インベントリの描画を一度初期化
        _selfInvRenderer.ClearRender();

        // 開いたとき
        if (_selfCanvasGroup.alpha.Equals(1))
        {
            // SE再生
            SoundManager.Instance.PlaySe(SE.RifregeratorOpen);
            
            // 冷蔵庫インベントリを有効化し、フォーカス
            _selfCanvasGroup.interactable = true;
            _selfInvRenderer.SelectSlot();

            // プレイヤーインベントリを無効化
            _playerCanvasGroup.interactable = false;
        }
        // 閉じたとき
        else
        {
            // 冷蔵庫インベントリを無効化
            _selfInvRenderer.UnhighlightSlotAt(_selfInvRenderer.LastSelectedIndex);
            _selfCanvasGroup.interactable = false;

            // プレイヤーインベントリを有効化し、フォーカス
            _playerCanvasGroup.interactable = true;
            _playerInvRenderer.SelectSlot();
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

        for (var i = 0; i < RefrigeratorManager.Instance.SlotSize; i++)
        {
            var selfSlotDefaultItem = (DefaultItems.Count != 0)
                                          ? DefaultItems[i].Item
                                          : null;

            // スロットインデックスがアイテム数以上の場合は空アイテムを追加
            if (i >= DefaultItems.Count)
            {
                _selfContainer.AddItem(null);

                continue;
            }

            // スロットインデックスがコンテナのアイテム数以上の場合は追加処理
            if (i >= _selfContainer.Container.Count)
            {
                _selfContainer.AddItem(selfSlotDefaultItem,
                                       // アイテムが存在するかによって状態変化
                                       (selfSlotDefaultItem != null)
                                           ? DefaultItems[i].State
                                           : new List<FoodState>()
                                           {
                                               FoodState.None
                                           });
            }
            // すでにコンテナに同一インデックスのスロットが存在するなら内容更新
            else
            {
                _selfContainer.UpdateItem(i, selfSlotDefaultItem,
                                          (selfSlotDefaultItem != null)
                                              ? DefaultItems[i].State
                                              : new List<FoodState>()
                                              {
                                                  FoodState.None
                                              });
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        _isNear                                     = true;
        RefrigeratorManager.Instance.currentNearObj = gameObject;
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        _isNear                                     = false;
        RefrigeratorManager.Instance.currentNearObj = null;

        // 冷蔵庫が開いている時
        if (_selfCanvasGroup.alpha.Equals(1))
        {
            var selfIndex = _selfInvRenderer.LastSelectedIndex;
            // 閉じる
            _selfCanvasGroup.alpha        = 0;
            _selfCanvasGroup.interactable = false;
            _selfInvRenderer.ClearRender();

            // 交換モードだったときの表示をリセット
            _selfInvRenderer.GetSlotAt(selfIndex).GetComponent<Button>().enabled = true;
            _selfInvRenderer.UnhighlightSlotAt(selfIndex);

            // プレイヤーインベントリ有効化
            // _playerInvRenderer.EnableAllSlot();
            _playerCanvasGroup.interactable = true;
            _playerInvRenderer.SelectSlot();
        }

        // アイテム交換モードだったら解除する
        if (!InventoryManager.Instance.IsSwapMode) return;

        InventoryManager.Instance.IsSwapMode = false;
    }
}
