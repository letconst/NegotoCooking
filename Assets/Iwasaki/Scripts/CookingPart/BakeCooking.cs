using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BakeCooking : InventorySlot
{
    private CookingInventoryBase CIB;
    private GameObject foodIcon;

    private Item bakeItem;
    private Slider _slider;
    private GameObject foodParent;
    private GameObject foodChild;
    // Start is called before the first frame update
    void Start()
    {
        //調理ゲージ
        GameObject noiseMator = GameObject.FindGameObjectWithTag("BakeMator");
        _slider = noiseMator.GetComponent<FireControl>()._slider;
        //食料prefabの親
        foodParent = GameObject.FindGameObjectWithTag("FoodParent");

        _selfInvObj = GameObject.FindGameObjectWithTag("CookingInventory");
        CIB = _selfInvObj.GetComponent<CookingInventoryBase>();
        _selfInvSlotObjs = GameObject.FindGameObjectsWithTag("CookingInventorySlot");

        // インベントリスロットを選択
        EventSystem.current.SetSelectedGameObject(_selfInvSlotObjs[0]);
    }

    // Update is called once per frame
    void Update()
    {
        if (_slider.value >= 100 && FireControl.bakeBool ==false)
        {
            //Debug.Log("inbakeBool");
            //if (bakeBool) return;
            FireControl.bakeBool = true;
        }

        //調理完了した食材をインベントリに戻す。(空いていたら)
        BakeCooking _playerInvSlot;
        _playerInvSlot = _selfInvSlotObjs[CIB.lastPuttedSlotIndex].GetComponent<BakeCooking>();

        if (FireControl.bakeBool && _playerInvSlot.selfItem == null)
        {
            //出ていた野菜を消す。
            Destroy(foodParent.transform.GetChild(0).gameObject);
            //保持していたscriptableObjectを入れる。
            CIB.AllItems[CIB.lastPuttedSlotIndex] = bakeItem;
            ChangeFoodName(CIB, CIB.lastPuttedSlotIndex);
            FireControl.clickBool = true;
            FireControl.bakeBool = false;

            if (CIB.AllItems[CIB.lastPuttedSlotIndex] == null) return;

            CIB.container.UpdateItem(CIB.lastPuttedSlotIndex, CIB.AllItems[CIB.lastPuttedSlotIndex], FoodState.Cooked);
        }
    }

    public override void OnClick()
    {
        int selfIndex = GetSelfIndex(_selfInvSlotObjs, gameObject);
        if (FireControl.clickBool == false ||
            GetInAllItem(CIB)     == null  ||
            CIB.container.Container[selfIndex].State == FoodState.Cooked) return;

        FireControl.clickBool = false;
        foodChild = Instantiate(GetInAllItem(CIB).FoodObj, new Vector3(GetInAllItem(CIB).FoodObj.transform.position.x,
                                                           GetInAllItem(CIB).FoodObj.transform.position.y,
                                                           GetInAllItem(CIB).FoodObj.transform.position.z), Quaternion.identity);
        foodChild.transform.parent = foodParent.transform;
        bakeItem = GetInAllItem(CIB);

        // プレイヤーインベントリコンテナからアイテムを削除
        CIB.container.RemoveItem(selfIndex);
        RemoveItem(CIB);

        // 投入元のスロットインデックスを記憶
        CIB.lastPuttedSlotIndex = selfIndex;
    }
}
