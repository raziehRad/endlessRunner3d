using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    [SerializeField] private ObjectPool _itemPool;
    [SerializeField] private float _offset = 3;

    public void Spawner(GroundData groundData)
    {
        var pos = groundData.groundPos[Random.Range(0, groundData.groundPos.Length)].position.z;
        var item = _itemPool.GetObject();
        if (item != null)
        {
            //item.SetActive(true);
            var data = item.GetComponent<Item>().data;
            item.transform.SetParent(groundData.transform);
            var xpos = Random.Range(-3f, 3f);
            item.transform.position = new Vector3(xpos, groundData.transform.position.y + data.offset, pos);
        }
    }
}