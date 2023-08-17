using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFleetMan
{
    private static EnemyFleetMan _instance;
    public static EnemyFleetMan Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new EnemyFleetMan();
            }
            return _instance;
        }
        private set
        {
            _instance = value;
        }
    }
    public List<enem_battlegroupMan> AvaliableBattlegroups = new List<enem_battlegroupMan>();
    public List<enem_unitMan> AvaliableUnits = new List<enem_unitMan>();
    private EnemyFleetMan() { }
}
