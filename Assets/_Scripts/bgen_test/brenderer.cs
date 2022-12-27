using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class brenderer : MonoBehaviour
{
    public Transform floorPrefab;
    public Transform[] wallPrefab;
    public Transform[] roofPrefab;
    Transform bldgFolder;

    public void Render(building bldg)
    {
        bldgFolder = new GameObject("Building").transform;
        foreach (wing w in bldg.Wings) {
            RenderWing(w);
        }
    }

    public void RenderWing(wing w)
    {
        Transform wingFolder = new GameObject("Wing").transform;
        wingFolder.SetParent(bldgFolder);
        foreach(story s in w.Stories)
        {
            RenderStory(s, w, wingFolder);
        }
        RenderRoof(w, wingFolder);
    }

    private void RenderStory(story s, wing w, Transform wingFolder)
    {

    }

    private void RenderRoof(wing w, Transform wingFolder)
    {

    }
}
