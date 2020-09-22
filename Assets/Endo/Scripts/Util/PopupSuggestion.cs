using UnityEngine;
using UnityEngine.UI;

public class PopupSuggestion : MonoBehaviour
{
    [SerializeField, Tooltip("インタラクトをするボタンの名称（一文字）")]
    private string interactButton;

    [SerializeField, Tooltip("インタラクトによって起こる動作の名称")]
    private string interactName;

    [SerializeField, Tooltip("Xのオフセット値")]
    private float xOffset = 0;

    [SerializeField, Tooltip("Yのオフセット値")]
    private float yOffset = 100;

    [SerializeField, Tooltip("ポップアップUIのプレハブ")]
    private GameObject popupPrefab;

    private GameObject _popupObj;

    // Start is called before the first frame update
    private void Start()
    {
        _popupObj = Instantiate(popupPrefab, transform);

        var trfWrapper = _popupObj.transform.Find("Wrapper");
        var popupPos   = trfWrapper.position;

        // 表示位置設定
        popupPos = new Vector3(popupPos.x + xOffset,
                               popupPos.y + yOffset);

        trfWrapper.position = popupPos;

        // 文字列設定
        trfWrapper.Find("InteractButton/ButtonName").GetComponent<Text>().text = interactButton;
        trfWrapper.Find("InteractName").GetComponent<Text>().text              = interactName;

        _popupObj.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) _popupObj.SetActive(true);
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player")) _popupObj.SetActive(false);
    }
}
