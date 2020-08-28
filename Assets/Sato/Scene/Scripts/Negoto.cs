using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Negoto : MonoBehaviour
{
    [SerializeField, Tooltip("プレイヤーオブジェクト")]
    private GameObject _playerObj;

    [SerializeField, Tooltip("寝言が表示される距離")]
    private float _showDistance = 10;

    [SerializeField, Tooltip("寝言の表示位置のXオフセット値")]
    private float _xOffset;

    [SerializeField, Tooltip("寝言の表示位置のYオフセット値")]
    private float _yOffset;

    private GameObject _negotoPanel;

    private void Start()
    {
        _negotoPanel = GameObject.FindGameObjectWithTag("Negoto").transform.parent.gameObject;

        _negotoPanel.SetActive(false);
    }

    private void Update()
    {
        // プレイヤーと師匠の距離が指定距離以内だったら寝言表示
        if (Vector3.Distance(transform.position, _playerObj.transform.position) < _showDistance)
        {
            var negotoRTrf = _negotoPanel.GetComponent<RectTransform>();

            var offset = new Vector2(Screen.width  * negotoRTrf.anchorMax.x + _xOffset,
                                     Screen.height * negotoRTrf.anchorMax.y + _yOffset);

            negotoRTrf.position = RectTransformUtility.WorldToScreenPoint(Camera.main,
                                                                          transform.position) - offset;

            _negotoPanel.SetActive(true);
        }
        else
        {
            _negotoPanel.SetActive(false);
        }
    }
}
