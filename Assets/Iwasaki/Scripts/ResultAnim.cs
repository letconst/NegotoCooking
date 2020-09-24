﻿using System.Collections;
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
    private GameObject goodSoup;
    [SerializeField]
    private GameObject badSoup;
    [SerializeField]
    private GameObject clearTime;
    [SerializeField]
    private GameObject toTitleButton;
    [SerializeField]
    private Text clearTimeText;
    void Start()
    {
        good.Stop();
        bad.Stop();
        kirakira.Stop();
        StartCoroutine(WaitTime(2.0f));
        if (GameManager.Instance.FailCount == 0 || GameManager.Instance.FailCount == 1)
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
        if (GameManager.Instance.FailCount == 0)
        {
            good.Play();
            kirakira.Play();
        }
        else if(GameManager.Instance.FailCount == 1)
        {
            good.Play();
        }
        else if(GameManager.Instance.FailCount == 2)
        {
            bad.Play();
        }
        yield break;
    }   
    public void FinishAnim()
    {
        clearTimeText.text = (400 - TimeCounter.CurrentTime).ToString(); 
        //clearTimeText.text = "200";
        clearTime.gameObject.SetActive(true);
        toTitleButton.gameObject.SetActive(true);
    }
}