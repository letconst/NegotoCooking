using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GarbageCanController : MonoBehaviour
{
    [SerializeField]
    private PlayerInventoryContainer playerContainer;

    private InventoryRenderer _playerInvRenderer;

    // プレイヤーが近くにいるか否か
    private bool _isNear;

    // Start is called before the first frame update
    private void Start()
    {
        playerContainer = InventoryManager.Instance.PlayerContainer;
        _playerInvRenderer = GameObject.FindGameObjectWithTag("PlayerInventory")
                                       .GetComponent<InventoryRenderer>();
    }

    // Update is called once per frame
    private void Update()
    {
        // X押下で選択アイテムを捨てる
        if (_isNear && (Input.GetKeyDown("joystick button 2") ||
                        Input.GetKeyDown(KeyCode.E)))
        {
            playerContainer.RemoveItem(_playerInvRenderer.LastSelectedIndex);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) _isNear = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player")) _isNear = false;
    }
}
