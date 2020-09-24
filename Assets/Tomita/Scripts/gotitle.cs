using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class gotitle : MonoBehaviour
{
    
    // Start is called before the first frame update
   
    public void OnClickStartButton()
    {
        SceneManager.LoadScene("TitleScenes");
    }
}
