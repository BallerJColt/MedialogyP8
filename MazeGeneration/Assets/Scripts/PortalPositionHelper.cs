using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public static class PortalPositionHelper
{
    private static int maxRows = GameObject.FindObjectOfType<MapManager>().mazeRows;
    private static int maxCols = GameObject.FindObjectOfType<MapManager>().mazeCols;
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

    private static List<int[]> CornerShutoffList = new List<int[]>
    {
        new int[2] {0,1},
        new int[2] {0, maxCols - 2},
        new int[2] {maxRows - 2, 0},
        new int[2] {maxRows - 1, maxCols - 2},
        new int[2] {1, 0},
        new int[2] {1, maxCols - 1},
        new int[2] {maxRows - 1, 1},
        new int[2] {maxRows - 2, maxCols - 1},
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
    private static int[] GetEntranceArray(string position)
    {
        return PortalEntranceArrayList[position];
    }

    public static int[] GetEntranceArray(int row, int col)
    {
        string direction = GetDirectionArray(row, col);
        return GetEntranceArray(direction);
    }

    public static int GetRandomPortalExit(int row, int col, int direction = -1)
    {
        string entrancePosition = GetDirectionArray(row, col);
        int[] directionArray = PortalEntranceArrayList[entrancePosition];
        return GetRandomArrayElementWithFlag(directionArray, direction);
    }

    public static List<int[]> GetShutoffCoordinate(int[] tileCoord)
    {
        /* int idx = 0;
        if (IsShutoffCoordinate(tileCoord))
        {
            idx = (CornerShutoffList.IndexOf(tileCoord) + 4) % 8;
        }
        return CornerShutoffList[idx]; */
        List<int> indexes = new List<int>();
        if (IsShutoffCoordinate(tileCoord))
        {

            foreach (int[] c in CornerShutoffList)
            {
                if (c == tileCoord)
                {
                    indexes.Add(CornerShutoffList.IndexOf(c));
                }
            }
        }
        List<int[]> shutoffCoords = new List<int[]>();
        foreach (int i in indexes)
        {
            int shutoffIndex = (i + 4) % 8;
            shutoffCoords.Add(CornerShutoffList[shutoffIndex]);
        }
        return shutoffCoords;
    }

    public static bool IsShutoffCoordinate(int[] tileCoord)
    {
        bool alma = false;
        Debug.Log("checking " + tileCoord[0] + ";" + tileCoord[1]);
        foreach (var v in CornerShutoffList)
        {
            if (v[0] == tileCoord[0] && v[1] == tileCoord[1])
            {
                Debug.Log("tile " + tileCoord[0] + ";" + tileCoord[1] + " is a shutoffcoord");
                alma = true;
            }
        }
        return alma;
    }
}