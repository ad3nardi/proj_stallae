using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Pool;

public class missilePool : OptimizedBehaviour
{
    private OptimizedBehaviour _spawnPos;
    private wpn_missile _missilePrefab;
    private ObjectPool<wpn_missile> _missilePool;

    private void Awake()
    {
        _missilePool = new ObjectPool<wpn_missile>(CreatePooledObject, OnTakeFromPool, OnReturnToPool, OnDestroyObject, false, 200, 100_000);

    }
    private wpn_missile CreatePooledObject()
    {
        wpn_missile instance = Instantiate(_missilePrefab, Vector3.zero, Quaternion.identity);
        //instance.Disable += ReturnObjectToPool;
        instance.CachedGameObject.SetActive(false);
        return instance;

    }

    private void OnTakeFromPool(wpn_missile instance)
    {
        instance.CachedGameObject.SetActive(true);
        SpawnMissile(instance);
        instance.CachedTransform.SetParent(transform, true);

    }

    private void OnReturnToPool(wpn_missile instance)
    {
        instance.CachedGameObject.SetActive(false);
    }

    private void OnDestroyObject(wpn_missile instance)
    {
        Destroy(instance.CachedGameObject);
    }

    private void ReturnObjectToPool(wpn_missile instance)
    {
        _missilePool.Release(instance);
    }

    private void SpawnMissile(wpn_missile instance)
    {
        Vector3 spawnPos = _spawnPos.CachedTransform.position;
        instance.CachedTransform.position = spawnPos;

    }

    private void OnGUI()
    {
        GUI.Label(new Rect(10, 10, 200, 30), $"Total Pool Size: {_missilePool.CountAll}");
        GUI.Label(new Rect(10, 10, 200, 30), $"Act Objects: {_missilePool.CountActive}");
    }

}
