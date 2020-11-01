using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ChoicePopup : MonoBehaviour
{
    private CanvasGroup       canvasGroup;
    private GameObject        yesButtonObj;
    private GameObject        noButtonObj;
    private Button            yesButton;
    private Button            noButton;
    private InventoryRenderer _playerInvRenderer;

    private IEnumerator Start()
    {
        canvasGroup        = GetComponent<CanvasGroup>();
        yesButtonObj       = transform.Find("YesButton").gameObject;
        yesButton          = yesButtonObj.GetComponent<Button>();
        noButtonObj        = transform.Find("NoButton").gameObject;
        noButton           = noButtonObj.GetComponent<Button>();
        yesButton.enabled  = false;
        noButton.enabled   = false;
        _playerInvRenderer = GameObject.FindGameObjectWithTag("PlayerInventory").GetComponent<InventoryRenderer>();

        yield return null;

        canvasGroup.interactable   = false;
        canvasGroup.blocksRaycasts = false;
    }

    public IEnumerator ShowWindow(string textToDisplay, SE yesSe = SE.Submit, SE noSe = SE.Cancel)
    {
        // プレイヤーインベントリ無効化
        _playerInvRenderer.DisableAllSlot();
        // ウィンドウを表示
        canvasGroup.alpha          = 1;
        canvasGroup.interactable   = true;
        canvasGroup.blocksRaycasts = true;
        // ボタンを有効化
        yesButton.enabled = true;
        noButton.enabled  = true;
        // noButtonにフォーカス
        EventSystem.current.SetSelectedGameObject(noButtonObj);
        // ウィンドウに指定されたテキストを表示
        transform.Find("SelectText").gameObject.GetComponent<Text>().text = textToDisplay;

        GameObject currentSelected;

        // インタラクトとウィンドウの同時に開かないようにする
        yield return null;

        // A入力待機
        yield return new WaitUntil(() =>
        {
            if (!Input.GetButtonDown("Submit") ||
                PushPause.Instance.IsNowPausing) return false;

            currentSelected = EventSystem.current.currentSelectedGameObject;

            if (currentSelected == null) return false;

            // YESまたはNOボタンのみ受け付ける
            return !PushPause.Instance.IsNowPausing &&
                   (currentSelected.gameObject == yesButtonObj ||
                    currentSelected.gameObject == noButtonObj);
        });

        // 現在選択中のゲームオブジェクト
        currentSelected = EventSystem.current.currentSelectedGameObject;

        SoundManager.Instance.PlaySe((currentSelected == yesButtonObj)
                                         ? yesSe
                                         : noSe);

        // 押したボタンがYesかどうか?
        yield return currentSelected == yesButtonObj;
    }

    public void HideWindow()
    {
        // ウィンドウを非表示
        canvasGroup.alpha          = 0;
        canvasGroup.interactable   = false;
        canvasGroup.blocksRaycasts = false;
        // プレイヤーインベントリ有効化
        _playerInvRenderer.EnableAllSlot();
        _playerInvRenderer.SelectSlot();
        // ボタンを有効化
        yesButton.enabled = false;
        noButton.enabled  = false;
    }
}
