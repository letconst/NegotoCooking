using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pause : MonoBehaviour
{
    //[SerializeField]
    private GameObject pauseCanvas;
    private void Start()
    {
        pauseCanvas = GameObject.FindGameObjectWithTag("PauseCanvas");
    }
    public void GoTitle()
    {
        SceneChanger.Instance.SceneLoad(SceneChanger.SceneName.TitleScenes);
    }

    public void ReturnGame()
    {
        pauseCanvas.SetActive(false);
        Time.timeScale = 1f;
    }
}
