using DG.Tweening;
using UnityEngine;

public class PlayerCollision : MonoBehaviour
{
    [SerializeField] private PlayerController _playerController;
    private int score = 0;
    private void OnCollisionExit(Collision other)
    {
        if (other.transform.CompareTag("Ground"))
        {
            _playerController.SetGroundCheck(false);
            
        }
    }
    private void OnCollisionEnter(Collision other)
    {
        if (other.transform.CompareTag("Ground"))
        {
            _playerController.SetGroundCheck(true);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Item"))
        {
            var data = other.GetComponent<Item>().data;
            AudioManager.instance.PlayItemSound(data.clip);
            score+=data.value;
            UIManager.instance.SetScore(score);
            Debug.Log("Collected item! Score: " + score);
            other.gameObject.SetActive(false);
        }
        if (other.CompareTag("Razor"))
        {
            var data = other.GetComponent<Item>().data;
            other.gameObject.SetActive(false);
            UIManager.instance.SetHealth(data.damage);
            
            StartCoroutine(_playerController.PlayDamageAnimation());
        }
    }

  
}