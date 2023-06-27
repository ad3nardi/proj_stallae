using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class raycastTest : MonoBehaviour
{

    [SerializeField] private float _distLength;
    [SerializeField] private bool _checkDist;
    [SerializeField] private LayerSet _layerSet;
    [SerializeField] private LayerMask _layerMask;
    private void Start()
    {
        _distLength = 0f;
        _layerSet = Helpers.LayerSet;
    }

    public void Update()
    {
        RaycastFire();
    }

    public void RaycastFire()
    {
        if (_checkDist)
        {
            RaycastHit hit;
            if(Physics.Raycast(transform.position, transform.forward, out hit, Mathf.Infinity))
            {
                Debug.DrawLine(transform.position, hit.transform.position, Color.green);
                _distLength = hit.distance;
                Debug.Log(hit.distance);
                Debug.Log(hit.transform);
            }
        }
    }
    public void OnDrawGizmos()
    {
        Gizmos.color = Color.magenta;
    }

}
