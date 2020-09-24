using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CookingManager : SingletonMonoBehaviour<CookingManager>
{
    private float timeleft; 
    private float burntTimeleft;
    private bool doOnceAlert = true;
    public void Awake()
    {
        if (this != Instance)
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (FireControl_boil.bubbleBool)
        {
            timeleft -= Time.deltaTime;
            if (timeleft <= 0.0)
            {
                //Debug.Log("PotAction");
                timeleft = 1.0f;
                //中だと1秒で10ポイント増える。強だと1秒で25ポイント。100ポイント溜まってしまうと泡があふれて警報が鳴る。
                if (GameManager.Instance.FireChange == 1)
                {
                    GameManager.Instance.BubblePoint += 10;
                }
                else if (GameManager.Instance.FireChange == 2)
                {
                    GameManager.Instance.BubblePoint += 25;
                }
            }
        }

        if (FireControl.burntBool)
        {            
            burntTimeleft -= Time.deltaTime;
            if (burntTimeleft <= 0.0)
            {
                burntTimeleft = 1.0f;
                if (GameManager.Instance.FireChange == 1)
                {
                    GameManager.Instance.BakePoint += 5;
                }
                else if (GameManager.Instance.FireChange == 2)
                {
                    GameManager.Instance.BakePoint += 10;
                }
            }
        }

        if (GameManager.Instance.BubblePoint == 100 && doOnceAlert)
        {            
            SoundManager.Instance.PlayBgm(BGM.Alert);            
            doOnceAlert = false;
            GameManager.Instance.alertBool = true;
        }

        if (GameManager.Instance.BubblePoint < 100 && !doOnceAlert)
        {
            SoundManager.Instance.FadeOutBgm(1.0f);            
            doOnceAlert = true;
            GameManager.Instance.alertBool = false;
            SoundManager.Instance.PlayBgm(BGM.BoilSound);
        }
    }
}
