using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BakeController : MonoBehaviour
{
    private GameObject _bakeMator;
    private GameObject _foodParent;
    private Slider     _bakeSlider;

    private PlayerInventoryContainer _playerContainer;

    // Start is called before the first frame update
    void Start()
    {
        _bakeMator  = GameObject.FindGameObjectWithTag("BakeMator");
        _foodParent = GameObject.FindGameObjectWithTag("FoodParent");
        _bakeSlider = _bakeMator.GetComponent<FireControl>()._slider;

        _playerContainer = TmpInventoryManager.Instance.PlayerContainer;
    }

    // Update is called once per frame
    void Update()
    {
        if (_bakeSlider.value >= 100 && !FireControl.bakeBool)
        {
            FireControl.bakeBool = true;
        }

        int               puttedSlotIndex = TmpInventoryManager.Instance.puttedSlotIndex;
        InventorySlotBase puttedSlot      = _playerContainer.Container[puttedSlotIndex];

        // 調理完了した食材をインベントリに戻す
        if (FireControl.bakeBool && puttedSlot.Item == null)
        {
            // 出ていた食材を削除
            Destroy(_foodParent.transform.GetChild(0).gameObject);

            // プレイヤーインベントリに戻す
            _playerContainer.UpdateItem(puttedSlotIndex, FireControl.foodInProgress, FoodState.Cooked);

            FireControl.clickBool      = true;
            FireControl.bakeBool       = false;
            FireControl.foodInProgress = null;
        }
    }
}
