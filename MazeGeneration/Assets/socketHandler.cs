using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class socketHandler : MonoBehaviour
{

    public bool plugOccupied = false;
    private void OnTriggerEnter(Collider other)
    {
        plugOccupied = true;
    }

    private void OnTriggerExit(Collider other)
    {
        plugOccupied = false;
    }
}
