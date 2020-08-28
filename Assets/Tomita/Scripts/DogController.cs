using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DogController : MonoBehaviour
{
    public enum DogState
    {
        Idle,
        FindFood,
        FindPlayer
    }
    private DogState State = DogState.Idle;
    private bool isNearPlayer;
    [SerializeField]
    private SphereCollider searchArea;
    [SerializeField]
    private float searchAngle;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            //主人公の方向
            var playerDirection = other.transform.position - transform.position;
            //敵の前方から主人公の方向
            var angle = Vector3.Angle(transform.forward, playerDirection);
            //サーチする角度内だったら発見
            if(angle<=searchAngle)
            {
                Debug.Log("主人公発見");
                State = DogState.FindPlayer;
            }
        }
    }
}
