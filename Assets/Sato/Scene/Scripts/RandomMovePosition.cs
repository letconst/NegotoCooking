using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomMovePosition : MonoBehaviour
{
    public Transform centerPos;

    private Vector3 destination;

    [SerializeField]
    float time = 0f;

    [SerializeField]
    float waitTime = 2f;

    [SerializeField]
    float radius = 3f;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;
        if (time > waitTime)
        {
            RandomPosition();
            time = 0;
        }

    }

    private void RandomPosition()
    {
        Vector3 ctp = centerPos.position;

        float posX = Random.Range(-1 * radius, radius);
        float posZ = Random.Range(-1 * radius, radius);

        ctp.x += posX;
        ctp.z += posZ;

        //destination = startPosition + new Vector3(posX, 0, posZ);

        transform.position = ctp;
    }
}
