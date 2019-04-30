using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyPadPuzzle : MonoBehaviour
{
    public int password;
    private int i = 0;
    private int currentNumber = 0;


    public void InputNumber(int number)
    {
        Debug.Log("Invoked");

        currentNumber = currentNumber + number * (int)Mathf.Pow(10, 3 - i % 4);

        Debug.Log(currentNumber);

        if (i % 4 == 3)
        {
            if (currentNumber == password)
            {
                // Success
                GameObject.Find("Safe_lever").GetComponent<Valve.VR.InteractionSystem.Interactable>().enabled = true;
                Debug.Log("CORRECT PASSWORD");
            }
            currentNumber = 0;
        }
        i++;
    }

    public void UnlockDoor()
    {
        gameObject.GetComponentInParent<Valve.VR.InteractionSystem.Interactable>().enabled = true;
    }
}
