using UnityEngine;
using System.Collections;

public class NavigationAllow : SingletonMonoBehaviour<NavigationAllow>
{
    [SerializeField]
    private Transform target_Sisyou;
    [SerializeField]
    private Transform target_Steps;
    [SerializeField]
    private Transform target_Kitchen;

    // カーソル
    [SerializeField]
    private Transform cursor;

    //bool管理
    [HideInInspector]
    public bool sisyouBool = true;
    [HideInInspector]
    public bool stepsBool;
    [HideInInspector]
    public bool kitchenBool;

    // Update is called once per frame
    void Update()
    {
        if (sisyouBool)
        {
            cursor.LookAt(target_Sisyou);
        }

        if (stepsBool)
        {            
            cursor.LookAt(target_Steps);
        }

        if (kitchenBool)
        {
            cursor.LookAt(target_Kitchen);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Nav_Sisyou")
        {
            sisyouBool = false;
            stepsBool = true;
        }

        if (other.gameObject.tag == "Nav_Steps")
        {            
            stepsBool = false;
            kitchenBool = true;
        }

        if (other.gameObject.tag == "Nav_Kitchen")
        {            
            kitchenBool = false;
            cursor.gameObject.SetActive(false);
        }
    }
}