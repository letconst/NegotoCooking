using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorEnabled : MonoBehaviour
{
    public void animatorEnabled()
    {
        this.gameObject.GetComponent<Animator>().enabled = false;
    }
    
}
