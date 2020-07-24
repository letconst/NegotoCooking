using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MasterController : SingletonMonoBehaviour<MasterController>
{
    // 調理済か否か（デバッグ用、本来は調理パートに持たせる）
    [SerializeField]
    private bool _isCooked = false;

    // 近くにいるか否か
    private bool _isNear = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // X押下および接近時に調理完了を行う
        if (Input.GetKeyDown("joystick button 2") && _isNear)
        {
            // 調理できていればゲームクリア
            if (_isCooked)
            {
                // ゲームクリアシーン読み込み
            }
            // 調理できていなければゲームオーバー
            else
            {
                SceneManager.LoadScene("GameOverScenes");
            }
        }
    }

    /// <summary>
    /// 接近時にフラグを立てる
    /// </summary>
    private void OnTriggerEnter()
    {
        _isNear = true;
    }

    /// <summary>
    /// 接近時にフラグを消す
    /// </summary>
    private void OnTriggerExit()
    {
        _isNear = false;
    }
}
