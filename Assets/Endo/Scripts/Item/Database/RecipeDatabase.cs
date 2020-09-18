using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "New Recipe Database", menuName = "Inventory/Create Recipe Database")]
public class RecipeDatabase : ScriptableObject
{
    [SerializeField]
    private List<RecipeEntry> entries = new List<RecipeEntry>();

    public List<RecipeEntry> Entries => entries;

    /// <summary>
    /// 登録されている全レシピの名前をリストで取得する
    /// </summary>
    /// <returns>レシピ名のリスト</returns>
    public List<string> GetAllRecipeNames()
    {
        return Entries.Select(entry => entry.RecipeName).ToList();
    }

    /// <summary>
    /// 指定した名前のレシピデータを取得する
    /// </summary>
    /// <param name="name">取得するレシピの名前</param>
    /// <returns>レシピデータ || null</returns>
    public RecipeEntry GetRecipeByName(string name)
    {
        return Entries.FirstOrDefault(entry => entry.RecipeName == name);
    }
}

[System.Serializable]
public class RecipeEntry
{
    [SerializeField]
    private string recipeName;

    [SerializeField]
    private List<RequireFoods> requireFoods = new List<RequireFoods>();

    public string             RecipeName   => recipeName;
    public List<RequireFoods> RequireFoods => requireFoods;
}

[System.Serializable]
public class RequireFoods
{
    [SerializeField]
    private Item food;

    [SerializeField]
    private HashSet<FoodState> states = new HashSet<FoodState>();

    public Item            Food   => food;
    public HashSet<FoodState> States => states;
}
