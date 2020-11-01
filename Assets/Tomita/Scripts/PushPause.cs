using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class PushPause : SingletonMonoBehaviour<PushPause>
{
    private GameObject  _pauseObj;
    private GameObject  _playerInvObj;
    private GameObject  _refInvObj;
    private GameObject  _choicePopupObj;
    private GameObject  _lastSelectedObj;
    private CanvasGroup _pauseCanvasGroup;
    private CanvasGroup _playerCanvasGroup;
    private CanvasGroup _refCanvasGroup;
    private CanvasGroup _choicePopupCanvasGroup;

    private bool _isInteractablePlayerInv;
    private bool _isInteractableRefInv;
    private bool _isInteractableChoicePopup;
    private bool _isNowCookingScene;

    // 現在ポーズ中か否か
    public bool IsNowPausing { get; private set; }

    // Start is called before the first frame update
    private IEnumerator Start()
    {
        var curSceneName = SceneManager.GetActiveScene().name;
        _isNowCookingScene =
            curSceneName == "BakeScenes" || curSceneName == "BoilScenes" || curSceneName == "CutScenes";

        _playerInvObj      = GameObject.FindGameObjectWithTag("PlayerInventory");
        _pauseObj          = GameObject.FindGameObjectWithTag("PauseCanvas");
        _playerCanvasGroup = _playerInvObj.GetComponent<CanvasGroup>();
        _pauseCanvasGroup  = _pauseObj.GetComponent<CanvasGroup>();

        if (!_isNowCookingScene)
        {
            _refInvObj              = GameObject.FindGameObjectWithTag("RefrigeratorInventory");
            _choicePopupObj         = GameObject.FindGameObjectWithTag("SelectWindow");
            _refCanvasGroup         = _refInvObj.GetComponent<CanvasGroup>();
            _choicePopupCanvasGroup = _choicePopupObj.GetComponent<CanvasGroup>();
        }

        yield return null;

        _pauseCanvasGroup.interactable = false;
    }

    // Update is called once per frame
    private void Update()
    {
        if (!Input.GetButtonDown("Pause")) return;

        // ポーズ画面を開くとき
        if (_pauseCanvasGroup.alpha.Equals(0))
        {
            IsNowPausing = true;

            // 現在の全CanvasGroupのinteractable状態を記憶
            // TODO: CanvasGroupマネージャーを作って管理したい
            _isInteractablePlayerInv        = _playerCanvasGroup.interactable;
            _playerCanvasGroup.interactable = false;

            // メインシーンでは冷蔵庫と確認ウィンドウのも記憶
            if (!_isNowCookingScene)
            {
                _isInteractableRefInv      = _refCanvasGroup.interactable;
                _isInteractableChoicePopup = _choicePopupCanvasGroup.interactable;
                // 他に選択可能なボタンを一斉無効化
                _refCanvasGroup.interactable         = false;
                _choicePopupCanvasGroup.interactable = false;
            }

            // 選択していたボタンを記憶
            _lastSelectedObj = EventSystem.current.currentSelectedGameObject;

            // SE再生
            SoundManager.Instance.PlaySe(SE.OpenPause);

            // ポーズを表示して選択可能に
            _pauseCanvasGroup.alpha        = 1;
            _pauseCanvasGroup.interactable = true;

            // ザ・ワールド
            Time.timeScale = 0f;

            // ゲームに戻るを初期選択
            EventSystem.current
                       .SetSelectedGameObject(_pauseObj.transform.Find("ButtonWrapper/ReturnToGame").gameObject);
        }
        // 閉じるとき
        else
        {
            StartCoroutine(ReturnToGame());
        }
    }

    public IEnumerator ReturnToGame()
    {
        IsNowPausing                   = false;
        _pauseCanvasGroup.alpha        = 0;
        _pauseCanvasGroup.interactable = false;

        // ポーズを開く前に有効になっていたCanvasGroupを再度有効化
        if (_isInteractablePlayerInv)
        {
            _playerCanvasGroup.interactable = true;

            if (_isInteractableChoicePopup)
            {
                _choicePopupCanvasGroup.interactable = true;
            }
        }
        else if (_isInteractableRefInv)
        {
            _refCanvasGroup.interactable = true;
        }

        // ゲームに戻るボタンを押した際、確認ウィンドウが表示されているとそのまま進んでしまうため1フレーム待つ
        yield return null;

        // SE再生
        SoundManager.Instance.PlaySe(SE.Cancel);

        // ポーズ前に選択していたボタンを選択
        EventSystem.current.SetSelectedGameObject(_lastSelectedObj);

        // そして時は動き出す
        Time.timeScale = 1f;
    }
}
