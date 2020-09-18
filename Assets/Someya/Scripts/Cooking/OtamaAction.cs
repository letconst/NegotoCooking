using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OtamaAction : MonoBehaviour
{
    [SerializeField]
    private GameObject otama;
    [SerializeField]
    private GameObject centerPostion;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        //Debug.Log(h + "," + v);
        otama.transform.position = new Vector3(centerPostion.transform.position.x + h * 65, centerPostion.transform.position.y, centerPostion.transform.position.z + v * 70);
    }
}
