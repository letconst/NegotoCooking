using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPositioning : MonoBehaviour
{
    private GameObject player;
    [SerializeField]
    private GameObject playerPosition;
    void Start()
    {
        Application.targetFrameRate = 60;
        player = GameObject.FindGameObjectWithTag("Player");
        if (GameManager.Instance.PlayerPos == new Vector3 (0,0,0))
        {
            Debug.Log("StartPos");
            player.transform.position = playerPosition.transform.position;            
        }
        else
        {
            Debug.Log("inCookingPos");
            player.transform.position = GameManager.Instance.PlayerPos;
            player.transform.localEulerAngles = GameManager.Instance.PlayerRotate;
        }        
    }
}
