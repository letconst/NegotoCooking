using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField, Tooltip("回転スピード")]
    private float rotateSpeed;

    [SerializeField, Tooltip("Zの移動を固定する座標値")]
    private float minFrontPos;

    // プレイヤーオブジェクト
    private GameObject _player;

    // カメラの親（軸）オブジェクト
    private GameObject _wrapper;

    // カメラの角度
    private bool _isAngle;

    //回転させる角度
    private float _angle;

    private Quaternion targetrotation;

    // Start is called before the first frame update
    private void Start()
    {
        _player  = GameObject.FindGameObjectWithTag("Player");
        _wrapper = transform.parent.gameObject;
    }

    // Update is called once per frame
    private void Update()
    {
        var playerPos = _player.transform.position;
        var R_Stick   = Input.GetAxis("R_Stick_H");

        // 軸をプレイヤーに追従
        _wrapper.transform.position = playerPos.z >= minFrontPos
                                          ? playerPos
                                          : new Vector3(playerPos.x, playerPos.y, minFrontPos);

        if (R_Stick < 0 &&
            !_isAngle)
        {
            _angle -= 90;
        }
        else if (R_Stick > 0 &&
                 !_isAngle)
        {
            _angle += 90;
        }

        if (!_isAngle &&
            R_Stick != 0)
        {
            // カメラを回転させる
            targetrotation = Quaternion.AngleAxis(_angle, Vector3.up);
            _isAngle       = true;
        }

        if (R_Stick == 0)
        {
            _isAngle = false;
        }

        _wrapper.transform.rotation = Quaternion.Slerp(_wrapper.transform.rotation, targetrotation == null
                                                           ? _wrapper.transform.rotation
                                                           : targetrotation, rotateSpeed);
    }
}
