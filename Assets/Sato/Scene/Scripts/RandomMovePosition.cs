using UnityEngine;

public class RandomMovePosition: MonoBehaviour
{
    //ランダムに移動する点の中心地
    public Transform centerPos;

    [SerializeField]
    float time = 0f;
    //待機時間
    [SerializeField]
    float waitTime = 2f;
    //centerPosからの半径
    [SerializeField]
    float radius = 3f;

    // Update is called once per frame
    void Update()
    {
        //時間を数える
        time += Time.deltaTime;
        //待機時間を超えたら新しいポイントを設定する
        if (time > waitTime)
        {
            RandomPosition();
            time = 0;
        }

    }

    //ランダムにポイントを生成する
    private void RandomPosition()
    {

        Vector3 ctp = centerPos.position;
        //直径の範囲内からX軸Z軸それぞれランダムにポイントを取得する
        float posX = Random.Range(-1 * radius, radius);
        float posZ = Random.Range(-1 * radius, radius);

        ctp.x += posX;
        ctp.z += posZ;
        transform.position = ctp;
    }
}
