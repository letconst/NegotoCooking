using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class KitchenControl : MonoBehaviour
{
    public void OnClickButton()
    {
        SceneManager.LoadScene("Player");        
    }
}
