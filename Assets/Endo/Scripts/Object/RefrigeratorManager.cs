using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RefrigeratorManager : SingletonMonoBehaviour<RefrigeratorManager>
{
    // プレイヤーのインタラクト範囲にある冷蔵庫
    public GameObject CurrentNearObj;

    // 冷蔵庫の最大スロット数
    public int SlotSize;

    // 冷蔵庫のインベントリオブジェクト
    private GameObject _refInvObj;

    // インタラクト範囲にある冷蔵庫のコンテナ
    public RefrigeratorInventoryContainerBase NearRefrigeratorContainer
    {
        get
        {
            RefrigeratorInventoryContainerBase result = null;

            if (CurrentNearObj == null) return result;

            result = InventoryManager.Instance.RefContainers.GetContainer(CurrentNearObj.name);

            return result;
        }
    }

    // Start is called before the first frame update
    private void Start()
    {
        _refInvObj = GameObject.FindGameObjectWithTag("RefrigeratorInventory");

        _refInvObj.SetActive(false);
    }
}
