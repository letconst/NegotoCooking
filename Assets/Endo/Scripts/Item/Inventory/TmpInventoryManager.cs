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

    [System.NonSerialized]
    public Item itemToSwapFromRef;
    [System.NonSerialized]
    public FoodState itemStateToSwap;

    // アイテム交換モードか否か
    [System.NonSerialized]
    public bool isSwapMode = false;

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
