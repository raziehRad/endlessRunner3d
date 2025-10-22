
    using UnityEngine;

    [CreateAssetMenu(fileName = "Item",menuName = "Game/Item")]
    public class ItemData : ScriptableObject
    {
        public string name;
        public GameObject prefab;
        public SpriteRenderer image;
        public int value;
        public int damage;
        public ItemType itemType;
        public AudioClip clip;
    }

    public enum ItemType
    {
        item,enemy
    }
