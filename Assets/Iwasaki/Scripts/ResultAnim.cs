using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    private int hyouka = 1;
    [SerializeField]
    private GameObject goodSoup;
    [SerializeField]
    private GameObject badSoup;
    [SerializeField]
    private GameObject clearTime;
    [SerializeField]
    private GameObject toTitleButton;
    void Start()
    {
        good.Stop();
        bad.Stop();
        kirakira.Stop();
        StartCoroutine(WaitTime(2.0f));
        if (hyouka == 1 || hyouka == 2)
        {
            goodSoup.gameObject.SetActive(true);
        }
        else if (hyouka == 3)
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
        if (hyouka == 1)
        {
            good.Play();
            kirakira.Play();
        }
        else if(hyouka == 2)
        {
            good.Play();
        }
        else if(hyouka == 3)
        {
            bad.Play();
        }
        yield break;
    }   
    public void FinishAnim()
    {
        clearTime.gameObject.SetActive(true);
        toTitleButton.gameObject.SetActive(true);
    }
}
