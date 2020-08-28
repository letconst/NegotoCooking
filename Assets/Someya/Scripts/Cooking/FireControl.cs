using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class FireControl : MonoBehaviour
{
    //火加減ごとの値
    [SerializeField]
    private float yowabi;
    [SerializeField]
    private float tyubi;
    [SerializeField]
    private float tuyobi;
    //ノイズメーターの値
    [SerializeField]
    private float noiseYowabi;
    [SerializeField]
    private float noiseTyubi;
    [SerializeField]
    private float noiseTuyobi;
    //火の強さを出すテキスト
    [SerializeField]
    private Text text;
    //焼き終わるまでのメーター
    public Slider _slider;
    //騒音のメーター
    [SerializeField]
    private Image noiseMator;

    [SerializeField]
    private GameObject fireChar;
    [SerializeField]
    private GameObject leftAllow;
    [SerializeField]
    private GameObject rightAllow;

    private Image _fireCharImage;

    private bool doOnce = true;
    private int fireChange = 1;

    //焼き処理が終わったか
    public static bool bakeBool;
    //今焼き処理中か
    public static bool clickBool = true;

    private bool actionBool;
    private bool actionBool2;

    private void Start()
    {
        // 中火の色にしておく
        _fireCharImage       = fireChar.GetComponent<Image>();
        _fireCharImage.color = Color.yellow;
    }

    private void Update()
    {
        if (Input.GetKeyDown("joystick button 1") && _slider.value == 0)
        {
            SceneManager.LoadScene("GameScenes");
        }

        float dph = Input.GetAxis("D_Pad_H");

        if (dph > 0 && fireChange != 0)
        {
            if(fireChange <= 0)
            {
                fireChange = 0;
                return;
            }

            if (doOnce)
            {
                doOnce = false;
                StartCoroutine(WaitForSeconds(1.0f));
                fireChange--;
            }
        }
        if(dph < 0 && fireChange != 2)
        {
            if (fireChange >= 2)
            {
                fireChange = 2;
                return;
            }

            if (doOnce)
            {
                doOnce = false;
                StartCoroutine(WaitForSeconds(1.0f));
                fireChange++;
            }
        }

        // 調理メーターがMAXになったら戻す
        if (_slider.value >= 100)
        {
            _slider.value = 0;
        }

        if (clickBool == true) return;

        if (fireChange <= 0)
        {
            leftAllow.gameObject.SetActive(false);
            _fireCharImage.color = Color.cyan;
            text.text = "弱";
            //スライダーに値を設定
            _slider.value += yowabi;
            GameManager.Instance.NoiseMator += noiseYowabi * 0.01f;
        }
        else if (fireChange == 1)
        {
            leftAllow.gameObject.SetActive(true);
            rightAllow.gameObject.SetActive(true);
            _fireCharImage.color = Color.yellow;
            text.text = "中";
            _slider.value += tyubi;
            GameManager.Instance.NoiseMator += noiseTyubi * 0.01f;
        }
        else if (fireChange >= 2)
        {
            rightAllow.gameObject.SetActive(false);
            _fireCharImage.color = Color.red;
            text.text = "強";
            _slider.value += tuyobi;
            GameManager.Instance.NoiseMator += noiseTuyobi * 0.01f;
        }
    }

    private IEnumerator WaitForSeconds(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        doOnce = true;
    }
}
