using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StairsWarp1Fooler : MonoBehaviour
{
    [SerializeField]
    public Vector3 pos;

    private void OnTriggerEnter(Collider other)
    {
        // 階段に触れたら移動
        other.gameObject.SetActive(false);
        other.gameObject.transform.position = pos;
        other.gameObject.SetActive(true);
    }
}
