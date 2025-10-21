using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float _jumpForce;
    [SerializeField] private float _moveSpeed=3;
   [SerializeField]   private Animator _animator;
   [SerializeField]   private UIManager _uiManager;
    private Rigidbody rb;
    private bool _isGround;
    
    public UIManager UiManager=>_uiManager;
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        JumpAction();
        MevementAction();
        if (transform.position.y<-10)
        {
            _uiManager.SaveData();
        }
    }

    private void JumpAction()
    {
        if (_isGround && (Input.GetButton("Jump") || IsTouchBegan()))
        {
            rb.AddForce(Vector3.up * _jumpForce, ForceMode.Impulse);
            // _animator.SetBool("jumpup",true);
            AudioManager.instance.PlayJumpSound();
            _animator.CrossFade("Jumping",0.1f);
        }
    }

    private void MevementAction()
    {
        var horizontal = Input.GetAxis("Horizontal");
        transform.Translate(horizontal * _moveSpeed * Time.deltaTime, 0, 0);
        if (transform.position.x < -3)
        {
            transform.position = new Vector3(-3, transform.position.y, transform.position.z);
        }

        if (transform.position.x > 3)
        {
            transform.position = new Vector3(3, transform.position.y, transform.position.z);
        }
    }

    private bool IsTouchBegan()
    {
        if (Input.touchCount > 0)
        {
            var t = Input.GetTouch(0);
            return t.phase == TouchPhase.Began;
        }
        return false;
    }

    public void SetGroundCheck(bool check)
    {
        _isGround = check;
        if (check)
        {
            // _animator.SetBool("isJump", false);
            _animator.CrossFade("Fire_Running", 0.2f);
        }
    }
    private int score = 0;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Item"))
        {
            score+=20;
            AudioManager.instance.PlayItemSound();
            UiManager.SetScore(score);
            Debug.Log("Collected item! Score: " + score);
            other.gameObject.SetActive(false);
        }
        if (other.CompareTag("Razor"))
        {
            other.gameObject.SetActive(false);
            UiManager.SetHealth(20);
        }
    }
}
