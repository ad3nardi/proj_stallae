using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gui_healthBarCon : MonoBehaviour
{
    [SerializeField] private GameObject _objParent;
    private Transform _tParent;
    
    [SerializeField] gui_healthBarVis _healthBarPrefab;

    private Dictionary<unit_subsystem, gui_healthBarVis> _healthBars = new Dictionary<unit_subsystem, gui_healthBarVis>();

    private void Awake()
    {
        unit_subsystem.OnHealthAdded += AddHealthBar;
        unit_subsystem.OnHealthRemoved += RemoveHealthBar;
        _tParent = _objParent.GetComponent<Transform>();
    }

    private void AddHealthBar(unit_subsystem health)
    {
        if(_healthBars.ContainsKey(health) == false)
        {
            var healthBar = Instantiate(_healthBarPrefab, _tParent);
            _healthBars.Add(health, healthBar);
            healthBar.SetHealth(health);
        }
    }

    private void RemoveHealthBar(unit_subsystem health)
    {
        if (_healthBars.ContainsKey(health))
        {
            Destroy(_healthBars[health].gameObject);
            _healthBars.Remove(health);
        }
    }

}
