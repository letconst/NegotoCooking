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

    //焼き処理が終わったか
    public static bool bakeBool;
    //今焼き処理中か
    public static bool clickBool = true;
    [HideInInspector]
    static public bool burntBool;

    private void Start()
    {
        // 中火の色にしておく
        _fireCharImage       = fireChar.GetComponent<Image>();
        _fireCharImage.color = Color.yellow;
        GameManager.Instance.FireChange = 1;
    }

    private void Update()
    {
        if (_slider.value == 0)
        {
            burntBool = false;
            if (Input.GetKeyDown("joystick button 1") && Input.GetKeyDown(KeyCode.E))
            {
                SceneManager.LoadScene("GameScenes");
            }
        }

        float dph = Input.GetAxis("D_Pad_H");

        if (dph > 0 && GameManager.Instance.FireChange != 0)
        {
            if(GameManager.Instance.FireChange <= 0)
            {
                GameManager.Instance.FireChange = 0;
                return;
            }

            if (doOnce)
            {
                doOnce = false;
                StartCoroutine(WaitForSeconds(1.0f));
                GameManager.Instance.FireChange--;
            }
        }
        if(dph < 0 && GameManager.Instance.FireChange != 2)
        {
            if (GameManager.Instance.FireChange >= 2)
            {
                GameManager.Instance.FireChange = 2;
                return;
            }

            if (doOnce)
            {
                doOnce = false;
                StartCoroutine(WaitForSeconds(1.0f));
                GameManager.Instance.FireChange++;
            }
        }

        // 調理メーターがMAXになったら戻す
        if (_slider.value >= 100)
        {
            _slider.value = 0;
        }

        if (clickBool == true) return;

        if (GameManager.Instance.FireChange <= 0)
        {
            burntBool = false;
            leftAllow.gameObject.SetActive(false);
            _fireCharImage.color = Color.cyan;
            text.text = "弱";
            //スライダーに値を設定
            _slider.value += yowabi;
            noiseMator.fillAmount -= noiseYowabi * 0.01f;
            GameManager.Instance.NoiseMator += noiseYowabi * 0.01f;
        }
        else if (GameManager.Instance.FireChange == 1)
        {
            burntBool = true;
            leftAllow.gameObject.SetActive(true);
            rightAllow.gameObject.SetActive(true);
            _fireCharImage.color = Color.yellow;
            text.text = "中";
            _slider.value += tyubi;
            noiseMator.fillAmount -= noiseTyubi * 0.01f;
            GameManager.Instance.NoiseMator += noiseTyubi * 0.01f;
        }
        else if (GameManager.Instance.FireChange >= 2)
        {
            burntBool = true;
            rightAllow.gameObject.SetActive(false);
            _fireCharImage.color = Color.red;
            text.text = "強";
            _slider.value += tuyobi;
            noiseMator.fillAmount -= noiseTuyobi * 0.01f;
            GameManager.Instance.NoiseMator += noiseTuyobi * 0.01f;
        }
    }

    private IEnumerator WaitForSeconds(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        doOnce = true;
    }
}
