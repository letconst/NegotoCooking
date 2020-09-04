using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

//オブジェクトにNavMeshAgentをコンポーネントを設置
[RequireComponent(typeof(NavMeshAgent))]

public class DogNav : MonoBehaviour
{
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
        agent = GetComponent<NavMeshAgent>();
        //目標地点に近づいても速度を落とさなくなる
        agent.autoBraking = false;

        //NavMeshAgentで回転しないようにする
        //agent.updateRotation = false;

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
    }

    void StopHere()
    {
        //NavMeshAgentを止める
        agent.isStopped = true;
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
        //経路探索の準備ができておらず
        //目標地点までの距離が0.5m未満ならNavMeshAgentを止める
        if (!agent.pathPending && agent.remainingDistance < 0.5f && !DogMoveStop)
        {
            StopHere();
        }
    }

    void OnTriggerEnter(Collider other)
    {
        Debug.Log("餌発見！");
        //餌のオブジェクト発見
        if (other.CompareTag("DogFood"))
        {
            //餌を目標地点に設定する
            agent.destination = other.transform.position;
        }

        //餌までの距離が0.5未満なら
        if (agent.remainingDistance < 0.5 && DogMoveStop)
        {
            Debug.Log("餌食べる");
            agent.isStopped = true;
            //時間を数える
            time += Time.deltaTime;
        }

        //犬が食べ終わったら動き出す
        if (time > eatTime)
        {
            //目標地点を設定し直す
            GotoNextPoint();
            time = 0;
            DogMoveStop = false;
        }
    }
}

//犬に視線の範囲をつけ、範囲内にプレイヤーが入ったら止まって吠える
//ある一定の範囲内に入ったら目線が向てなくても餌に向かい20秒間そこで止まる
//犬のモーション導入