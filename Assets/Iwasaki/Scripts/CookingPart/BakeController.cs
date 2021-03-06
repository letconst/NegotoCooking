﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BakeController : MonoBehaviour
{
    [SerializeField]
    private GameObject FlyingPan;

    private GameObject _bakeMeter;
    private GameObject _foodParent;
    private Slider     _bakeSlider;
    private int flyingpanTimes = 0;
    private float timeleft;
    [HideInInspector]
    public static bool doOnceBakeSound = true;
    private bool doOnce = true;
    [SerializeField]
    private ParticleSystem blackSmoke;

    private PlayerInventoryContainer _playerContainer;

    // 調理が完了しているか否か
    private bool _isCompleteCooking;

    // 現在調理中の食材（スロット渡し）
    public static InventorySlotBase FoodSlotBeingBaked;

    [SerializeField]
    private GameObject operationCanvas;

    // Start is called before the first frame update
    private void Start()
    {
        blackSmoke.Stop();
        _bakeMeter  = GameObject.FindGameObjectWithTag("BakeMator");
        _foodParent = GameObject.FindGameObjectWithTag("FoodParent");
        _bakeSlider = _bakeMeter.GetComponent<FireControl>()._slider;

        _playerContainer = InventoryManager.Instance.PlayerContainer;
    }

    // Update is called once per frame
    private void Update()
    {
        // ポーズ中は進行および操作させない
        if (PushPause.Instance.IsNowPausing) return;

        if (operationCanvas.activeSelf == true) return;

        CookingCompleteListener();
        FlyingPanActionHandler();
        blackSmoke.transform.position = new Vector3(FlyingPan.transform.position.x, FlyingPan.transform.position.y, FlyingPan.transform.position.z);
    }

    /// <summary>
    /// フライパン内の食材の調理が完了しているかを確認し、完了していたらプレイヤーインベントリに戻す
    /// </summary>
    private void CookingCompleteListener()
    {
        int               puttedSlotIndex = InventoryManager.Instance.PuttedSlotIndex;
        InventorySlotBase puttedSlot      = _playerContainer.Container[puttedSlotIndex];

        if (_bakeSlider.value >= 100 && !_isCompleteCooking)
        {
            _isCompleteCooking = true;
        }

        if (GameManager.Instance.BakePoint == 100)
        {
            BakeController.doOnceBakeSound = true;
            Destroy(_foodParent.transform.GetChild(0).gameObject);
            _bakeSlider.value = 0;
            GameManager.Instance.BakePoint = 0;
            SoundManager.Instance.FadeOutBgm(0.1f);
            FireControl.clickBool = true;
            _playerContainer.RemoveItem(puttedSlotIndex);
            blackSmoke.Stop();
        }
        if (GameManager.Instance.BakePoint >= 70 && doOnce)
        {
            doOnce = false;
            blackSmoke.Play();
        }
        else if(GameManager.Instance.BakePoint < 70 && !doOnce)
        {
            doOnce = true;
            blackSmoke.Stop();
        }

        // 調理が完了していないか、投入元のスロットにアイテムがある場合は動作しない
        if (!_isCompleteCooking || puttedSlot.Item != null) return;

        // 出ていた食材を削除
        Destroy(_foodParent.transform.GetChild(0).gameObject);

        // 食材の状態を更新し、プレイヤーインベントリに戻す
        FoodSlotBeingBaked.RemoveState(FoodState.Raw);
        FoodSlotBeingBaked.AddState(FoodState.Cooked);
        _playerContainer.AddItem(FoodSlotBeingBaked.Item, FoodSlotBeingBaked.States);

        _isCompleteCooking    = false;
        FireControl.clickBool = true;
        FoodSlotBeingBaked    = null;
    }

    /// <summary>
    /// フライパンのアクション処理
    /// </summary>
    private void FlyingPanActionHandler()
    {
        float Stick_V = Input.GetAxis("R_Stick_V");

        if (Stick_V != 0)
        {
            timeleft -= Time.deltaTime;
            if (timeleft <= 0.0)
            {
                timeleft = 2.0f;
            }

            if (flyingpanTimes >= 6)
            {
                flyingpanTimes = 6;
            }

            float value = Mathf.Floor(flyingpanTimes / 2);

            switch (value)
            {
                case 1:
                    GameManager.Instance.BakePoint -= 2;
                    break;
                case 2:
                    GameManager.Instance.BakePoint -= 5;
                    break;
                case 3:
                    GameManager.Instance.BakePoint -= 10;
                    break;
            }
        }

        if (Stick_V > 0 || Input.GetKeyDown(KeyCode.S))
        {
            if (FlyingPan.transform.position.z > -10)
            {
                FlyingPan.transform.position = new Vector3(FlyingPan.transform.position.x, FlyingPan.transform.position.y, FlyingPan.transform.position.z - 3.5f);
                flyingpanTimes++;
            }
        }

        if (Stick_V < 0 || Input.GetKeyDown(KeyCode.W))
        {
            if (FlyingPan.transform.position.z < 130)
            {
                FlyingPan.transform.position = new Vector3(FlyingPan.transform.position.x, FlyingPan.transform.position.y, FlyingPan.transform.position.z + 3.5f);
                flyingpanTimes++;
            }
        }
    }

    /// <summary>
    /// 全ゲームシーンで共有される値をリセット
    /// </summary>
    public static void ResetValues()
    {
        doOnceBakeSound    = true;
        FoodSlotBeingBaked = null;
    }
}
