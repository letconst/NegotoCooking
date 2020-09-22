using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "Create Item")]
public class Item : ScriptableObject
{
    public enum KindOfItem
    {
        Seasoning, // 調味料
        Food       // 食材
    }

    // アイテムの種類
    [SerializeField]
    private KindOfItem kindOfItem;

    // アイテムアイコン
    [SerializeField]
    private Sprite itemIcon;

    // アイテム名
    [SerializeField]
    private string itemName;

    // 食材3Dオブジェクト
    [SerializeField]
    private GameObject foodObj;

    public KindOfItem KindOfItem1 => kindOfItem;
    public Sprite     ItemIcon    => itemIcon;
    public GameObject FoodObj     => foodObj;
    public string     ItemName    => itemName;
}
