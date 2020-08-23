﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "New Recipe Database", menuName = "Inventory/Create Recipe Database")]
public class RecipeDatabase : ScriptableObject
{
    [SerializeField]
    private List<RecipeEntry> _entries = new List<RecipeEntry>();

    public List<RecipeEntry> Entries => _entries;

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
    private string _recipeName;

    [SerializeField]
    private List<RequireFoods> _requireFoods = new List<RequireFoods>();

    public string             RecipeName   => _recipeName;
    public List<RequireFoods> RequireFoods => _requireFoods;
}

[System.Serializable]
public class RequireFoods
{
    [SerializeField]
    private Item _food;

    [SerializeField]
    private List<FoodState> _states = new List<FoodState>();

    public Item Food              => _food;
    public List<FoodState> States => _states;
}
