using System.Collections;
using UnityEngine;

public class RefrigeratorManager : SingletonMonoBehaviour<RefrigeratorManager>
{
    [SerializeField, Tooltip("冷蔵庫の最大スロット数")]
    private int slotSize;

    // プレイヤーのインタラクト範囲にある冷蔵庫
    public GameObject currentNearObj;

    // 冷蔵庫のインベントリオブジェクト
    private GameObject _refInvObj;

    // インタラクト範囲にある冷蔵庫のコンテナ
    public RefrigeratorInventoryContainerBase NearRefrigeratorContainer =>
        (currentNearObj == null)
            ? null
            : InventoryManager.Instance.RefContainers.GetContainer(currentNearObj.name);

    public int SlotSize => slotSize;

    // Start is called before the first frame update
    private IEnumerator Start()
    {
        _refInvObj = GameObject.FindGameObjectWithTag("RefrigeratorInventory");

        // 同一フレーム内でinteractableをfalseにしても何故か反映されないので1フレーム待つ
        yield return null;

        _refInvObj.GetComponent<CanvasGroup>().interactable = false;
    }
}
