using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : SingletonMonoBehaviour<SceneChanger>
{
    public static Item[] playerInv;
    private GameObject _playerInv;
    [SerializeField]
    private float waittime = 1;
    private string[] refTagName =
    {
        "Refrigerator_1",
        "Refrigerator_2",
    };

    public enum SceneName
    {
        TitleScenes = 0,
        Cooking_test,
        CookingScenes,
        GameOverScenes,
        GameScenes,

    }
    public void Awake()
    {
        if (this != Instance)
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);
    }
    private void Start()
    {
        _playerInv = GameObject.FindGameObjectWithTag("PlayerInventory");        
        
        //for(int i = 0; i < refTagName.Length; i++)
        //{
        //    GameObject _ref = GameObject.FindGameObjectWithTag(refTagName[i]);
        //    _refInv = _ref.
        //}        
    }
    //連打対策
    private bool doOnceSceneChange = true;

    public void SceneLoad(SceneName sceneName)
    {
        if (doOnceSceneChange == false) return;

        if (doOnceSceneChange)
        {
            StartCoroutine(LoadSceneCor(sceneName));
        }
    }

    private IEnumerator LoadSceneCor(SceneName sceneName)
    {
        doOnceSceneChange = false;
        var async = SceneManager.LoadSceneAsync(sceneName.ToString());
        async.allowSceneActivation = false;
        //for (int i = 0; i < _playerInv.GetComponent<PlayerInventory>().AllItems.Length - 1; i++)
        //{
        //    playerInv[i] = _playerInv.GetComponent<PlayerInventory>().AllItems[i];
        //}
        yield return new WaitForSeconds(waittime);
        doOnceSceneChange = true;
        async.allowSceneActivation = true;
    }
}
