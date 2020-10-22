﻿using System.Collections;
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
        if (_isNear                         &&
            Input.GetButtonDown("Interact") &&
            _playerContainer.GetItem(_playerInvRenderer.LastSelectedIndex) != null)
        {
            StartCoroutine(nameof(InputHandler));
        }
    }

    /// <summary>
    /// ボタン入力を処理する
    /// </summary>
    /// <returns></returns>
    private IEnumerator InputHandler()
    {
        var coroutine = _choicePopup.showWindow("食材を捨てていいですか?");

        // ボタン入力を待機
        yield return coroutine;

        // はい選択だったら食材を捨てる
        if (coroutine.Current != null &&
            (bool) coroutine.Current)
        {
            // SE再生
            SoundManager.Instance.PlaySe(SE.ThrowOutFood);

            // プレイヤーインベントリから選択食材を削除
            _playerContainer.RemoveItem(_playerInvRenderer.LastSelectedIndex);

            // 捨てた回数をカウント
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
        if (!other.CompareTag("Player")) return;

        _isNear = false;
        _choicePopup.HideWindow();
    }
}
