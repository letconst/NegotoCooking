using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rizaruto : MonoBehaviour
{
    [SerializeField]
    private Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        animator.SetTrigger("rizaruto");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
