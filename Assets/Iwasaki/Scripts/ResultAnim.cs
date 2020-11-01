using System.Collections;
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
        isEmpty = InventoryManager.Instance.LargePlateContainer.Container.Count == 0;

        StartCoroutine(WaitTime(2.0f));

        if (isEmpty)
        {
            emptyPlate.gameObject.SetActive(true);
        }
        else
        {
            switch (GameManager.Instance.FailCount)
            {
                case 0:
                case 1:
                    goodSoup.gameObject.SetActive(true);

                    break;

                case int count when count >= 2:
                    badSoup.gameObject.SetActive(true);

                    break;
            }
        }
    }

    private IEnumerator WaitTime(float waittime)
    {
        yield return new WaitForSeconds(waittime);

        anim.SetTrigger("Cloche");

        yield return new WaitForSeconds(1.0f);

        if (isEmpty)
        {
            yield break;
        }

        switch (GameManager.Instance.FailCount)
        {
            // 評価：良い
            case 0:
                good.Play();
                kirakira.Play();

                break;

            // 評価：普通
            case 1:
                good.Play();

                break;

            // 評価：悪い
            case int count when count >= 2:
                bad.Play();

                break;
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
