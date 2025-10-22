
    using UnityEngine;

    public class ItemSpawner : MonoBehaviour
    {
        [SerializeField] private ObjectPool _itemPool;
        [SerializeField] private float _offset=3;
        public void Spawner(GroundData groundData)
        {
            var chanceExist = Random.Range(0f, 1f);
            if (chanceExist>0.5f) return;
            var chancePos = Random.Range(0f, 1f);
            float zpos=groundData.transform.position.z;
            if (chancePos<0.35) zpos= groundData.transform.position.z;//middle
            else if(chancePos>0.35 && chancePos<0.7) zpos=groundData.transform.position.z + (groundData.groundScrip.width/2f)-2;//left
            else if(chancePos>0.7)zpos=groundData.transform.position.z - (groundData.groundScrip.width/2f)+2;//right
            var item = _itemPool.GetObject();
            if (item != null)
            {
                var xpos = Random.Range(-3f, 3f);
                item.transform.position = new Vector3(xpos, groundData.transform.position.y + _offset,zpos);
                item.SetActive(true);
                item.transform.SetParent(groundData.transform);
            }
        }
    }
