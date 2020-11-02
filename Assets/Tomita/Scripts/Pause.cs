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
        SoundManager.Instance.PlaySe(SE.Submit);
        GameManager.Instance.ResetAllValues();
        SceneChanger.Instance.SceneLoad(SceneChanger.SceneName.TitleScenes, true, .5f);
    }

    public void ReturnGame()
    {
        StartCoroutine(PushPause.Instance.ReturnToGame());
    }
}
