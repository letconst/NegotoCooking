using UnityEngine;

public class Pause : MonoBehaviour
{
    private GameObject  _pauseCanvas;
    private CanvasGroup _pauseCanvasGroup;
    private CanvasGroup _playerCanvasGroup;

    private void Start()
    {
        _pauseCanvas      = GameObject.FindGameObjectWithTag("PauseCanvas");
        _pauseCanvasGroup = _pauseCanvas.GetComponent<CanvasGroup>();
        _playerCanvasGroup = GameObject.FindGameObjectWithTag("PlayerInventory")
                                       .GetComponent<CanvasGroup>();
    }

    public void GoTitle()
    {
        _pauseCanvasGroup.alpha = 0;
        Time.timeScale          = 1f;
        SceneChanger.Instance.SceneLoad(SceneChanger.SceneName.TitleScenes);
    }

    public void ReturnGame()
    {
        StartCoroutine(PushPause.Instance.ReturnToGame());
    }
}
