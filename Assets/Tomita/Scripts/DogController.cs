﻿using System.Collections;
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
        Move,
        FindFood,
        FindPlayer,
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

    [SerializeField] float time2 = 0;

    //餌を食べる時間
    private float eatTime = 20;

    private Collider nearObject;

    public bool DogMoveStop;
    public bool DogBark;

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
        State = DogState.Move;
        //NavMeshAgentのストップの解除
        agent.isStopped = false;

        //CentralPointの位置にPosXとPosZを足す
        pos = central.position;

        //NavMeshAgentに目標地点を設定する
        agent.destination = pos;
    }

    void StopHere()
    {
        //NavMeshAgentを止める
        agent.isStopped = true;

        State = DogState.Idle;

    }

    // Update is called once per frame
    void Update()
    {
        // Debug.Log(DogBark);
        // 検知範囲にプレイヤーがいたら吠える

        if (State == DogState.Idle)
        {
            DogMoveStop = false;
            _animator.SetBool("Walk", false);
            _animator.SetBool("Bark", false);
            _animator.SetBool("EatFood", false);
            if(DogBark==true)
            {
                GetComponent<AudioSource>().Stop();
                DogBark = false;
            }

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
        else if (State == DogState.Move)
        {
            DogMoveStop = false;
            _animator.SetBool("Walk", true);
            _animator.SetBool("Bark", false);
            _animator.SetBool("EatFood", false);
        }
        else if (State == DogState.FindFood)
        {
            DogMoveStop = true;
            _animator.SetBool("Walk", false);
            _animator.SetBool("Bark", false);
            _animator.SetBool("EatFood", true);
        }
        else if (State == DogState.FindPlayer)
        {
            DogMoveStop = true;
            GameManager.Instance.NoiseMator += (decreaseValue / 100) * Time.deltaTime;
            _animator.SetBool("Walk", false);
            _animator.SetBool("Bark", true);
            _animator.SetBool("EatFood", false);
            if(DogBark==false)
            {
                GetComponent<AudioSource>().PlayDelayed(0.5f);
                DogBark = true;
            }
        }
        else
        {
            DogMoveStop = false;
            _animator.SetBool("Walk", true);
            _animator.SetBool("Bark", false);
            _animator.SetBool("EatFood", false);
        }

        NearObjectHandler();

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

    private void NearObjectHandler()
    {
        if (nearObject == null)
        {
            if (State == DogState.FindFood || State == DogState.FindPlayer)
            {
                State = DogState.Idle;
            }
            return;
        }

        if (nearObject.CompareTag("DogFood"))
        {
            var DogToy = nearObject.gameObject.GetComponent<DogToyController>();
            float targetPositionDistance;
            //Debug.Log(DogToy.isFoundDogFood);

            agent.destination = nearObject.transform.position;
            Debug.Log(gameObject.name + " " + agent.destination);

            if (agent.pathPending)
            {
                targetPositionDistance = Vector3.Distance(transform.position, agent.destination);
            }
            else
            {
                targetPositionDistance = agent.remainingDistance;
            }
            //餌までの距離が0.5未満なら
            if (targetPositionDistance < 2f)
            {
                State = DogState.FindFood;
                // Debug.Log("餌食べる");
                DogMoveStop = true;
                //時間を数える
                //time2 += Time.deltaTime;
                DogToy.dogFoodHealth -= Time.deltaTime;
            }
            //犬が食べ終わったら動き出す
            if (DogToy.dogFoodHealth <= 0)
            {
                //time2 = 0;

                DogMoveStop = false;
                if (nearObject.gameObject != null)
                {
                    Destroy(nearObject.gameObject);
                }
                State = DogState.Idle;
            }
        }
        else if (nearObject.CompareTag("Player"))
        {
            //主人公の方向
            var playerDirection = nearObject.transform.position - transform.position;
            //敵の前方から主人公の方向
            var angle = Vector3.Angle(transform.forward, playerDirection);
            //サーチする角度内だったら発見
            if (angle <= searchAngle)
            {
                //GetComponent<AudioSource>().Play();
                //Debug.Log("主人公発見");
                State = DogState.FindPlayer;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (nearObject == null && (other.CompareTag("Player") || other.CompareTag("DogFood")))
        {
            nearObject = other;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && nearObject.CompareTag("DogFood"))
        {
            return;
        }

        if(other.CompareTag("Player")||other.CompareTag("DogFood"))
        {
            nearObject = null;
        }
    }
}
