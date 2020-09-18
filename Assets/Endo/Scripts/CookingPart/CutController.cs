using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CutController : MonoBehaviour
{
    private GameObject _cutMeter;
    private GameObject _foodParent;
    private Slider     _cutSlider;

    private PlayerInventoryContainer _playerContainer;

    // 調理が完了しているか否か
    private bool _isCompleteCooking;

    // 現在調理中の食材
    public static Item FoodBeingCut;

    // 現在調理中の食材の状態
    public static HashSet<FoodState> FoodStatesBeingCut;

    // Start is called before the first frame update
    private void Start()
    {
        _cutMeter   = GameObject.FindGameObjectWithTag("BakeMator");
        _foodParent = GameObject.FindGameObjectWithTag("FoodParent");
        _cutSlider  = _cutMeter.GetComponent<CutGauge>()._slider;

        _playerContainer = InventoryManager.Instance.PlayerContainer;
    }

    // Update is called once per frame
    private void Update()
    {
        CookingCompleteListener();
    }

    /// <summary>
    /// 食材の調理が完了しているかを確認し、完了していたらプレイヤーインベントリに戻す
    /// </summary>
    private void CookingCompleteListener()
    {
        int puttedSlotIndex          = InventoryManager.Instance.PuttedSlotIndex;
        InventorySlotBase puttedSlot = _playerContainer.Container[puttedSlotIndex];

        if (_cutSlider.value >= 100 && !_isCompleteCooking)
        {
            _isCompleteCooking = true;
        }

        // 調理が完了していないか、投入元のスロットにアイテムがある場合は動作しない
        if (!_isCompleteCooking || puttedSlot.Item != null) return;

        // 出ていた食材を削除
        Destroy(_foodParent.transform.GetChild(0).gameObject);

        // 食材に状態を付加し、プレイヤーインベントリに戻す
        FoodStatesBeingCut.Add(FoodState.Cut);
        _playerContainer.AddItem(FoodBeingCut, FoodStatesBeingCut);

        _isCompleteCooking = false;
        CutGauge.clickBool = true;
        CutGauge.cantBackBool = true;
        FoodBeingCut = null;
        FoodStatesBeingCut = null;
    }
}
