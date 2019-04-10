using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TileInfo
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

    public TileInfo(TileInfo obj)
    {
        row = obj.row;
        column = obj.column;
        direction = obj.direction;
    }

    public static bool operator ==(TileInfo lhs, TileInfo rhs)
    {
        return (lhs.row == rhs.row && lhs.column == rhs.column && lhs.direction == rhs.direction);
    }
    public static bool operator !=(TileInfo lhs, TileInfo rhs)
    {
        return !(lhs.row == rhs.row && lhs.column == rhs.column && lhs.direction == rhs.direction);
    }

    public override bool Equals(object obj)
    {
        if (obj == null)
            return false;
        else
        {
            TileInfo tile = (TileInfo)obj;
            return (row == tile.row && column == tile.column && direction == tile.direction);
        }
    }

    public override int GetHashCode()
    {
        return row + column + direction;
    }
}
