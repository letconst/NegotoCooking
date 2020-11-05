using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CookingManager : SingletonMonoBehaviour<CookingManager>
{
    private float timeleft;
    private float burntTimeleft;
    private bool  doOnceAlert = true;
    private bool  go100Point;
    private bool  _isNowCooking;

    public void Awake()
    {
        if (this != Instance)
        {
            Destroy(gameObject);

            return;
        }

        DontDestroyOnLoad(gameObject);
    }

    // Update is called once per frame
    private void Update()
    {
        var curSceneName = SceneManager.GetActiveScene().name;
        _isNowCooking = curSceneName == "BakeScenes" || curSceneName == "BoilScenes" || curSceneName == "CutScenes";

        // 調理中でなければ終了
        if (!_isNowCooking) return;

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
                    GameManager.Instance.BubblePoint += 15;
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
            go100Point                     = true;
            doOnceAlert                    = false;
            GameManager.Instance.alertBool = true;
        }

        if (GameManager.Instance.BubblePoint < 100 &&
            !doOnceAlert)
        {
            if (go100Point)
            {
                go100Point = false;
                SoundManager.Instance.FadeOutBgm(0.1f);
                StartCoroutine(waitTime(0.2f));
            }

            doOnceAlert                    = true;
            GameManager.Instance.alertBool = false;
        }
    }

    private IEnumerator waitTime(float waittime)
    {
        yield return new WaitForSeconds(waittime);

        SoundManager.Instance.PlayBgm(BGM.BoilSound, isLoop: true);

        yield break;
    }
}
