using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class wing
{
    RectInt bounds;
    story[] stories;
    roof roofing;

    public RectInt Bounds { get => bounds; }
    public story[] Stories { get => stories; }
    public roof Roofing { get => roofing; }

    public wing(RectInt bounds)
    {
        this.bounds = bounds;

    }

    public wing(RectInt bounds, story[] stories, roof roofing)
    {
        this.bounds = bounds;
        this.stories = stories;
        this.roofing = roofing;

    }

    public override string ToString()
    {
        string wing = "wing(" + bounds.ToString() + "):\n";
            foreach(story s in stories)
        {
            wing += "\t" + s.ToString() + "\n";
        }

        wing += "\n" + roofing.ToString() + "\n";
        return wing;
    }
}
