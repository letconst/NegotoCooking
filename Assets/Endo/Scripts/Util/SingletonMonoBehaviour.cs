using UnityEngine;

public class SingletonMonoBehaviour<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T _instance;

    public static T Instance
    {
        get
        {
            if (_instance != null) return _instance;

            _instance = (T)FindObjectOfType(typeof(T));

            if (_instance == null)
            {
                Debug.LogError(typeof(T) + "がシーンに存在しません。");
            }

            return _instance;
        }
    }

}
