using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : SingletonMonoBehaviour<InventoryManager>
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
    public bool IsSwapMode = false;
    // 交換用: 冷蔵庫アイテムのキャッシュ
    [System.NonSerialized]
    public Item ItemToSwapFromRef;
    // 交換用: 冷蔵庫アイテムの状態のキャッシュ
    [System.NonSerialized]
    public FoodState ItemStateToSwap;

    // 調理用: 食材を投入したスロットインデックス
    [System.NonSerialized]
    public int PuttedSlotIndex;

    public PlayerInventoryContainer        PlayerContainer     => _playerContainer;
    public InventoryContainerBase          LargePlateContainer => _largePlateContainer;
    public RefrigeratorInventoryContainers RefContainers       => _refContainers;
    public RecipeDatabase                  RecipeDatabase      => _recipeDatabase;

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
