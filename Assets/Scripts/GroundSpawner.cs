using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class GroundSpawner : MonoBehaviour
{
    [SerializeField] private ObjectPool _groundPool;
    [SerializeField] private ItemSpawner _itemSpawner;
    [SerializeField] private ItemSpawner _razorSpawner;
    [SerializeField] private PlayerController _player;
    [SerializeField] private int _initialCount = 3;
    [SerializeField] private Vector2 razorCount = new Vector2(1, 7);
   [SerializeField] private Vector2 itemCount = new Vector2(1, 5);
    private List<GameObject> grounds=new List<GameObject>();
    [SerializeField] private float speed=5f;
    private int nextGroundIndex = 0; 
    [SerializeField]  private float fixedSpeed=5f;
    private void Start()
    {
        float spawnZ = 0;
        for (int i = 0; i < _initialCount; i++)
        {
            GameObject go = SpawnGround(spawnZ);
            spawnZ += GetWidth(go) ;
        }
    }

    private void Update()
    {
        foreach (var ground in grounds)
        {
           ground.transform.Translate(Vector3.back* speed *Time.deltaTime);
        }

        CheckForRecycle();
    }

    private GameObject SpawnGround(float zPos)
    {
        GameObject newGround = _groundPool.GetObject();
        if (newGround != null)
        {
            newGround.transform.position = new Vector3(0, 0, zPos);
            newGround.SetActive(true);
            grounds.Add(newGround);
            SpawnerItems(newGround);
        }
        return newGround;
    }

    private void SpawnerItems(GameObject newGround)
    {
        var randItem = Random.Range(itemCount.x, itemCount.y);
        var randrazor = Random.Range(razorCount.x, razorCount.y);
        for (int i = 0; i < randItem; i++)
        {
            _itemSpawner.Spawner(newGround.GetComponent<GroundData>());
        }

        for (int i = 0; i < randrazor; i++)
        {
            _razorSpawner.Spawner(newGround.GetComponent<GroundData>());
        }
    }

    private void CheckForRecycle()
    {
        var currentGround = grounds[nextGroundIndex].GetComponent<GroundData>();
        
        if (_player.transform.position.z - currentGround.transform.position.z > currentGround.groundScrip.width)
        {
            float maxZ = currentGround.transform.position.z;
            foreach (var ground in grounds)
            {
                if (ground.transform.position.z > maxZ)
                    maxZ = ground.transform.position.z;
            }
            currentGround.transform.position = new Vector3(currentGround.transform.position.x, currentGround.transform.position.y, maxZ + currentGround.groundScrip.width);
            nextGroundIndex = (nextGroundIndex + 1) % _initialCount;
            SpawnerItems(grounds[nextGroundIndex]);
        }
    }

    float GetWidth(GameObject obj)
    {
        GroundData mt = obj.GetComponent<GroundData>(); 
        if (mt != null)
            return mt.groundScrip.width;
        else
            return 3;
    }

    public void SetSpeed(int boostedSpeed, bool isBoosted)
    {
        if (isBoosted) speed = boostedSpeed;
        else speed = fixedSpeed;
    }
}