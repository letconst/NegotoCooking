using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Statistics", menuName = "Create Statistics")]
public class StatisticsManager : ScriptableObject
{
    [SerializeField]
    public int throwInCount;
    [SerializeField]
    public int missItemCount;

    public float a;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        a += Time.deltaTime;
    }
}
