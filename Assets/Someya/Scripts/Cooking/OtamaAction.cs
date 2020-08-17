using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OtamaAction : MonoBehaviour
{
    [SerializeField]
    private GameObject Otama;
    [SerializeField]
    private GameObject Centerpostion;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        Debug.Log(h + "," + v);
        Otama.transform.position = new Vector3(Centerpostion.transform.position.x + h * 65, Centerpostion.transform.position.y, Centerpostion.transform.position.z + v * 70);
    }
}
