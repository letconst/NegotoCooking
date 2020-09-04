using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

//オブジェクトにNavMeshAgentをコンポーネントを設置
[RequireComponent(typeof(NavMeshAgent))]


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
    
    private Animator _animator;

    public Transform central;

    private NavMeshAgent agent;
    //ランダムで決める数値の最大値
    [SerializeField] float radius = 3;
    //設定した待機時間
    [SerializeField] float waitTime = 2;
    //待機時間を数える
    [SerializeField] float time = 0;
    //餌を食べる時間
    private float eatTime = 20;

    public bool DogMoveStop;

    Vector3 pos;

    // Start is called before the first frame update
    void Start()
    {
        _animator = GetComponent<Animator>();

        agent = GetComponent<NavMeshAgent>();

        //目標地点に近づいても速度を落とさなくなる
        agent.autoBraking = false;

        //目標地点を決める
        GotoNextPoint();
    }

    void GotoNextPoint()
    {
        //NavMeshAgentのストップの解除
        agent.isStopped = false;

        //CentralPointの位置にPosXとPosZを足す
        pos = central.position;

        //NavMeshAgentに目標地点を設定する
        agent.destination = pos;

        _animator.SetBool("Walk", true);
    }

    void StopHere()
    {
        //NavMeshAgentを止める
        agent.isStopped = true;

        _animator.SetBool("Walk", false);

        //待ち時間を数える
        if (!DogMoveStop)
        {
            time += Time.deltaTime;
        }

        //待ち時間が設定された数値を超えると発動
        if (time > waitTime)
        {
            //目標地点を設定し直す
            GotoNextPoint();
            time = 0;
        }


    }

    // Update is called once per frame
    void Update()
    {
        // 検知範囲にプレイヤーがいたら吠える
        if (State == DogState.FindPlayer)
        {
            DogMoveStop = true;
            GameManager.Instance.NoiseMator += (decreaseValue / 100) * Time.deltaTime;
            _animator.SetBool("Walk", false);
            _animator.SetBool("Bark", true);
        }
        else
        {
            DogMoveStop = false;
            _animator.SetBool("Walk", true);
            _animator.SetBool("Bark", false);
        }

        //経路探索の準備ができておらず
        //目標地点までの距離が0.5m未満ならNavMeshAgentを止める
        if (!agent.pathPending && agent.remainingDistance < 0.5f && !DogMoveStop)
        {
            StopHere();
        }

        if (DogMoveStop)
        {
            agent.destination = transform.position;
            agent.isStopped = true;
            return;
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
