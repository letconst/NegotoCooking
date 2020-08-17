using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CutCooking : InventorySlot
{
    private CookingInventoryBase CIB;
    private GameObject foodIcon;

    private Item bakeItem;
    private Slider _slider;
    private GameObject foodParent;
    private GameObject foodChild;
    private GameObject foodPosition;
    // Start is called before the first frame update
    void Start()
    {
        //調理ゲージ
        GameObject cutMator = GameObject.FindGameObjectWithTag("BakeMator");
        _slider = cutMator.GetComponent<CutGauge>()._slider;
        //食料prefabの親
        foodParent = GameObject.FindGameObjectWithTag("FoodParent");

        _selfInvObj = GameObject.FindGameObjectWithTag("CutInventory");
        CIB = _selfInvObj.GetComponent<CookingInventoryBase>();
        _selfInvSlotObjs = GameObject.FindGameObjectsWithTag("CutInventorySlot");

        // インベントリスロットを選択
        EventSystem.current.SetSelectedGameObject(_selfInvSlotObjs[0]);

        foodPosition = GameObject.FindGameObjectWithTag("FoodPosition");
    }

    // Update is called once per frame
    void Update()
    {
        if (_slider.value >= 100 && CutGauge.cutBool == false)
        {
            //Debug.Log("inbakeBool");
            //if (bakeBool) return;
            CutGauge.cutBool = true;
        }

        //調理完了した食材をインベントリに戻す。(空いていたら)
        CutCooking _playerInvSlot;
        _playerInvSlot = _selfInvSlotObjs[CIB.lastPuttedSlotIndex].GetComponent<CutCooking>();

        if (CutGauge.cutBool && _playerInvSlot.selfItem == null)
        {
            //出ていた野菜を消す。
            Destroy(foodParent.transform.GetChild(0).gameObject);
            //保持していたscriptableObjectを入れる。
            CIB.AllItems[CIB.lastPuttedSlotIndex] = bakeItem;
            CutFoodName(CIB, CIB.lastPuttedSlotIndex);
            CutGauge.clickBool = true;
            CutGauge.cutBool = false;

            if (CIB.AllItems[CIB.lastPuttedSlotIndex] == null) return;

            CIB.container.UpdateItem(CIB.lastPuttedSlotIndex, CIB.AllItems[CIB.lastPuttedSlotIndex], FoodState.Cut);
        }
    }

    public override void OnClick()
    {
        int selfIndex = GetSelfIndex(_selfInvSlotObjs, gameObject);
        if (CutGauge.clickBool == false ||
            GetInAllItem(CIB) == null ||
            CIB.container.Container[selfIndex].State != FoodState.Raw) return;

        CutGauge.clickBool = false;
        foodChild = Instantiate(GetInAllItem(CIB).FoodObj, new Vector3(foodPosition.transform.position.x,foodPosition.transform.position.y,foodPosition.transform.position.z), 
                                                           new Quaternion(foodPosition.transform.rotation.x, foodPosition.transform.rotation.y, foodPosition.transform.rotation.z, foodPosition.transform.rotation.w));
        foodChild.transform.parent = foodParent.transform;
        bakeItem = GetInAllItem(CIB);

        // プレイヤーインベントリコンテナからアイテムを削除
        CIB.container.RemoveItem(selfIndex);
        RemoveItem(CIB);

        // 投入元のスロットインデックスを記憶
        CIB.lastPuttedSlotIndex = selfIndex;
    }
}
