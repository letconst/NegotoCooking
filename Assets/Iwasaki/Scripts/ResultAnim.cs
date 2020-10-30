using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResultAnim : MonoBehaviour
{
    [SerializeField]
    private Animator anim;
    [SerializeField]
    private ParticleSystem kirakira;
    [SerializeField]
    private ParticleSystem good;
    [SerializeField]
    private ParticleSystem bad;
    [SerializeField]
    private GameObject emptyPlate;
    [SerializeField]
    private GameObject goodSoup;
    [SerializeField]
    private GameObject badSoup;
    [SerializeField]
    private GameObject clearTime;
    [SerializeField]
    private GameObject toTitleButton;
    [SerializeField]
    private Text clearTimeText;
    private bool isEmpty;
    void Start()
    {
        good.Stop();
        bad.Stop();
        kirakira.Stop();
        isEmpty = TimeCounter.CountUp - TimeCounter.CurrentTime == 0 || NegotoManager.Instance.CurDisplayCount != 0 || GameManager.Instance.NoiseMator == 0;
        StartCoroutine(WaitTime(2.0f));
        if(isEmpty)


        {
            emptyPlate.gameObject.SetActive(true);
        }
        else if (GameManager.Instance.FailCount == 0 || GameManager.Instance.FailCount == 1)
        {
            goodSoup.gameObject.SetActive(true);
        }
        else if (GameManager.Instance.FailCount == 2)
        {
            badSoup.gameObject.SetActive(true);
        }
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    private IEnumerator WaitTime(float waittime)
    {
        yield return new WaitForSeconds(waittime);
        anim.SetTrigger("Cloche");
        yield return new WaitForSeconds(1.0f);
        // FailCountは3もあり得るので、暫定的に2以上に
        if(isEmpty)
        {
            yield break;
        }
       else if (GameManager.Instance.FailCount                                >= 2 ||
            InventoryManager.Instance.LargePlateContainer.Container.Count <= 2) // 暫定
        {
            bad.Play();
        }
        else if(GameManager.Instance.FailCount == 0)
        {
            good.Play();
            kirakira.Play();
        }
        else if(GameManager.Instance.FailCount == 1)
        {
            good.Play();
        }
       
    }

    public void FinishAnim()
    {
        clearTimeText.text = (TimeCounter.CountUp - TimeCounter.CurrentTime).ToString("F1");
        //clearTimeText.text = "200";
        clearTime.gameObject.SetActive(true);
        toTitleButton.gameObject.SetActive(true);
    }
}
