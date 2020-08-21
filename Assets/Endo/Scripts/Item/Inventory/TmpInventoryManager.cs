using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TmpInventoryManager : SingletonMonoBehaviour<TmpInventoryManager>
{
    [SerializeField]
    private PlayerInventoryContainer        _playerContainer;
    [SerializeField]
    private InventoryContainerBase          _largePlateContainer;
    [SerializeField]
    private RefrigeratorInventoryContainers _refContainers;
    [SerializeField]
    private RecipeDatabase                  _recipeDatabase;

    // 交換用: アイテム交換モードか否か
    [System.NonSerialized]
    public bool isSwapMode = false;
    // 交換用: 冷蔵庫アイテムのキャッシュ
    [System.NonSerialized]
    public Item itemToSwapFromRef;
    // 交換用: 冷蔵庫アイテムの状態のキャッシュ
    [System.NonSerialized]
    public FoodState itemStateToSwap;

    // 調理用: 食材を投入したスロットインデックス
    [System.NonSerialized]
    public int puttedSlotIndex;

    public PlayerInventoryContainer        PlayerContainer     { get => _playerContainer; }
    public InventoryContainerBase          LargePlateContainer { get => _largePlateContainer; }
    public RefrigeratorInventoryContainers RefContainers       { get => _refContainers; }
    public RecipeDatabase                  RecipeDatabase      { get => _recipeDatabase; }

    private void Awake()
    {
        if (this != Instance)
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);
    }

    private void OnApplicationQuit()
    {
        // 終了時にコンテナ消去
        PlayerContainer.Container.Clear();
        RefContainers.RefInvContainers.Clear();
        LargePlateContainer.Container.Clear();
    }
}
