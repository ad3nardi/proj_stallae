using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gui_healthBarCon : MonoBehaviour
{
    [SerializeField] gui_healthBarVis _healthBarPrefab;

    private Dictionary<unit_subsytems, gui_healthBarVis> _healthBars = new Dictionary<unit_subsytems, gui_healthBarVis>();

    private void Awake()
    {
        unit_subsytems.OnHealthAdded += AddHealthBar;
        unit_subsytems.OnHealthRemoved += RemoveHealthBar;
    }

    private void AddHealthBar(unit_subsytems health)
    {
        if(_healthBars.ContainsKey(health) == false)
        {
            var healthBar = Instantiate(_healthBarPrefab, transform);
            _healthBars.Add(health, healthBar);
            healthBar.SetHealth(health);
        }
    }

    private void RemoveHealthBar(unit_subsytems health)
    {
        if (_healthBars.ContainsKey(health))
        {
            Destroy(_healthBars[health].gameObject);
            _healthBars.Remove(health);
        }
    }

}
