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

        //目標地点のX軸、Z軸をランダムで決める
        float posX = Random.Range(-1 * radius, radius);
        float posZ = Random.Range(-1 * radius, radius);

        //CentralPointの位置にPosXとPosZを足す
        pos = central.position;

        pos.x += posX;
        pos.z += posZ;

        ////Y軸だけ変更しない目標地点
        //Vector3 direction = new Vector3(pos.x, transform.position.y, pos.z);

        ////Y軸だけ変更しない目標地点から現在地を引いて向きを割り出す
        //Quaternion rotation = Quaternion.LookRotation(direction-transform.position, Vector3.up);

        ////このオブジェクトの向きを変える
        //transform.rotation = rotation;

        //NavMeshAgentに目標地点を設定する
        agent.destination = pos;
    }

    void StopHere()
    {
        //NavMeshAgentを止める
        agent.isStopped = true;
        //待ち時間を数える
        time += Time.deltaTime;

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
        if (!agent.pathPending && agent.remainingDistance < 0.5f)
        {
            StopHere();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Vector3 dogFood = GameObject.Find("DogFood").transform.position;
        agent.destination = dogFood;
        
    }
}

//犬に視線の範囲をつけ、範囲内にプレイヤーが入ったら止まって吠える
//ある一定の範囲内に入ったら目線が向てなくても餌に向かい20秒間そこで止まる
//犬のモーション導入
//2秒ごとにcentralpositionをランダムな位置に移動させる