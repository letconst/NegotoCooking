using UnityEngine;

public class OperationConfirmation : MonoBehaviour
{
    [SerializeField]
    private GameObject operationCanvas;
    private bool notFirst;
    void Update()
    {
        //最初の一回は必ず操作確認を表示。次回からは任意で表示できる。
        if (GameManager.Instance.operationBool && Input.GetKeyDown("joystick button 0") ||
            GameManager.Instance.operationBool && Input.GetKeyDown(KeyCode.Q))
        {
            operationCanvas.SetActive(false);
            GameManager.Instance.operationBool = false;
            notFirst = true;
        }

        //特定のボタンを押すと操作確認を表示。再度押すと非表示
        if (notFirst && !GameManager.Instance.operationBool && Input.GetKeyDown("joystick button 0") ||
            notFirst && !GameManager.Instance.operationBool && Input.GetKeyDown(KeyCode.M))
        {
            notFirst = false;
            operationCanvas.SetActive(true);
        }
        else if(!notFirst && !GameManager.Instance.operationBool && Input.GetKeyDown("joystick button 0") ||
                !notFirst && !GameManager.Instance.operationBool && Input.GetKeyDown(KeyCode.M))
        {
            notFirst = true;
            operationCanvas.SetActive(false);
        }


        //デバッグ用
        if (Input.GetKeyDown(KeyCode.P))
        {
            GameManager.Instance.operationBool = true;
        }

        if (Input.GetKeyDown(KeyCode.O))
        {
            GameManager.Instance.operationBool = false;
        }
    }
}
