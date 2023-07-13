using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enem_fleetMan : OptimizedBehaviour
{
    [Header("MissionParam")]
    [SerializeField] private gameManager _gm;
    [SerializeField] private missionStructure _activeMission;
    [SerializeField] private missionType _activeMissionType;

    [Header("Plugins")]
    [SerializeField] private Transform _vShipT;
    [SerializeField] private unit_Manager  _vShip;

    [Header("Valeria Settings ")]
    [SerializeField] private LayerMask _checkValeriaLayer;
    [SerializeField] private bool _inBand1, _inBand2;
    [SerializeField] private float _checkRangeBandOne, _checkRangeBandTwo;
    [SerializeField] private float _enemCountFar, _enemCountMid;

    [Header("Battlegroup Settings")]
    [SerializeField] private List<Transform> _fleetList = new List<Transform>();
    [SerializeField] private List<enem_battlegroupMan> _battlegroups = new List<enem_battlegroupMan>();

    [Header("Hotzone Settings")]
    [SerializeField] private LayerMask _checkHotzoneLayer;
    [SerializeField] private float _mapSize = 1000;
    [SerializeField] private float _mapOffset = 250;
    [SerializeField] private int _gridSize = 3; // Size of the grid (3x3 in this case)
    [SerializeField] private float _squareSize = 0;
    [SerializeField] private float _hotzoneOverlap = 0.1f;
    [SerializeField] private Vector3 _hotzoneScale;
    [SerializeField] private List<float> _hotzonesCount = new List<float>();

    private void Awake()
    {
        _activeMissionType = missionType.none;

        _inBand1 = false;
        _inBand2 = false;
    }

    private void Start()
    {
        _squareSize = _mapSize / _gridSize;
        _hotzoneScale = new Vector3(_squareSize, 10, _squareSize)/2;
        for (int i = 0; i < _gridSize*_gridSize; i++)
        {
            _hotzonesCount.Add(0);
        }

        _vShip = _vShipT.GetComponent<unit_Manager>();
        _battlegroups.Clear();
        for (int i = 0; i < _fleetList.Count; i++)
        {
            _battlegroups.Add(_fleetList[i].GetComponent<enem_battlegroupMan>());
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(_vShip.CachedTransform.position, _checkRangeBandOne);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(_vShip.CachedTransform.position, _checkRangeBandTwo);
        Gizmos.color = new Color(60, 60, 60, 0.3f);
        
        for (int row = 0; row < _gridSize; row++)
        {
            for (int col = 0; col < _gridSize; col++)
            {
                float xPos = col * _squareSize - _hotzoneOverlap;
                float yPos = row * _squareSize - _hotzoneOverlap;

                Gizmos.DrawCube(new Vector3(xPos- _mapOffset, 0, yPos - _mapOffset), _hotzoneScale*2);
            }
        }
    }

    private void Update()
    {
        UpdateValeriaRangeCheck();
        UpdateHotzoneCheck();
    }

    private void UpdateValeriaRangeCheck()
    {
        _inBand1 = false;
        Collider[] hitColB1 = Physics.OverlapSphere(_vShip.CachedTransform.position, _checkRangeBandOne, _checkValeriaLayer);
        if (hitColB1.Length > 0)
        {
            _inBand1 = true;
            _enemCountFar = hitColB1.Length;

            _enemCountMid = 0;
            _inBand2 = false;

            Collider[] hitColB2 = Physics.OverlapSphere(_vShip.CachedTransform.position, _checkRangeBandTwo, _checkValeriaLayer);
            if (hitColB2.Length > 0)
            {
                _inBand2 = true;
                _enemCountMid = hitColB2.Length;
                for (int i = 0; i < hitColB1.Length; i++)
                {
                    if (hitColB1[i].transform == hitColB2[i].transform)
                    {
                        _enemCountFar -= 1;
                        if (_enemCountFar == 0)
                        {
                            _inBand1 = false;
                        }
                    }
                }
            }
        }
    }

    private void UpdateHotzoneCheck()
    {
        for (int i = 0; i < _hotzonesCount.Count; i++)
        {
            _hotzonesCount[i] = 0f;
        }
        
        for (int row = 0; row < _gridSize; row++)
        {
            for (int col = 0; col < _gridSize; col++)
            {
                float xPos = col * _squareSize - _hotzoneOverlap;
                float yPos = row * _squareSize - _hotzoneOverlap;
                Collider[] hitCol = Physics.OverlapBox(new Vector3(xPos - _mapOffset, 0, yPos- _mapOffset), _hotzoneScale, Quaternion.identity, _checkHotzoneLayer);
                if(hitCol.Length > 0)
                {
                    _hotzonesCount[col + row] = hitCol.Length;
                }
            
            }
        }

    }
}

