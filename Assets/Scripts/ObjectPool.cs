using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class ObjectPool : MonoBehaviour
{
    public GameObject[] _prefabs;
    public int _instanceCount = 10;

    private List<GameObject> pool = new List<GameObject>();

    private void Awake()
    {
        for (int i = 0; i < _instanceCount; i++)
        {
            var rand = Random.Range(0, _prefabs.Length);
            var _obj = Instantiate(_prefabs[rand]);
            _obj.SetActive(false);
            pool.Add(_obj);
        }
    }

    public GameObject GetObject()
    {
        foreach (var obj in pool)
        {
            if (!obj.activeInHierarchy)
            {
                obj.SetActive(true);
                return obj;
            }
        }

        return null;
    }
}