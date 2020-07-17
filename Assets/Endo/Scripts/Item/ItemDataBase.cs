using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemDataBase", menuName = "Create Item DataBase")]
public class ItemDataBase : ScriptableObject
{
    [SerializeField]
    private List<Item> _itemLists = new List<Item>();

    public List<Item> ItemLists { get => _itemLists; }
}
