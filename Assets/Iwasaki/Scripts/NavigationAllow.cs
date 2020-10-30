using System;
using UnityEngine;
using System.Collections;

public class NavigationAllow : SingletonMonoBehaviour<NavigationAllow>
{
    [SerializeField]
    private Transform target_Sisyou;

    [SerializeField]
    private Transform target_Steps;

    [SerializeField]
    private Transform target_Kitchen;

    // カーソル
    [SerializeField]
    private Transform cursor;

    private GameManager _gameManager;

    private void Start()
    {
        _gameManager = GameManager.Instance;

        // すでにキッチン到達まで終えていれば矢印は表示しない
        if (_gameManager.IsReachedNavOfKitchen) cursor.gameObject.SetActive(false);
    }

    // Update is called once per frame
    private void Update()
    {
        if (_gameManager.IsReachedNavOfKitchen) return;

        if (!_gameManager.IsReachedNavOfNegoto)
        {
            cursor.LookAt(target_Sisyou);
        }
        else if (!_gameManager.IsReachedNavOfStairs)
        {
            cursor.LookAt(target_Steps);
        }
        else if (!_gameManager.IsReachedNavOfKitchen)
        {
            cursor.LookAt(target_Kitchen);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (_gameManager.IsReachedNavOfKitchen) return;

        if (other.gameObject.CompareTag("Nav_Sisyou"))
        {
            _gameManager.IsReachedNavOfNegoto = true;
        }

        // 階段以降は寝言を見ていないと次の目的地は更新されない
        if (other.gameObject.CompareTag("Nav_Steps") &&
            _gameManager.IsReachedNavOfNegoto)
        {
            _gameManager.IsReachedNavOfStairs = true;
        }

        if (other.gameObject.CompareTag("Nav_Kitchen") &&
            _gameManager.IsReachedNavOfNegoto)
        {
            _gameManager.IsReachedNavOfKitchen = true;
            cursor.gameObject.SetActive(false);
        }
    }
}
