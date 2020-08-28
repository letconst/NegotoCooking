using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomMovePosition : MonoBehaviour
{
    private Vector3 startPosition = GameObject.Find("CentralPoint").transform.position;

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
        float posX = Random.Range(-1 * radius, radius);
        float posZ = Random.Range(-1 * radius, radius);

        //destination = startPosition + new Vector3(posX, 0, posZ);

        transform.position = startPosition + new Vector3(posX, 0, posZ);
    }
}
