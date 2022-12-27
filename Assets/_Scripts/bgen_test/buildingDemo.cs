using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class buildingDemo : MonoBehaviour
{
    public bgen_bldgSettings settings;

    private void Start()
    {  
        building b = bgenerator.Generate(settings);
        Debug.Log(b.ToString());
    }
}
