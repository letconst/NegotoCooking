using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ToCooking : InventorySlotGrid
{
    private void OnTriggerStay(Collider other)
    {
        SceneManager.LoadScene("CookingScene");
    }
}
