using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct TileInfo
{
    public int row;
    public int column;
    public int direction;

    public TileInfo(int _row, int _col, int _dir)
    {
        row = _row;
        column = _col;
        direction = _dir;
    }
}
