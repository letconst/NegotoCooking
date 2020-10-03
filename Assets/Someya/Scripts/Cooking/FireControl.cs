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
    private bool doOnceFire = true;

    //焼き処理が終わったか
    public static bool bakeBool;
    //今焼き処理中か
    public static bool clickBool = true;
    [HideInInspector]
    static public bool burntBool;
    private bool doOnceBakeSound = true;
    [SerializeField]
    private ParticleSystem fireS;
    [SerializeField]
    private ParticleSystem fireM;
    [SerializeField]
    private ParticleSystem fireL;

    private void Start()
    {
        // 中火の色にしておく
        _fireCharImage       = fireChar.GetComponent<Image>();
        _fireCharImage.color = Color.yellow;
        GameManager.Instance.FireChange = 1;
        fireS.Play();
        fireL.Play();
    }

    private void Update()
    {
        if (_slider.value == 0)
        {
            burntBool = false;
            if (Input.GetKeyDown("joystick button 1") || Input.GetKeyDown(KeyCode.E))
            {
                SceneManager.LoadScene("GameScenes");
            }
        }

        float dph = Input.GetAxis("D_Pad_H");

        if (dph < 0 && GameManager.Instance.FireChange != 0)
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
                doOnceFire = true;
            }
        }
        if(dph > 0 && GameManager.Instance.FireChange != 2)
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
                doOnceFire = true;
            }
        }

        // 調理メーターがMAXになったら戻す
        if (_slider.value >= 100)
        {
            _slider.value = 0;
            doOnceBakeSound = true;
            //焼き調理音をフェードアウトさせる。
            SoundManager.Instance.FadeOutBgm(0.2f);
        }

        if (clickBool == true) return;

        if (doOnceBakeSound)
        {
            doOnceBakeSound = false;
            SoundManager.Instance.PlayBgm(BGM.BakeSound);
        }
        
        if (GameManager.Instance.FireChange <= 0)
        {
            if (doOnceFire)
            {
                doOnceFire = false;
                fireM.Stop();
                fireL.Stop();
                fireS.Play();
            }
            burntBool = false;
            leftAllow.gameObject.SetActive(false);
            _fireCharImage.color = Color.cyan;
            text.text = "弱";
            //スライダーに値を設定
            _slider.value += yowabi;
            GameManager.Instance.NoiseMator += noiseYowabi * 0.01f;
        }
        else if (GameManager.Instance.FireChange == 1)
        {
            if (doOnceFire)
            {
                doOnceFire = false;
                fireS.Stop();
                fireL.Stop();
                fireM.Play();
            }
            burntBool = true;
            leftAllow.gameObject.SetActive(true);
            rightAllow.gameObject.SetActive(true);
            _fireCharImage.color = Color.yellow;
            text.text = "中";
            _slider.value += tyubi;
            GameManager.Instance.NoiseMator += noiseTyubi * 0.01f;
        }
        else if (GameManager.Instance.FireChange >= 2)
        {
            if (doOnceFire)
            {
                doOnceFire = false;
                fireS.Stop();
                fireM.Stop();
                fireL.Play();
            }
            burntBool = true;
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
