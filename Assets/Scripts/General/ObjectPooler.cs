using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class ObjectPooler : MonoBehaviour
{
    public GameObject _prefabToPool;
    public int _maxObjects;
    private LinkedList<GameObject> _currentPool = new LinkedList<GameObject>();


    public GameObject GetPrefab()
    {
        if (_currentPool.Count < 1) return Instantiate(_prefabToPool); //Create new if pool is empty.

        var obj = _currentPool.First;
        var pulledObject = obj.Value;
        _currentPool.Remove(pulledObject);
        pulledObject.SetActive(true);
        return pulledObject;
    }

    public void DestroyPrefab(GameObject obj) //Check if this object should be destroyed or pooled.
    {
        if (_currentPool.Count >= _maxObjects)
        {
            Destroy(obj);
            return;
        }

        obj.transform.parent = transform;
        obj.SetActive(false);
        _currentPool.AddFirst(obj);
    }

    private void OnDisable()
    {
        foreach (var obj in _currentPool)
        {
            Destroy(obj);
        }

        _currentPool.Clear();
    }
}
