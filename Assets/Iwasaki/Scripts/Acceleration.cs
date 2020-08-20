using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Acceleration : MonoBehaviour
{
    [SerializeField]
    private Vector3 acceleration;	// 加速度
    // Update is called once per frame
    void Update()
    {
        // 加速度与える
        gameObject.GetComponent<Rigidbody>().AddForce(acceleration, ForceMode.Acceleration);
    }
}
