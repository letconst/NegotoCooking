using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetInventory : MonoBehaviour
{
    private GameObject _playerInv;
    // Start is called before the first frame update
    void Start()
    {
        _playerInv = GameObject.FindGameObjectWithTag("PlayerInventory");
        for (int i = 0; i < _playerInv.GetComponent<PlayerInventory>().AllItems.Length; i++)
        {
            _playerInv.GetComponent<PlayerInventory>().AllItems[i] = SceneChanger.playerInv[i];
        }
    }
}
