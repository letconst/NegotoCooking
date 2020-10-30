using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ReSharper disable once InconsistentNaming
public class DoorSEHelper : MonoBehaviour
{
    private DoorController _parentController;

    // Start is called before the first frame update
    private void Start()
    {
        _parentController = transform.parent.gameObject.GetComponent<DoorController>();
    }

    private void PlayOpenSound() => _parentController.PlayOpenSound();

    private void PlayCloseSound() => _parentController.PlayCloseSound();
}
