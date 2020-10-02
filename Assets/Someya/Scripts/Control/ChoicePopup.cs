using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ChoicePopup : SingletonMonoBehaviour<ChoicePopup>
{
    private CanvasGroup canvasGroup;
    private void Start()
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }
    public IEnumerator showWindow(string TextToDisplay)
    {
        // ウィンドウを表示
        canvasGroup.alpha = 1;
        // ウィンドウに指定されたテキストを表示
        transform.Find("SelectText").gameObject.GetComponent<Text>().text = TextToDisplay;
        // A入力待機
        yield return new WaitUntil(() => Input.GetButtonDown("Submit"));
        // 現在選択中のゲームオブジェクト
        GameObject currentSelected = EventSystem.current.currentSelectedGameObject;
        // 押したボタンがYesかどうか?
        yield return currentSelected.name == "YesButton";
    }
    public void HideWindow()
    {
        // ウィンドウを非表示
        canvasGroup.alpha = 0;
    }
}
