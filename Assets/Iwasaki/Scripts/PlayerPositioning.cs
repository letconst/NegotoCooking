using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPositioning : MonoBehaviour
{
    [SerializeField]
    private GameObject player;
    [HideInInspector]
    public static Vector3 lastPlayerPos = new Vector3(25,2,0);
    void Start()
    {
        player.transform.position = lastPlayerPos;
    }
}
