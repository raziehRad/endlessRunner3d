using System;
using System.Collections;
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
    private bool moveLeft;
    private bool moveRight;
    private bool jumping;
    public UIManager UiManager=>_uiManager;
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        JumpAction();
        MevementAction();
        if (moveLeft)
            MoveLeft();
        if (moveRight)
            MoveRight();
        if (transform.position.y<-10)
        {
            _uiManager.SaveData();
        }
    }

    private void JumpAction()
    {
        if (_isGround && (Input.GetButton("Jump")|| jumping))
        {
            rb.AddForce(Vector3.up * _jumpForce, ForceMode.Impulse);
            AudioManager.instance.PlayJumpSound();
            _animator.CrossFade("Jumping",0.1f);
        }
    }

    public void JumpButton()
    {
        jumping = !jumping;
    }
    private void MevementAction()
    {
        var horizontal = Input.GetAxis("Horizontal");
        transform.Translate(horizontal * _moveSpeed * Time.deltaTime, 0, 0);
        if (transform.position.x < -3)
            transform.position = new Vector3(-3, transform.position.y, transform.position.z);
        
        if (transform.position.x > 3)
            transform.position = new Vector3(3, transform.position.y, transform.position.z);
    }
    public void OnLeftButtonDown()
    {
        moveLeft = true;
    }

    public void OnLeftButtonUp()
    {
        moveLeft = false;
    }

    public void OnRightButtonDown()
    {
        moveRight = true;
    }

    public void OnRightButtonUp()
    {
        moveRight = false;
    }
    public void MoveRight()
    {
        transform.Translate(Vector3.right * _moveSpeed * Time.deltaTime);
        if (transform.position.x > 3)
            transform.position = new Vector3(3, transform.position.y, transform.position.z);
    }

    public void MoveLeft()
    {
        transform.Translate(Vector3.left * _moveSpeed * Time.deltaTime);
        if (transform.position.x < -3)
            transform.position = new Vector3(-3, transform.position.y, transform.position.z);
    }
    
    private bool IsTouchBegan()
    {
        // if (Input.touchCount > 0)
        // {
        //     var t = Input.GetTouch(0);
        //     return t.phase == TouchPhase.Began;
        // }
        return false;
    }

    public void SetGroundCheck(bool check)
    {
        _isGround = check;
        if (check)
        {
            jumping = false;
            _animator.CrossFade("Fire_Running", 0.2f);
        }
    }
    public IEnumerator PlayDamageAnimation()
    {
        _animator.CrossFade("damage1", 0.5f);
        
        AnimatorStateInfo info = _animator.GetCurrentAnimatorStateInfo(0);
        yield return new WaitForSeconds(info.length);

        _animator.CrossFade("Fire_Running", 0.2f);
    }
}
