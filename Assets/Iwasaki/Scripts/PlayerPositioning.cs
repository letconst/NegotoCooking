using UnityEngine;

public class PlayerPositioning : MonoBehaviour
{
    [SerializeField]
    private GameObject playerPosition;

    private GameObject _player;

    private void Start()
    {
        Application.targetFrameRate = 60;
        _player                     = GameObject.FindGameObjectWithTag("Player");

        if (GameManager.Instance.PlayerPos == new Vector3(0, 0, 0))
        {
            Debug.Log("StartPos");
            _player.transform.position = playerPosition.transform.position;
        }
        else
        {
            Debug.Log("inCookingPos");
            _player.SetActive(false);

            _player.transform.position         = GameManager.Instance.PlayerPos;
            _player.transform.localEulerAngles = GameManager.Instance.PlayerRotate;

            _player.SetActive(true);
        }
    }
}
