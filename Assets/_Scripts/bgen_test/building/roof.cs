using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class roof
{
    RoofType type;
    RoofDirection direction;

    public RoofType Type { get => type; }
    public RoofDirection Direction { get => direction; }


    public roof(RoofType type = RoofType.Peak, RoofDirection direction = RoofDirection.North)
    {
        this.type = type;
        this.direction = direction;
    }

    public override string ToString()
    {
        return "Roof: " + type.ToString() + ((type == RoofType.Peak || type == RoofType.Slope) ? "," + direction.ToString() : "");
    }
}

public enum RoofType
{
    Point,
    Peak,
    Slope,
    Flat
}

public enum RoofDirection
{
    North,  //positive y
    East,   //positive x
    South,  //negative y
    West    //negative y
}