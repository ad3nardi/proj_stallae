using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static UnityEditor.PlayerSettings;

public class formation_manager : MonoBehaviour
{
    [Header("Plugins")]
    private formation_base _formation;
    public formation_base Formation
    {
        get
        {
            if(_formation == null) _formation = GetComponent<formation_base>();
            return _formation;
        }
        set => _formation = value;
    }

    [Header("Settings")]
    [SerializeField] private GameObject _unitVisual;
    [SerializeField] private int _unitCount;
    [SerializeField] private float _unitSpeed = 2;

    [SerializeField] private readonly List<GameObject> _spawnedUnits = new List<GameObject>();

    private List<Vector3> _points = new List<Vector3>();
    private Transform _parent;

    private void Awake()
    {
        _parent = this.transform;
    }


    private void Start()
    {
        _points = Formation.EvaluatePoints().ToList();
        Spawn(_unitCount);
    }

    private void Update()
    {
        SetFormation();
    }

    private void SetFormation()
    {
        _points = Formation.EvaluatePoints().ToList();
        if(_unitCount < _spawnedUnits.Count)
        {
            Kill(_spawnedUnits.Count - _unitCount);
        }
        if (_unitCount > _spawnedUnits.Count)
        {
            Spawn(_unitCount - _spawnedUnits.Count);
        }
        for (int i = 0; i < _spawnedUnits.Count; i++)
        {
            _spawnedUnits[i].transform.position = Vector3.MoveTowards(_spawnedUnits[i].transform.position,
                transform.position + _points[i], _unitSpeed * Time.deltaTime);
        }
    }

    private void Spawn(int unitsToSpawn)
    {
        for (int i = 0; i < unitsToSpawn; i++)
        {
            var unit = Instantiate(_unitVisual, transform.position, Quaternion.identity, _parent);
            _spawnedUnits.Add(unit);
        }
    }

    private void Kill(int num)
    {
        for (int i = 0; i < num; i++)
        {
            var unit = _spawnedUnits.Last();
            _spawnedUnits.Remove(unit);
            Destroy(unit.gameObject);
        }
    }
}


/*

private void SetFormation()
    {
        _points = Formation.EvaluatePoints().ToList();
        
        if(_points.Count > _spawnedUnits.Count )
        {
            var remaingPoints = _points.Skip(_spawnedUnits.Count);
            Spawn(remaingPoints);
        }
        
if (_points.Count < _spawnedUnits.Count)
{
    Kill(_spawnedUnits.Count - _points.Count);
}
for (int i = 0; i < _spawnedUnits.Count; i++)
{
    _spawnedUnits[i].transform.position = Vector3.MoveTowards(_spawnedUnits[i].transform.position,
        transform.position + _points[i], _unitSpeed * Time.deltaTime);
}
    }
    private void Spawn(IEnumerable<Vector3> points)
{
    foreach (var pos in points)
    {
        var unit = Instantiate(_unitPrefab, transform.position + pos, Quaternion.identity, _parent);
        _spawnedUnits.Add(unit);

    }
}

private void Kill(int num)
{
    for (int i = 0; i < num; i++)
    {
        var unit = _spawnedUnits.Last();
        _spawnedUnits.Remove(unit);
        Destroy(unit.gameObject);
    }
}

*/