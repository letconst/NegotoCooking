using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OperationConfirmation : MonoBehaviour
{
    [SerializeField]
    private GameObject operationCanvas;
    private bool notFirst = true;
    private void Start()
    {
        operationCanvas.SetActive(false);
    }
    void Update()
    {
        CutOperation();
        BakeOperation();
        BoilOperation();
    }

    private void CutOperation()
    {
        if (SceneManager.GetActiveScene().name == "CutScenes")
        {
            //最初の一回は必ず操作確認を表示。次回からは任意で表示できる。
            if (GameManager.Instance.cutOperationBool)
            {
                operationCanvas.SetActive(true);
            }
            
            if (GameManager.Instance.cutOperationBool && Input.GetKeyDown("joystick button 0") ||
                GameManager.Instance.cutOperationBool && Input.GetKeyDown(KeyCode.Q))
            {
                operationCanvas.SetActive(false);
                GameManager.Instance.cutOperationBool = false;                
            }

            //特定のボタンを押すと操作確認を表示。
            if (notFirst && !GameManager.Instance.cutOperationBool && Input.GetKeyDown("joystick button 0") ||
                notFirst && !GameManager.Instance.cutOperationBool && Input.GetKeyDown(KeyCode.M))
            {
                notFirst = false;
                operationCanvas.SetActive(true);
            }

            //再度押すと非表示。
            else if (!notFirst && !GameManager.Instance.cutOperationBool && Input.GetKeyDown("joystick button 0") ||
                     !notFirst && !GameManager.Instance.cutOperationBool && Input.GetKeyDown(KeyCode.M))
            {
                notFirst = true;
                operationCanvas.SetActive(false);
            }
            StartCoroutine(waitTime(0.3f));
        }        
    }

    private void BakeOperation()
    {
        if (SceneManager.GetActiveScene().name == "BakeScenes")
        {
            //最初の一回は必ず操作確認を表示。次回からは任意で表示できる。
            if (GameManager.Instance.bakeOperationBool)
            {
                operationCanvas.SetActive(true);
            }

            if (GameManager.Instance.bakeOperationBool && Input.GetKeyDown("joystick button 0") ||
                GameManager.Instance.bakeOperationBool && Input.GetKeyDown(KeyCode.Q))
            {
                operationCanvas.SetActive(false);
                GameManager.Instance.bakeOperationBool = false;
            }

            //特定のボタンを押すと操作確認を表示。
            if (notFirst && !GameManager.Instance.bakeOperationBool && Input.GetKeyDown("joystick button 0") ||
                notFirst && !GameManager.Instance.bakeOperationBool && Input.GetKeyDown(KeyCode.M))
            {
                notFirst = false;
                operationCanvas.SetActive(true);
            }

            //再度押すと非表示。
            else if (!notFirst && !GameManager.Instance.bakeOperationBool && Input.GetKeyDown("joystick button 0") ||
                     !notFirst && !GameManager.Instance.bakeOperationBool && Input.GetKeyDown(KeyCode.M))
            {
                notFirst = true;
                operationCanvas.SetActive(false);
            }
            StartCoroutine(waitTime(0.3f));
        }
    }

    private void BoilOperation()
    {
        if (SceneManager.GetActiveScene().name == "BoilScenes")
        {
            //最初の一回は必ず操作確認を表示。次回からは任意で表示できる。
            if (GameManager.Instance.boilOperationBool)
            {
                operationCanvas.SetActive(true);
            }

            if (GameManager.Instance.boilOperationBool && Input.GetKeyDown("joystick button 0") ||
                GameManager.Instance.boilOperationBool && Input.GetKeyDown(KeyCode.Q))
            {
                operationCanvas.SetActive(false);
                GameManager.Instance.boilOperationBool = false;
            }

            //特定のボタンを押すと操作確認を表示。
            if (notFirst && !GameManager.Instance.boilOperationBool && Input.GetKeyDown("joystick button 0") ||
                notFirst && !GameManager.Instance.boilOperationBool && Input.GetKeyDown(KeyCode.M))
            {
                notFirst = false;
                operationCanvas.SetActive(true);
            }

            //再度押すと非表示。
            else if (!notFirst && !GameManager.Instance.boilOperationBool && Input.GetKeyDown("joystick button 0") ||
                     !notFirst && !GameManager.Instance.boilOperationBool && Input.GetKeyDown(KeyCode.M))
            {
                notFirst = true;
                operationCanvas.SetActive(false);
            }
            StartCoroutine(waitTime(0.3f));
        }
    }
    private IEnumerator waitTime(float time)
    {
        yield return new WaitForSeconds(time);
        yield break;    
    }
}
