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
        _slider = noiseMator.transform.GetChild(0).GetComponent<Slider>();
        //食料prefabの親
        foodParent = GameObject.FindGameObjectWithTag("FoodParent");
        _selfInvObj = GameObject.FindGameObjectWithTag("CookingInventory");
        CIB = _selfInvObj.GetComponent<CookingInventoryBase>();
        _selfInvSlotObjs = GameObject.FindGameObjectsWithTag("CookingInventorySlot");
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
        for (int i = 0; i < CIB.SlotSize; i++)
        {
            if (FireControl.bakeBool && CIB.AllItems[i] == null)
            {
                //Debug.Log("inmain");
                Destroy(foodParent.transform.GetChild(0).gameObject);
                CIB.AllItems[i] = bakeItem;                
                ChangeFoodName(CIB, i);
                //CIB.AllItems[i].IsBake = true;
                FireControl.clickBool = true;
                if (CIB.AllItems[i] != null)
                {
                    CIB.AllItems[i].IsBake = true;
                }
                FireControl.bakeBool = false;
            }
        }
    }

    public override void OnClick()
    {
        if (FireControl.clickBool == false || GetInAllItem(CIB) == null || GetInAllItem(CIB).IsBake) return;

        FireControl.clickBool = false;
        foodChild = Instantiate(GetInAllItem(CIB).FoodObj, new Vector3(GetInAllItem(CIB).FoodObj.transform.position.x, 
                                                           GetInAllItem(CIB).FoodObj.transform.position.y, 
                                                           GetInAllItem(CIB).FoodObj.transform.position.z), Quaternion.identity);
        foodChild.transform.parent = foodParent.transform;
        bakeItem = GetInAllItem(CIB);
        //Debug.Log(bakeItem);
        RemoveItem(CIB);
    }
}
