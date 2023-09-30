using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnFly_manager : Singleton<OnFly_manager>
{
    [SerializeField] public so_flyResources _onFlyResources { get; private set; }
    public void Awake()
    {
        _onFlyResources = Resources.Load<so_flyResources>("Prefabs/onFlyResources");
    }

}
