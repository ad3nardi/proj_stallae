using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.Controls;

public class cinematic_demo : OptimizedBehaviour
{
    public List<GameObject> gameObjects = new List<GameObject>();

    private float _Timer;
    public float _Time;

    private void Start()
    {
        _Timer = _Time;
    }

    public void Update()
    {

        UpdateTimer();
    }

    private void UpdateTimer()
    {
        _Timer -= Time.deltaTime;
        if(_Timer <= 0)
        {
            for (int i = 0; i < gameObjects.Count; i++)
            {
                gameObjects[i].SetActive(false);
            }
        }
    }


}
