using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Horizontal Move")]
    public float maximumAcceleration;
    public float horizontalAcceleration;
    [Range(0f, 1f)]
    public float horizontalDamping;

    [Header("Jump Variable")]
    public LayerMask groundMask;
    [Range(0f, 1f)]
    public float cutJumpHeight;
    public float jumpPressedRememberTime;
    public float groundRememberTime;
    public float jumpPower;
    float jumpPressedRemember;
    float groundRemember;
    bool isJump = false;
    bool isGrounded;

    [Header("Rocket Data")]
    public float rocketPower;
    public float rocketBoostTime;
    public float rocketTime;
    public float rocketRechargeTime;
    float rocketTimeCount;
    public ParticleSystem rocketParticle;
    bool useRocket;
    bool canRocket;

    Rigidbody2D _rigidBody;
    Animator _animator;

    private void Awake()
    {
        _rigidBody = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();

        rocketTimeCount = rocketTime;
        UIManager.Instance.rocketBar.SetMaxAmount(rocketTime);

        rocketParticle.Stop();
    }

    private void FixedUpdate()
    {
        Move();
        Jump();
    }

    private void Update()
    {
        if (Input.GetButtonDown("Jump"))
        {
            isJump = true;
        }
        else if (Input.GetButtonUp("Jump"))
        {
            isJump = false;
        }

        Rocket();
    }

    public void FlipPlayer(float input)
    {
        if (input > 0)
            transform.eulerAngles = new Vector3(0, 0, 0);
        else if (input < 0)
            transform.eulerAngles = new Vector3(0, 180, 0);
    }

    void Rocket()
    {
        if (Input.GetKey(KeyCode.W) && canRocket)
        {
            if (rocketTimeCount > 0)
            {
                useRocket = true;
                _rigidBody.velocity = new Vector2(_rigidBody.velocity.x, rocketPower);
                rocketParticle.Play();
                rocketTimeCount -= Time.deltaTime * rocketBoostTime;
                UIManager.Instance.rocketBar.SetAmount(rocketTimeCount);
            }
            else
            {
                rocketParticle.Stop();
                canRocket = false;
            }
        }

        if (Input.GetKeyUp(KeyCode.W))
        {
            useRocket = false;
            rocketParticle.Stop();
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

    void Move()
    {
        float horizontalVelocity = _rigidBody.velocity.x;
        float input = Input.GetAxisRaw("Horizontal");

        FlipPlayer(input);
        _animator.SetFloat("speed", Mathf.Abs(input));

        horizontalAcceleration = Mathf.Clamp(maximumAcceleration - (GameManager.Instance.gameSpeed * 0.95f), 5f, 11f);

        horizontalVelocity += input;
        horizontalVelocity *= Mathf.Pow(1f - horizontalDamping, Time.deltaTime * horizontalAcceleration);
        _rigidBody.velocity = new Vector2(horizontalVelocity, _rigidBody.velocity.y);
    }

    void Jump()
    {
        Vector2 groundChectPoint = (Vector2)transform.position + new Vector2(0, -0.02f);
        isGrounded = Physics2D.OverlapBox(
                groundChectPoint,
                new Vector3(transform.localScale.x / 2, transform.localScale.y * 1.35f, transform.localScale.z),
                0, groundMask
            );

        groundRemember -= Time.deltaTime;
        if (isGrounded)
        {
            groundRemember = groundRememberTime;
            canRocket = false;
            _animator.SetBool("isJumping", false);
        }
        else if (!isGrounded)
        {
            canRocket = true;
            _animator.SetBool("isJumping", true);
        }

        jumpPressedRemember -= Time.deltaTime;
        if (isJump)
        {
            canRocket = true;
            jumpPressedRemember = jumpPressedRememberTime;
        }

        /*if (!isJump)
        {
            if (_rigidBody.velocity.y > 0)
            {
                _rigidBody.velocity = new Vector2(_rigidBody.velocity.x, _rigidBody.velocity.y * cutJumpHeight);
            }
        }*/

        if ((jumpPressedRemember > 0) && (groundRemember > 0))
        {
            _animator.SetTrigger("takeOff");
            jumpPressedRemember = 0;
            groundRemember = 0;
            _rigidBody.velocity = new Vector2(_rigidBody.velocity.x, jumpPower);
        }
    }
}
