using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Negoto : MonoBehaviour
{
    private GameObject[] Negotos;//配列


    private void Start()
    {
        Negotos = GameObject.FindGameObjectsWithTag("Negoto");
        foreach (GameObject i in Negotos)
        {
            i.SetActive(false);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        Debug.Log("寝言");

    }

    void OnTriggerStay(Collider other)
    {
        foreach(GameObject i in Negotos)
        {
            i.SetActive(true);
        }
    }

    void OnTriggerExit(Collider other)
    {
        foreach (GameObject i in Negotos)
        {
            i.SetActive(false);
        }
    }
}
