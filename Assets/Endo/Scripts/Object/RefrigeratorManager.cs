using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RefrigeratorManager : SingletonMonoBehaviour<RefrigeratorManager>
{
    // プレイヤーのインタラクト範囲にある冷蔵庫
    public GameObject currentNearObj;

    // 冷蔵庫の最大スロット数
    public int slotSize;

    // 冷蔵庫のインベントリオブジェクト
    private GameObject _refInvObj;

    // インタラクト範囲にある冷蔵庫のコンテナ
    public RefrigeratorInventoryContainerBase NearRefrigeratorContainer
    {
        get
        {
            RefrigeratorInventoryContainerBase result = null;

            if (currentNearObj == null) return result;

            result = TmpInventoryManager.Instance.refContainers.GetContainer(currentNearObj.GetInstanceID());
            
            return result;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        _refInvObj = GameObject.FindGameObjectWithTag("RefrigeratorInventory");

        _refInvObj.SetActive(false);
    }
}
