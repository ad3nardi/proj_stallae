using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fleet_manager : MonoBehaviour
{
    [Header("Fleet")]
    [SerializeField] public Transform fleetParent;
    [SerializeField] public Transform spawnPosition;
    [SerializeField] public float spawnOffsetAmount;
    [SerializeField] public List<GameObject> fleet = new List<GameObject>();

    public void InstantiateFleet(List<GameObject> fleet)
    {
        for(int i = 0; i < fleet.Count; i++)
        {
            Instantiate(fleet[i], spawnPosition.position + Vector3.right * i * spawnOffsetAmount, Quaternion.identity, fleetParent);
        }
    }

    public void AddShip(GameObject unit)
    {
        fleet.Add(unit);  
    }

    public void RemoveShip(GameObject unit)
    {
        fleet.Remove(unit);
    }


}
