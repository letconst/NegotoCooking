using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ChoicePopup : MonoBehaviour
{
    private CanvasGroup canvasGroup;
    private GameObject yesButtonObj;
    private GameObject noButtonObj;
    private Button yesButton;
    private Button noButton;
    private InventoryRenderer _playerInvRenderer;
    private void Start()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        yesButtonObj = transform.Find("YesButton").gameObject;
        yesButton = yesButtonObj.GetComponent<Button>();
        noButtonObj = transform.Find("NoButton").gameObject;
        noButton = noButtonObj.GetComponent<Button>();
        yesButton.enabled = false;
        noButton.enabled = false;
        _playerInvRenderer = GameObject.FindGameObjectWithTag("PlayerInventory").GetComponent<InventoryRenderer>();
    }
    public IEnumerator showWindow(string TextToDisplay)
    {
        // プレイヤーインベントリ無効化
        _playerInvRenderer.DisableAllSlot();
        // ウィンドウを表示
        canvasGroup.alpha = 1;
        // ボタンを有効化
        yesButton.enabled = true;
        noButton.enabled = true;
        // noButtonにフォーカス
        EventSystem.current.SetSelectedGameObject(noButtonObj);
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
        // プレイヤーインベントリ有効化
        _playerInvRenderer.EnableAllSlot();
        _playerInvRenderer.SelectSlot();
        // ボタンを有効化
        yesButton.enabled = false;
        noButton.enabled = false;
    }
}
