using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemDataBase", menuName = "Create Item DataBase")]
public class ItemDatabase : ScriptableObject
{
    [SerializeField]
    private List<Item> itemLists = new List<Item>();

    public List<Item> ItemLists => itemLists;
}
