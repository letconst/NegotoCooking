using UnityEngine;

public class gotitle : MonoBehaviour
{
    public void GoToTitle() => SceneChanger.Instance.SceneLoad(SceneChanger.SceneName.TitleScenes);
}
