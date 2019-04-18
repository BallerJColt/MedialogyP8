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
        currentNumber = currentNumber + number * (int)Mathf.Pow(10, 3 - i);

        i++;

        if (i == 3)
        {
            if (currentNumber == password)
            {
                // Success
                Debug.Log("CORRECT PASSWORD");
            }

            i = 0;
            currentNumber = 0;
        }
    }
}
