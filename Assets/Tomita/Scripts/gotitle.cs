using UnityEngine;

public class gotitle : MonoBehaviour
{
    public void GoToTitle()
    {
        SoundManager.Instance.PlaySe(SE.Submit);
        SceneChanger.Instance.SceneLoad(SceneChanger.SceneName.TitleScenes);
    }
}
