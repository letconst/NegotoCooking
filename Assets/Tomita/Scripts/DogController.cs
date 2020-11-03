using UnityEngine;
using UnityEngine.AI;
using System.Linq;

//オブジェクトにNavMeshAgentをコンポーネントを設置
[RequireComponent(typeof(NavMeshAgent))]

public class DogController : MonoBehaviour
{
    //列挙型で犬の状態を管理する
    public enum DogState 
    {
        Idle,
        Move,
        FindFood,
        FindPlayer,
    }

    private DogState State = DogState.Idle;

    //犬の探知範囲
    [SerializeField]
    private SphereCollider searchArea;
    //犬の視界範囲
    [SerializeField]
    private float searchAngle;
    //睡眠ゲージの減少
    [SerializeField]
    private float decreaseValue;
    //犬の動く先の目的地点
    [SerializeField]
    private Transform central;

    private Animator _animator;

    private NavMeshAgent agent;
    //ランダムで決める数値の最大値
    [SerializeField] float radius = 3;
    //設定した待機時間
    [SerializeField] float waitTime = 2;
    //待機時間を数える
    [SerializeField] float time = 0;

    private Collider nearObject;

    private bool DogMoveStop;
    private bool DogBark;
    private bool dontRay;

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
        if (State == DogState.Idle)
        {
            DogMoveStop = false;
            _animator.SetBool("Walk", false);
            _animator.SetBool("Bark", false);
            _animator.SetBool("EatFood", false);

            //吠える音声を止める
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
            //吠える音声を止める
            if (DogBark == true)
            {
                GetComponent<AudioSource>().Stop();
                DogBark = false;
            }
        }
        else if (State == DogState.FindFood)
        {
            DogMoveStop = true;
            _animator.SetBool("Walk", false);
            _animator.SetBool("Bark", false);
            _animator.SetBool("EatFood", true);
            //吠える音声を止める
            if (DogBark == true)
            {
                GetComponent<AudioSource>().Stop();
                DogBark = false;
            }
        }
        else if (State == DogState.FindPlayer)
        {
            DogMoveStop = true;
            GameManager.Instance.NoiseMator += (decreaseValue / 100) * Time.deltaTime;
            _animator.SetBool("Walk", false);
            _animator.SetBool("Bark", true);
            _animator.SetBool("EatFood", false);
            //吠える音声を流す
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
        //自分自身に目的を設定する
        if (DogMoveStop)
        {
            agent.destination = transform.position;
            agent.isStopped = true;
            return;
        }
    }

    private void NearObjectHandler()
    {
        //探知範囲にオブジェクトが存在しない場合Idle状態にする
        if (nearObject == null)
        {
            if (State == DogState.FindFood || State == DogState.FindPlayer)
            {
                State = DogState.Idle;
            }
            return;
        }
        //探知範囲に犬のおもちゃがあったら犬のおもちゃに向かう
        if (nearObject.CompareTag("DogFood"))
        {
            //餌への方向ベクトルの取得
            var dogFoodVector = ((nearObject.transform.position) - (transform.position + Vector3.up)).normalized;
            //Rayを飛ばし、リスト化する
            var dogFoodHits = Physics.RaycastAll(transform.position + Vector3.up, dogFoodVector).ToList();
            //近い順に並び替え
            dogFoodHits.Sort((a, b) => a.distance.CompareTo(b.distance));
            //餌が一番近い場合またはプレイヤー越しに餌がある場合に餌に食いつく
            if(!dogFoodHits.ElementAt(0).transform.CompareTag("DogFood") && 
                (dogFoodHits.Count < 2 || !dogFoodHits.ElementAt(0).transform.CompareTag("Player") || !dogFoodHits.ElementAt(1).transform.CompareTag("DogFood")))
            {
                if(State == DogState.FindFood)
                {
                    State = DogState.Idle;
                }
                dontRay = true;
                return;
            }

            dontRay = false;
            State = DogState.Move;
            var DogToy = nearObject.gameObject.GetComponent<DogToyController>();
            float targetPositionDistance;
            agent.isStopped = false;
            agent.destination = nearObject.transform.position;
            //経路探索が犬のおもちゃに設定する
            if (agent.pathPending)
            {
                targetPositionDistance = Vector3.Distance(transform.position, agent.destination);
            }
            //経路探索が準備されてない場合targetPositionDistanceに設定する
            else
            {
                targetPositionDistance = agent.remainingDistance;
            }
            //餌までの距離が2未満なら
            if (targetPositionDistance < 2f)
            {
                State = DogState.FindFood;
                DogMoveStop = true;
                //餌にダメージを与える
                DogToy.dogFoodHealth -= Time.deltaTime;
            }
            //犬が食べ終わったら動き出す
            if (DogToy == null)
            {
                DogMoveStop = false;
                State = DogState.Idle;
            }
        }
        //探索範囲にプレイヤーがいたらプレイヤーに向かって吠える
        else if (nearObject.CompareTag("Player"))
        {
            //プレイヤーへの方向ベクトルの取得
            var playerFindVector = ((nearObject.transform.position + Vector3.up) - (transform.position + Vector3.up)).normalized;
            //プレイヤーにRay飛ばす
            Physics.Raycast(transform.position + Vector3.up, playerFindVector, out var hit, Mathf.Infinity);
            //プレイヤーだった時のみ吠える
            if(!hit.transform.CompareTag("Player"))
            {
                if (State == DogState.FindPlayer)
                {
                    State = DogState.Idle;
                }
                return;
            }

            //主人公の方向
            var playerDirection = nearObject.transform.position - transform.position;
            //敵の前方から主人公の方向
            var angle = Vector3.Angle(transform.forward, playerDirection);
            //サーチする角度内だったら発見
            if (angle <= searchAngle)
            {
                State = DogState.FindPlayer;
            }
            else if(State == DogState.FindPlayer)
            {
                State = DogState.Idle;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        //タグが餌であり認識対象がプレイヤーまたは空なら餌を認識対象に変更する
        if (other.CompareTag("DogFood") && (nearObject==null || nearObject.CompareTag("Player")))
        {
            nearObject = other;
        }
        //タグがプレイヤーであり認識対象が空または犬の視線範囲外ならプレイヤーを認識対象に変更する
        else if (other.CompareTag("Player") && (nearObject==null || dontRay))
        {
            nearObject = other;
        }
        //タグが餌であり犬の視線範囲外なら餌を認識対象に変更する
        else if (dontRay && other.CompareTag("DogFood"))
        {
            nearObject = other;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        //認識対象が視線範囲内の餌であるとき何もしない
        if(nearObject != null && nearObject.CompareTag("DogFood") && !dontRay)
        {
            return;
        }
        //認識対象が視線範囲外であるときは何もしない
        if (other == nearObject && dontRay)
        {
            return;
        }

        if (other.CompareTag("DogFood"))
        {
            nearObject = other;
            return;
        }

        if(other.CompareTag("Player"))
        {
            nearObject = other;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        //タグがプレイヤーであり認識対象が餌であるときは何もしない
        if (other.CompareTag("Player") && nearObject.CompareTag("DogFood"))
        {
            return;
        }
        //タグがプレイヤーまたは餌であるとき空にする
        if(other.CompareTag("Player") || other.CompareTag("DogFood"))
        {
            nearObject = null;
        }
    }
}
