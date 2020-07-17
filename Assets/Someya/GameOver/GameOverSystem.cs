using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverSystem : MonoBehaviour
{
    public void OnclickTitleButton()
    {
        SceneManager.LoadScene("TitleScenes");
    }
}
