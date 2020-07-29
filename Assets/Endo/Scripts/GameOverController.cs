using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverController : MonoBehaviour
{
    public PlayerInventoryContainer playerContainer;
    public InventoryContainerBase largePlateContainer;

    // Start is called before the first frame update
    void Start()
    {
        // 各値をリセットしてあげる
        playerContainer.Container.Clear();
        largePlateContainer.Container.Clear();
        TimeCounter.currentTime = TimeCounter.countup;
        GameManager.Instance.NoiseMator = 0;
    }
}
