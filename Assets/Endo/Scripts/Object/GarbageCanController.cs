using System.Collections;
using UnityEngine;

public class GarbageCanController : MonoBehaviour
{
    private PlayerInventoryContainer _playerContainer;
    private InventoryRenderer        _playerInvRenderer;
    private ChoicePopup              _choicePopup;

    // プレイヤーが近くにいるか否か
    private bool _isNear;

    // Start is called before the first frame update
    private void Start()
    {
        _playerContainer   = InventoryManager.Instance.PlayerContainer;
        _playerInvRenderer = GameObject.FindGameObjectWithTag("PlayerInventory").GetComponent<InventoryRenderer>();
        _choicePopup       = GameObject.FindGameObjectWithTag("SelectWindow").GetComponent<ChoicePopup>();
    }

    // Update is called once per frame
    private void Update()
    {
        // X押下で選択アイテムを捨てる
        if (_isNear &&
            Input.GetButtonDown("Interact")&&
            _playerContainer.GetItem(_playerInvRenderer.LastSelectedIndex)!=null)
        {
            StartCoroutine("InputHandler");
        }
    }

    private IEnumerator InputHandler()
    {
        IEnumerator coroutine = _choicePopup.showWindow("食材を捨ててもよろしいでしょうか?");
        yield return coroutine;
        if((bool) coroutine.Current)
        {
            _playerContainer.RemoveItem(_playerInvRenderer.LastSelectedIndex);
            GameManager.Instance.StatisticsManager.throwInCount++;
        }
        _choicePopup.HideWindow();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) _isNear = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _isNear = false;
            _choicePopup.HideWindow();
        }
    }
}
