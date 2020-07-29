using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InputController : SingletonMonoBehaviour<InputController>
{
    // 最後に選択していたボタン
    private GameObject _lastSelectedObj;

    private void Awake()
    {
        if (this != Instance)
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        // ボタンのフォーカスが外れている時、フォーカスを復活させる
        if (EventSystem.current.currentSelectedGameObject == null)
        {
            // 十字ボタンの入力があるときだけ実行
            if (Input.GetAxis("D_Pad_V") == 0 && Input.GetAxis("D_Pad_H") == 0) return;

            EventSystem.current.SetSelectedGameObject(_lastSelectedObj);
        }
        else
        {
            _lastSelectedObj = EventSystem.current.currentSelectedGameObject;
        }
    }
}
