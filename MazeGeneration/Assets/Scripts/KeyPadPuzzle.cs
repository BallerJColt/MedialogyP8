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
                Debug.Log("CORRECT PASSWORD");
                gameObject.GetComponentInChildren<Valve.VR.InteractionSystem.Interactable>().enabled = true;
            }
            currentNumber = 0;
        }
        i++;
    }
}
