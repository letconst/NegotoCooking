using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopupSuggestion : MonoBehaviour
{
    [SerializeField, Tooltip("インタラクトをするボタンの名称（一文字）")]
    private string _interactButton;

    [SerializeField, Tooltip("インタラクトによって起こる動作の名称")]
    private string _interactName;

    [SerializeField, Tooltip("Xのオフセット値")]
    private float _xOffset = 0;

    [SerializeField, Tooltip("Yのオフセット値")]
    private float _yOffset = 100;

    [SerializeField, Tooltip("ポップアップUIのプレハブ")]
    private GameObject _popupPrefab;

    private GameObject _popupObj;

    // Start is called before the first frame update
    private void Start()
    {
        _popupObj = Instantiate(_popupPrefab, transform);

        var trfWrapper = _popupObj.transform.Find("Wrapper");

        // 表示位置設定
        trfWrapper.position = new Vector3(trfWrapper.position.x + _xOffset,
                                          trfWrapper.position.y + _yOffset);

        // 文字列設定
        trfWrapper.Find("InteractButton/ButtonName").GetComponent<Text>().text = _interactButton;
        trfWrapper.Find("InteractName").GetComponent<Text>().text              = _interactName;

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
