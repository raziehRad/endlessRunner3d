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
    //[SerializeField] private Vector2 Zspawning = new Vector2(1, 3);
   // [SerializeField] private Vector2 Yspawning = new Vector2(-1, 1);
    private List<GameObject> grounds=new List<GameObject>();
    [SerializeField] private float speed=5f;
    private int nextGroundIndex = 0; 
    private float fixedSpeed=5f;
    private void Start()
    {
        float spawnZ = 0;
        for (int i = 0; i < _initialCount; i++)
        {
            //float yPos = Random.Range(Yspawning.x, Yspawning.y);
            GameObject go = SpawnGround(spawnZ, 0,true);
            //float spacing = Random.Range(Zspawning.x, Zspawning.y);
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

    private GameObject SpawnGround(float zPos, float yPos, bool check)
    {
        GameObject newGround = _groundPool.GetObject();
        if (newGround != null)
        {
            newGround.transform.position = new Vector3(0, 0, zPos);
            newGround.SetActive(true);
            grounds.Add(newGround);
            _itemSpawner.Spawner(newGround.GetComponent<GroundData>());
            _razorSpawner.Spawner(newGround.GetComponent<GroundData>());
        }
        return newGround;
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
          //  float spacing = Random.Range(Zspawning.x, Zspawning.y);
            //float yPos = Random.Range(Yspawning.x, Yspawning.y);
            currentGround.transform.position =
                new Vector3(currentGround.transform.position.x, currentGround.transform.position.y, maxZ + currentGround.groundScrip.width);
            
            nextGroundIndex = (nextGroundIndex + 1) % _initialCount;
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