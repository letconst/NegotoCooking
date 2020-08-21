using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BakeController : MonoBehaviour
{
    [SerializeField]
    private GameObject FlyingPan;

    private GameObject _bakeMator;
    private GameObject _foodParent;
    private Slider     _bakeSlider;

    private PlayerInventoryContainer _playerContainer;

    // 調理が完了しているか否か
    private bool _isCompleteCooking = false;

    // 現在調理中の食材
    public static Item foodBeingBaked;

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
        CookingCompleteListener();
        FlyingPanActionHandler();
    }

    /// <summary>
    /// フライパン内の食材の調理が完了しているかを確認し、完了していたらプレイヤーインベントリに戻す
    /// </summary>
    private void CookingCompleteListener()
    {
        int               puttedSlotIndex = TmpInventoryManager.Instance.puttedSlotIndex;
        InventorySlotBase puttedSlot      = _playerContainer.Container[puttedSlotIndex];

        if (_bakeSlider.value >= 100 && !_isCompleteCooking)
        {
            _isCompleteCooking = true;
        }

        // 調理完了した食材をインベントリに戻す
        if (_isCompleteCooking && puttedSlot.Item == null)
        {
            // 出ていた食材を削除
            Destroy(_foodParent.transform.GetChild(0).gameObject);

            // プレイヤーインベントリに戻す
            _playerContainer.UpdateItem(puttedSlotIndex, foodBeingBaked, FoodState.Cooked);

            _isCompleteCooking = false;
            FireControl.clickBool = true;
            foodBeingBaked = null;
        }
    }

    /// <summary>
    /// フライパンのアクション処理
    /// </summary>
    private void FlyingPanActionHandler()
    {
        float Stick_V = Input.GetAxis("Vertical");

        if (Stick_V > 0 || Input.GetKeyDown(KeyCode.S))
        {
            if (FlyingPan.transform.position.z <= -6)
            {
                FlyingPan.transform.position = new Vector3(FlyingPan.transform.position.x, FlyingPan.transform.position.y, FlyingPan.transform.position.z);
            }

            if (FlyingPan.transform.position.z > -6)
            {
                FlyingPan.transform.position = new Vector3(FlyingPan.transform.position.x, FlyingPan.transform.position.y, FlyingPan.transform.position.z - 1.5f);
            }
        }

        if (Stick_V < 0 || Input.GetKeyDown(KeyCode.W))
        {
            if (FlyingPan.transform.position.z >= 114)
            {
                Debug.Log("114ue");
                FlyingPan.transform.position = new Vector3(FlyingPan.transform.position.x, FlyingPan.transform.position.y, FlyingPan.transform.position.z);
            }

            if (FlyingPan.transform.position.z < 114)
            {
                Debug.Log("114shita");
                FlyingPan.transform.position = new Vector3(FlyingPan.transform.position.x, FlyingPan.transform.position.y, FlyingPan.transform.position.z + 1.5f);
            }
        }
    }
}
