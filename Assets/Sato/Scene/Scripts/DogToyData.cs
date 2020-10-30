using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "DogToy", menuName = "Create DogToy")]

public class DogToyData : ScriptableObject
{
    [SerializeField]
    private List<DogFoodEntry> entries = new List<DogFoodEntry>();
    public List<DogFoodEntry> Entries => entries;

    public void AddEntry(Vector3 position,Quaternion rotation,int instanceID)
    {
        entries.Add(new DogFoodEntry(position, rotation, instanceID));
    }

    public void RemoveEntry(int instanceID)
    {
        entries.RemoveAll(e => e.InstanceID == instanceID);
    }

    public void UpdateEntry(DogFoodEntry entry, Vector3 position, Quaternion rotation, int instanceID)
    {
        entries.First(e => e == entry).UpdateEntry(position,rotation,instanceID);
    }
}

[System.Serializable]
public class DogFoodEntry
{
    [SerializeField]
    private Vector3 position;
    [SerializeField]
    private Quaternion rotation;
    [SerializeField]
    private int instanceID;

    public DogFoodEntry(Vector3 position, Quaternion rotation, int instanceID)
    {
        this.position = position;
        this.rotation = rotation;
        this.instanceID = instanceID;
    }

    public void UpdateEntry(Vector3 position, Quaternion rotation, int instanceID)
    {
        this.position = position;
        this.rotation = rotation;
        this.instanceID = instanceID;
    }

    public Vector3 Position => position;
    public Quaternion Rotation => rotation; 
    public int InstanceID => instanceID;
}

