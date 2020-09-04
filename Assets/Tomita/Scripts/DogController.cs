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
    [SerializeField]
    private float decreaseValue;
    private DogNav dogNav;
    private Animator _animator;

    // Start is called before the first frame update
    void Start()
    {
        dogNav = gameObject.GetComponent<DogNav>();
        _animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        // 検知範囲にプレイヤーがいたら吠える
        if (State == DogState.FindPlayer)
        {
            dogNav.DogMoveStop = true;
            GameManager.Instance.NoiseMator += (decreaseValue / 100) * Time.deltaTime;
            _animator.SetBool("Walk", false);
            _animator.SetBool("Bark", true);
        }
        else
        {
            dogNav.DogMoveStop = false;
            _animator.SetBool("Walk", true);
            _animator.SetBool("Bark", false);
        }
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
            else
            {
                State = DogState.Idle;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            State = DogState.Idle;
        }
    }
}
