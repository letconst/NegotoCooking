using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CutController : MonoBehaviour
{
    private GameObject _cutMator;
    private GameObject _foodParent;
    private Slider     _cutSlider;

    private PlayerInventoryContainer _playerContainer;

    // 調理が完了しているか否か
    private bool _isCompleteCooking = false;

    // 現在調理中の食材
    public static Item foodBeingCut;

    // Start is called before the first frame update
    void Start()
    {
        _cutMator   = GameObject.FindGameObjectWithTag("BakeMator");
        _foodParent = GameObject.FindGameObjectWithTag("FoodParent");
        _cutSlider  = _cutMator.GetComponent<CutGauge>()._slider;

        _playerContainer = TmpInventoryManager.Instance.PlayerContainer;
    }

    // Update is called once per frame
    void Update()
    {
        CookingCompleteListener();
    }

    /// <summary>
    /// 食材の調理が完了しているかを確認し、完了していたらプレイヤーインベントリに戻す
    /// </summary>
    private void CookingCompleteListener()
    {
        int puttedSlotIndex = TmpInventoryManager.Instance.puttedSlotIndex;
        InventorySlotBase puttedSlot = _playerContainer.Container[puttedSlotIndex];

        if (_cutSlider.value >= 100 && !_isCompleteCooking)
        {
            _isCompleteCooking = true;
        }

        // 調理完了した食材をインベントリに戻す
        if (_isCompleteCooking && puttedSlot.Item == null)
        {
            // 出ていた食材を削除
            Destroy(_foodParent.transform.GetChild(0).gameObject);

            // プレイヤーインベントリに戻す
            _playerContainer.UpdateItem(puttedSlotIndex, foodBeingCut, FoodState.Cut);

            _isCompleteCooking = false;
            CutGauge.clickBool = true;
            foodBeingCut = null;
        }
    }
}
