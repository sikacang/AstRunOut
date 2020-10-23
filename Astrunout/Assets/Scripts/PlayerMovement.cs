using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float runSpeed = 0;
    public float jumpPower;
    public bool sedangLompat;

    [Header("Ground Checker")]
    public Transform groundPoint;
    public LayerMask whatIsGround;
    public float checkRadius;
    public bool isGrounded;

    [Header("Rocket Data")]
    public float rocketPower;
    public float rocketBoostTime;
    public float rocketTime;
    public float rocketRechargeTime;
    float rocketTimeCount;
    public ParticleSystem rocketParticle;
    bool useRocket;

    Rigidbody2D _rigidBody;
    Animator _animator;

    private void Awake()
    {
        _rigidBody = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();

        runSpeed = GameManager.Instance.gameSpeed * 1.5f;
        rocketTimeCount = rocketTime;
        UIManager.Instance.rocketBar.SetMaxAmount(rocketTime);

        rocketParticle.Stop();
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void Update()
    {
        isGrounded = Physics2D.OverlapCircle(groundPoint.position, checkRadius, whatIsGround);

        Jump();
    }

    public void FlipPlayer(float input)
    {
        if (input > 0)
            transform.eulerAngles = new Vector3(0, 0, 0);
        else if (input < 0)
            transform.eulerAngles = new Vector3(0, 180, 0);
    }

    void Move()
    {
        float input = Input.GetAxisRaw("Horizontal");
        _animator.SetFloat("speed", Mathf.Abs(input));

        FlipPlayer(input);
        runSpeed = GameManager.Instance.gameSpeed * 1.5f;
        Vector2 direction = new Vector2(input * runSpeed, _rigidBody.velocity.y);
        _rigidBody.velocity = direction;
    }

    public void SetJumpAnimation()
    {
        _animator.SetTrigger("takeOff");
    }

    void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            SetJumpAnimation();
            AudioManager.Instance.Play("PlayerJump");
            _rigidBody.velocity = Vector2.up * jumpPower;
        }

        if (Input.GetKey(KeyCode.W) && sedangLompat)
        {
            if (rocketTimeCount > 0)
            {
                useRocket = true;
                _rigidBody.velocity = Vector2.up * rocketPower;
                rocketParticle.Play();
                rocketTimeCount -= Time.deltaTime * rocketBoostTime;
                UIManager.Instance.rocketBar.SetAmount(rocketTimeCount);
            }
            else
            {
                rocketParticle.Stop();
                sedangLompat = false;
            }
        }
        else if (Input.GetKeyUp(KeyCode.W))
        {
            useRocket = false;
            rocketParticle.Stop();
        }

        if (isGrounded)
        {
            sedangLompat = false;
            _animator.SetBool("isJumping", false);
            rocketParticle.Stop();
        }
        else if (!isGrounded)
        {
            sedangLompat = true;
            _animator.SetBool("isJumping", true);
        }


        if (rocketTimeCount <= rocketTime && !useRocket)
        {
            rocketTimeCount += Time.deltaTime * rocketRechargeTime;
            UIManager.Instance.rocketBar.SetAmount(rocketTimeCount);
        }
        else if (rocketTimeCount > rocketTime)
        {
            rocketTimeCount = rocketTime;
            UIManager.Instance.rocketBar.SetAmount(rocketTimeCount);
        }
    }

    public void AddForceToPlayer(float forcePower, Vector2 direction, float distance)
    {
        _rigidBody.velocity = direction * forcePower * distance;
    }
}
