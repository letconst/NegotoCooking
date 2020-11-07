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

    // 現在調理中の食材（スロット渡し）
    public static InventorySlotBase FoodSlotBeingCut;

    [SerializeField]
    private GameObject operationCanvas;

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
        // ポーズ中は進行および操作させない
        if (PushPause.Instance.IsNowPausing) return;

        if (operationCanvas.activeSelf == true) return;

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

        // 食材の状態を更新し、プレイヤーインベントリに戻す
        FoodSlotBeingCut.RemoveState(FoodState.Raw);
        FoodSlotBeingCut.AddState(FoodState.Cut);
        _playerContainer.AddItem(FoodSlotBeingCut.Item, FoodSlotBeingCut.States);

        _isCompleteCooking    = false;
        CutGauge.clickBool    = true;
        CutGauge.cantBackBool = true;
        FoodSlotBeingCut      = null;
    }

    /// <summary>
    /// 全ゲームシーンで共有される値をリセット
    /// </summary>
    public static void ResetValues()
    {
        FoodSlotBeingCut = null;
    }
}
