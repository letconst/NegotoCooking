using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : SingletonMonoBehaviour<InventoryManager>
{
    [SerializeField]
    private PlayerInventoryContainer playerContainer;

    [SerializeField]
    private InventoryContainerBase largePlateContainer;

    [SerializeField]
    private RefrigeratorInventoryContainers refContainers;

    [SerializeField]
    private RecipeDatabase recipeDatabase;

    [SerializeField]
    private ItemDatabase itemDatabase;

    // 交換用: アイテム交換モードか否か
    [System.NonSerialized]
    public bool IsSwapMode = false;

    // 交換用: 冷蔵庫アイテムのキャッシュ
    [System.NonSerialized]
    public Item ItemToSwapFromRef;

    // 交換用: 冷蔵庫アイテムの状態のキャッシュ
    [System.NonSerialized]
    public List<FoodState> ItemStatesToSwap;

    // 調理用: 食材を投入したスロットインデックス
    [System.NonSerialized]
    public int PuttedSlotIndex;

    public PlayerInventoryContainer        PlayerContainer     => playerContainer;
    public InventoryContainerBase          LargePlateContainer => largePlateContainer;
    public RefrigeratorInventoryContainers RefContainers       => refContainers;
    public RecipeDatabase                  RecipeDatabase      => recipeDatabase;

    public ItemDatabase ItemDatabase { get => itemDatabase; }

    private void Awake()
    {
        if (this != Instance)
        {
            Destroy(gameObject);

            return;
        }

        DontDestroyOnLoad(gameObject);

        PlayerContainer.DogFoodCount = playerContainer.MaxDogFoodCount;
    }
}
