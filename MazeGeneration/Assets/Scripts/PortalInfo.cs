using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct PortalInfo
{
    public int row;
    public int column;
    public int entranceDirection;

    public PortalInfo(int _row, int _col, int _dir)
    {
        row = _row;
        column = _col;
        entranceDirection = _dir;
    }
}
