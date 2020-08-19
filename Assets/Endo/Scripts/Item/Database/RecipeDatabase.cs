using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Recipe Database", menuName = "Inventory/Create Recipe Database")]
public class RecipeDatabase : ScriptableObject
{
    [SerializeField]
    private List<RecipeEntry> _entries = new List<RecipeEntry>();

    public List<RecipeEntry> Entries { get => _entries; private set => _entries = value; }

    /// <summary>
    /// 登録されている全レシピの名前をリストで取得する
    /// </summary>
    /// <returns>レシピ名のリスト</returns>
    public List<string> GetAllRecipeNames()
    {
        List<string> results = new List<string>();

        foreach (var entry in Entries)
        {
            results.Add(entry.RecipeName);
        }

        return results;
    }

    /// <summary>
    /// 指定した名前のレシピデータを取得する
    /// </summary>
    /// <param name="name">取得するレシピの名前</param>
    /// <returns>レシピデータ || null</returns>
    public RecipeEntry GetRecipeByName(string name)
    {
        RecipeEntry result = null;

        foreach (var entry in Entries)
        {
            if (entry.RecipeName == name)
            {
                result = entry;
                break;
            }
        }

        return result;
    }
}

[System.Serializable]
public class RecipeEntry
{
    [SerializeField]
    private string _recipeName;

    [SerializeField]
    private List<RequireFoods> _requireFoods = new List<RequireFoods>();

    public string RecipeName { get => _recipeName; }
    public List<RequireFoods> RequireFoods { get => _requireFoods; }
}

[System.Serializable]
public class RequireFoods
{
    [SerializeField]
    private Item _food;

    [SerializeField]
    private List<FoodState> _states = new List<FoodState>();

    public Item Food { get => _food; }
    public List<FoodState> States { get => _states; }
}
