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
    public RefrigeratorInventoryContainerBase NearRefrigeratorContainer =>
        (currentNearObj == null)
            ? null
            : InventoryManager.Instance.RefContainers.GetContainer(currentNearObj.name);

    // Start is called before the first frame update
    private void Start()
    {
        _refInvObj = GameObject.FindGameObjectWithTag("RefrigeratorInventory");

        _refInvObj.SetActive(false);
    }
}
