using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResultAnim : MonoBehaviour
{
    [SerializeField]
    private Animator anim;
    [SerializeField]
    private ParticleSystem good;
    [SerializeField]
    private ParticleSystem bad;
    private int hyouka = 2;
    [SerializeField]
    private GameObject goodSoup;
    [SerializeField]
    private GameObject badSoup;
    void Start()
    {
        good.Stop();
        bad.Stop();
        StartCoroutine(WaitTime(2.0f));
        if (hyouka == 1)
        {
            goodSoup.gameObject.SetActive(true);
        }
        else if (hyouka == 2)
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
        anim.SetTrigger("ResultAnim");
        yield return new WaitForSeconds(1.0f);
        if (hyouka == 1)
        {
            good.Play();
        }
        else if(hyouka == 2)
        {
            bad.Play();
        }
        yield break;
    }   
}
