using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PortalPositionHelper
{
    private static int maxRows = GameObject.FindObjectOfType<MapGenerator>().mazeRows;
    private static int maxCols = GameObject.FindObjectOfType<MapGenerator>().mazeCols;
    private static Dictionary<string, int[]> PortalEntranceArrayList = new Dictionary<string, int[]>
    {
        {"topLeftCorner", new int[2] {1,2}},
        {"topRightCorner", new int[2] {2,3}},
        {"bottomRightCorner", new int[2] {0,3}},
        {"bottomLeftCorner", new int[2] {0,1}},
        {"topRow", new int[3] {1,2,3}},
        {"rightColumn", new int[3] {0,2,3}},
        {"bottomRow", new int[3] {0,1,3}},
        {"leftColumn", new int[3] {0,1,2}},
        {"inside", new int[4] {0,1,2,3}},
    };
    private static string GetDirectionArray(int row, int col)
    {
        if (row == 0)
        {
            if (col == 0)
                return "topLeftCorner";
            else if (col == maxCols - 1)
                return "topRightCorner";
            else
                return "topRow";
        }
        else if (row == maxRows - 1)
        {
            if (col == 0)
                return "bottomLeftCorner";
            else if (col == maxCols - 1)
                return "bottomRightCorner";
            else
                return "bottomRow";
        }
        else
        {
            if (col == 0)
                return "leftColumn";
            else if (col == maxCols - 1)
                return "rightColumn";
            else
                return "inside";
        }
    }

    // Returns a random array element except for the one specified in the flag argument
    private static int GetRandomArrayElementWithFlag(int[] arr, int flag)
    {
        int i = 0;
        do
        {
            i = Random.Range(0, arr.Length);
        }
        while (arr[i] == flag);

        return arr[i];
    }

    // If we want to choose an exit direction ourselves, we can use this method to access the possible exit directions
    public static int[] GetEntranceArray(string position) {
        return PortalEntranceArrayList[position];
    }

    public static int GetRandomPortalExit(int row, int col, int direction)
    {
        string entrancePosition = GetDirectionArray(row,col);
        int[] directionArray = PortalEntranceArrayList[entrancePosition];
        return GetRandomArrayElementWithFlag(directionArray,direction);
    }
}
