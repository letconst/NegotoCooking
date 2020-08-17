using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Recipe Database", menuName = "Inventory/Create Recipe Database")]
public class RecipeDatabase : ScriptableObject
{
    [SerializeField]
    private List<RecipeEntry> _entries = new List<RecipeEntry>();

    public List<RecipeEntry> Entries { get => _entries; private set => _entries = value; }
}

[System.Serializable]
public class RecipeEntry
{
    [SerializeField]
    private string _recipeName;

    [SerializeField]
    private List<RequireFoods> _requireFoods = new List<RequireFoods>();
}

[System.Serializable]
public class RequireFoods
{
    [SerializeField]
    private Item _food;

    [SerializeField]
    private List<FoodState> _states = new List<FoodState>();
}
