using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebrisHolder : Singleton<DebrisHolder>
{
    public OptimizedBehaviour HolderGO;

    private void Awake()
    {
        GameObject go = Instantiate(new GameObject(), Vector3.zero, Quaternion.identity);
        go.transform.name = "debrisHolderOB";
        go.AddComponent<OptimizedBehaviour>();
        HolderGO = go.GetComponent<OptimizedBehaviour>();
    }

}
