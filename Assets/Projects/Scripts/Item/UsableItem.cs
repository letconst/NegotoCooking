using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class UsableItem : Item
{
    /// <summary>
    /// 所持アイテムを使用した際の処理
    /// </summary>
    public abstract void Use();
}
