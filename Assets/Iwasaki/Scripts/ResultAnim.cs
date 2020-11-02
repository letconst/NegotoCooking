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
    private GameObject goodRating;

    [SerializeField]
    private GameObject normalRating;

    [SerializeField]
    private GameObject badRating;

    [SerializeField]
    private GameObject goodHukidashi;

    [SerializeField]
    private GameObject normalHukidashi;

    [SerializeField]
    private GameObject clearTime;

    [SerializeField]
    private GameObject toTitleButton;

    [SerializeField]
    private Text clearTimeText;

    // 大皿が空か否か
    private bool _isEmpty;

    private void Start()
    {
        good.Stop();
        bad.Stop();
        kirakira.Stop();
        _isEmpty = InventoryManager.Instance.LargePlateContainer.Container.Count == 0;

        StartCoroutine(WaitTime(2.0f));

        if (_isEmpty)
        {
            emptyPlate.SetActive(true);

            return;
        }

        Debug.Log(GameManager.Instance.FailCount);
        switch (GameManager.Instance.FailCount)
        {
            // 評価：良い
            case 0:
            // 評価：普通
            case 1:
                goodSoup.SetActive(true);

                break;

            // 評価：悪い
            case int count when count >= 2:
                badSoup.SetActive(true);

                break;
        }
    }

    private IEnumerator WaitTime(float waittime)
    {
        yield return new WaitForSeconds(waittime);

        anim.SetTrigger("Cloche");

        if (_isEmpty)
        {
            yield break;
        }

        yield return new WaitForSeconds(3.0f);

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

        if (_isEmpty) return;

        switch (GameManager.Instance.FailCount)
        {
            // 評価：良い
            case 0:
                goodHukidashi.SetActive(true);
                goodRating.SetActive(true);

                break;

            // 評価：普通
            case 1:
                normalHukidashi.SetActive(true);
                normalRating.SetActive(true);

                break;

            // 評価：悪い
            case int count when count >= 2:
                normalHukidashi.SetActive(true);
                badRating.SetActive(true);

                break;
        }
    }
}
