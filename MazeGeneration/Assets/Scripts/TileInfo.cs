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

    public bool IsInCorner()
    {
        int mazeRows = GameObject.Find("MapManager").GetComponent<MapManager>().mazeRows; //these should be globals somewhere...
        int mazeCols = GameObject.Find("MapManager").GetComponent<MapManager>().mazeCols;
        bool inCorner = false;
        if (row == 0 && column == 0 ||
            row == 0 && column == mazeCols - 1 ||
            row == mazeRows && column == 0 ||
            row == mazeRows && column == mazeCols - 1)
            inCorner = true;
        return inCorner;
    }

    public bool IsPerpendicular()
    {
        int mazeRows = GameObject.Find("MapManager").GetComponent<MapManager>().mazeRows;
        int mazeCols = GameObject.Find("MapManager").GetComponent<MapManager>().mazeCols;
        bool perpendicular = false;
        switch (direction)
        {
            case 0:
                if (row == mazeRows - 1)
                    perpendicular = true;
                break;
            case 1:
                if(column == 0)
                    perpendicular = true;
                break;
            case 2:
                if(row == 0)
                    perpendicular = true;
                break;
            case 3:
                if(column == mazeCols - 1)
                    perpendicular = true;
                break;
            default:
                break;
        }
        return perpendicular;
    }

    public bool IsSamePosition(TileInfo tile)
    {
        return (row == tile.row && column == tile.column);
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
