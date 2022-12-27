using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class story
{
    int level;
    wall[] walls;

    public int Level { get => level; }
    public wall[] Walls { get => walls; }

    public story(int level, wall[] walls)
    {
        this.level = level;
        this.walls = walls;
    }
    public override string ToString()
    {
        string story = "Story " + level + "\n";
        story += "\t\tWalls: ";
        foreach (wall w in walls)
        {
            story += w.ToString() + ", ";
        }
        return story;         
    }

}

