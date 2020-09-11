using UnityEngine;

public class GarbageCanController : MonoBehaviour
{
    private PlayerInventoryContainer _playerContainer;
    private InventoryRenderer        _playerInvRenderer;

    // プレイヤーが近くにいるか否か
    private bool _isNear;

    // Start is called before the first frame update
    private void Start()
    {
        _playerContainer   = InventoryManager.Instance.PlayerContainer;
        _playerInvRenderer = GameObject.FindGameObjectWithTag("PlayerInventory").GetComponent<InventoryRenderer>();
    }

    // Update is called once per frame
    private void Update()
    {
        // X押下で選択アイテムを捨てる
        if (_isNear &&
            Input.GetButtonDown("Interact"))
        {
            _playerContainer.RemoveItem(_playerInvRenderer.LastSelectedIndex);
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
