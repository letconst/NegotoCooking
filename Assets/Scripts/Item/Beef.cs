using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Beef", menuName = "New beef")]
public class Beef : Item, Usable
{
    public void Use()
    {
        Debug.Log("Used a beef!");
    }
}
