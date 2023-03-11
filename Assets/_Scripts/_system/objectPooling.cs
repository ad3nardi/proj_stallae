using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*TO USE:
 * CALL GetObject
 * set Instance's properties as needed
 * 
 * To return -> set logic in child (allow a GetComponent after instantiated to get pooler and return (this) when needed
 */
public class objectPooling : OptimizedBehaviour
{
    [Header("Plugins")]
    [SerializeField] private OptimizedBehaviour _objectPrefab;
    [Header("Pool")]
    [SerializeField] private int _poolSize;
    [SerializeField] private bool _expandable;
    [SerializeField] private List<OptimizedBehaviour> _freeList;
    [SerializeField] private List<OptimizedBehaviour> _usedList;

    private void Awake()
    {
        _freeList = new List<OptimizedBehaviour>();
        _usedList = new List<OptimizedBehaviour>();

        for (int i = 0; i < _poolSize; i++)
        {
            GenerateNewObject();
        }
    }

    public OptimizedBehaviour GetObject()
    {
        int totalFree = _freeList.Count;
        if (totalFree == 0 && !_expandable)
            return null;
        else if (totalFree == 0 && _expandable)
            GenerateNewObject();

        OptimizedBehaviour g = _freeList[totalFree - 1];
        _freeList.RemoveAt(totalFree -1);
        _usedList.Add(g);
        return g;
    }

    public void ReturnObject(OptimizedBehaviour instance)
    {
        Debug.Assert(_usedList.Contains(instance));
        instance.CachedGameObject.SetActive(false);
        _usedList.Remove(instance);
        _freeList.Add(instance);
    }

    private void GenerateNewObject()
    {
        OptimizedBehaviour g = Instantiate(_objectPrefab);
        g.CachedTransform.parent = CachedTransform;
        g.CachedGameObject.SetActive(false);
        _freeList.Add(g);
    }
}
