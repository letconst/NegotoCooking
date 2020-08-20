using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BoilController : MonoBehaviour
{
    [SerializeField, Tooltip("お玉のオブジェクト")]
    private GameObject _ladle;
    [SerializeField, Tooltip("鍋の中心座標のオブジェクト")]
    private GameObject _potCenterPos;

    private GameObject _bakeMator;
    private GameObject _foodParent;
    private Slider     _bakeSlider;

    private PlayerInventoryContainer _playerContainer;

    // 調理が完了しているか否か
    private bool _isCompleteCooking = false;

    // Start is called before the first frame update
    void Start()
    {
        _bakeMator = GameObject.FindGameObjectWithTag("BoilMator");
        _foodParent = GameObject.FindGameObjectWithTag("FoodParent");
        _bakeSlider = _bakeMator.GetComponent<FireControl>()._slider;

        _playerContainer = TmpInventoryManager.Instance.PlayerContainer;
    }

    // Update is called once per frame
    void Update()
    {
        CookingCompleteListener();
        PotActionHandler();
    }

    /// <summary>
    /// 鍋内の食材の調理が完了しているかを確認し、完了していたらプレイヤーインベントリに戻す
    /// </summary>
    private void CookingCompleteListener()
    {
        if (_bakeSlider.value >= 100 && !_isCompleteCooking)
        {
            _isCompleteCooking = true;
        }

        int               puttedSlotIndex = TmpInventoryManager.Instance.puttedSlotIndex;
        InventorySlotBase puttedSlot      = _playerContainer.Container[puttedSlotIndex];

        // 調理完了した食材をインベントリに戻す
        if (_isCompleteCooking && puttedSlot.Item == null)
        {
            // 出ていた食材を削除
            Destroy(_foodParent.transform.GetChild(0).gameObject);

            // プレイヤーインベントリに戻す
            _playerContainer.UpdateItem(puttedSlotIndex, FireControl.foodInProgress, FoodState.Boil);

            _isCompleteCooking = false;
            FireControl.clickBool = true;
            FireControl.foodInProgress = null;
        }
    }

    /// <summary>
    /// 鍋のアクション処理
    /// </summary>
    private void PotActionHandler()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        Debug.Log(h + "," + v);
        _ladle.transform.position = new Vector3(_potCenterPos.transform.position.x + h * 65,
                                                _potCenterPos.transform.position.y,
                                                _potCenterPos.transform.position.z + v * 70);
    }
}
