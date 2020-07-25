using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class KitchenControl : MonoBehaviour
{
    public void OnClickButton()
    {
        SceneChanger.Instance.SceneLoad(SceneChanger.SceneName.Cooking_test);
    }
}
