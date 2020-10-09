using System.Collections;
using UnityEngine;

public class LargePlateController : MonoBehaviour
{
    // インベントリアセット
    [SerializeField]
    private InventoryContainerBase selfContainer;

    // 近くにいるか否か
    private bool _isNear;

    private GameObject               _soup;
    private GameObject               _playerInvObj;
    private PlayerInventoryContainer _playerInvContainer;
    private InventoryRenderer        _playerInvRenderer;
    private ChoicePopup              _choicePopup;

    // Start is called before the first frame update
    private void Start()
    {
        _soup               = transform.Find("Soup").gameObject;
        _playerInvObj       = GameObject.FindGameObjectWithTag("PlayerInventory");
        _playerInvContainer = InventoryManager.Instance.PlayerContainer;
        _playerInvRenderer  = _playerInvObj.GetComponent<InventoryRenderer>();
        _choicePopup        = GameObject.FindGameObjectWithTag("SelectWindow").GetComponent<ChoicePopup>();
        _soup.SetActive(false);
    }

    // Update is called once per frame
    private void Update()
    {
        if (_isNear &&
            Input.GetButtonDown("Interact"))
        {
            StartCoroutine("InputHandler");
        }

        // コンテナにアイテムが1つでも入ったらスープを表示
        if (selfContainer.Container.Count > 0) _soup.SetActive(true);
    }

    private IEnumerator InputHandler()
    {
        IEnumerator coroutine = _choicePopup.showWindow("大皿に素材を投入していいですか?");
        yield return coroutine;
        if ((bool)coroutine.Current)
        {
            var selectedFood = _playerInvContainer.GetItem(_playerInvRenderer.LastSelectedIndex);
            var selectedFoodState = _playerInvContainer.GetStates(_playerInvRenderer.LastSelectedIndex);

            // 調味料か調理済みの食材のみ受け付ける
            if (selectedFood.KindOfItem1 != Item.KindOfItem.Seasoning &&
                (selectedFoodState.Contains(FoodState.None) ||
                 selectedFoodState.Contains(FoodState.Raw))) yield break;

            var targetFoodIndex = _playerInvRenderer.LastSelectedIndex;

            // 大皿に現在選択しているアイテムをぶち込む
            selfContainer.AddSlot(_playerInvContainer.GetItem(targetFoodIndex),
                                  selectedFoodState);

            // 表示中の寝言の達成をチェック
            NegotoManager.Instance.CheckNegotoAchieved(_playerInvContainer.GetItem(targetFoodIndex));

            // プレイヤーのアイテムを削除
            _playerInvContainer.RemoveItem(targetFoodIndex);
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
