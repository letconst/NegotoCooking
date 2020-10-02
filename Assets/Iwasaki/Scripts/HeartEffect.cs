using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeartEffect : MonoBehaviour
{
    [SerializeField]
    private Animator animator;
    [SerializeField]
    private ParticleSystem heartEffect;
    [SerializeField]
    private GameObject heartPos;
    private bool doOnce = true;
    private void Start()
    {
        heartEffect.Stop();
    }
    void Update()
    {
        if(animator.GetCurrentAnimatorStateInfo(0).IsName("Base Layer.EatFood") == true && doOnce)
        {
            Debug.Log("effectPlay");
            doOnce = false;
            heartEffect.transform.position = new Vector3(heartPos.transform.position.x, heartPos.transform.position.y, heartPos.transform.position.z);
            heartEffect.Play();
        }
        else if(animator.GetCurrentAnimatorStateInfo(0).IsName("Base Layer.EatFood") == false && !doOnce)
        {
            Debug.Log("effectStop");
            doOnce = true;
            heartEffect.Stop();
        }
    }
}
