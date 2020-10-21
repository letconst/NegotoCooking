using UnityEngine;

public class DogToyController : MonoBehaviour
{
    //犬のおもちゃの体力を設定する
    [SerializeField]
    public float dogFoodHealth = 20f;

    private void Update()
    {
        if(dogFoodHealth <= 0)
        {
            GameManager.Instance.DogToyData.RemoveEntry(gameObject.GetInstanceID());
            Destroy(gameObject);
        }
    }
}
