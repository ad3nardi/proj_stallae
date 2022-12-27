using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class building
{

    Vector2Int size;
    wing[] wings;

    public Vector2Int Size { get { return size; } }
    public wing[] Wings { get { return wings; } }


    public building(int sizeX, int sizeY, wing[] wings)
    {
        size = new Vector2Int(sizeX, sizeY);
        this.wings = wings;
    }

    public override string ToString()
    {
        string bldg = "Building:(" + size.ToString() + ": " + wings.Length + ")\n";
        foreach(wing w in wings)
        {
            bldg += "\t" + w.ToString() + "\n";  
        }
        return bldg;
    }
}
