using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mS_captureArea : missionStructure
{
    [Header("Area Points")]
    [SerializeField] private List<Transform> _TcaptureAreas= new List<Transform>();


    private void Start()
    {

    }
}
