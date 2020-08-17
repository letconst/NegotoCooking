using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TmpInventoryManager : SingletonMonoBehaviour<TmpInventoryManager>
{
    public PlayerInventoryContainer        playerContainer;
    public InventoryContainerBase          largePlateContainer;
    public RefrigeratorInventoryContainers refContainers;
    public Item                            itemToSwapFromRef;
    public FoodState                       itemStateToSwap;

    // アイテム交換モードか否か
    [System.NonSerialized]
    public bool isSwapMode = false;

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
        playerContainer.Container.Clear();
        refContainers.RefInvContainers.Clear();
        largePlateContainer.Container.Clear();
    }
}
