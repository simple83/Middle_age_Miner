using System;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private SpriteRenderer spriteRenderer;

    public float linearMoveSpeed = 5f;
    private Rigidbody2D rb;
    private Vector2 movement;
    private Vector3 originScale;

    private bool isLandingOnPlatform => rb.linearVelocityY == 0;
    private bool isTouchingLeftWall = false;
    private bool isTouchingRightWall = false;

    public float flyForce = 30f;        // 위로 가하는 힘
    public float maxFlySpeed = 10f;     // 최대 비행 속도
    public float maxFallSpeed = 30f;    // 최대 낙하 속도

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        originScale = transform.localScale;
    }

    private int horizontalDirection = 1;

    private void SetXDirection(float moveX)
    {
        if (moveX == 0) return;
        var direction = 1;
        if (moveX < 0)
        {
            direction = -1;
        }
        if (horizontalDirection == direction) return;
        horizontalDirection = direction;
        var newScale = transform.localScale;
        newScale.x = horizontalDirection;
        transform.localScale = newScale;
    }

    private void Update()
    {
        UpdatePlayerAnimation();
    }

    void UpdatePlayerAnimation()
    {
        animator.SetBool("isOnPlatform", isLandingOnPlatform);
        animator.SetBool("isWalk", isLandingOnPlatform && rb.linearVelocityX != 0);
        animator.SetBool("isFlying", Input.GetKey(KeyCode.UpArrow));
    }

    void FixedUpdate()
    {
        // 수평방향 움직임. 등속도 운동
        float moveX = Input.GetAxisRaw("Horizontal");
        rb.linearVelocityX = moveX * linearMoveSpeed;
        SetXDirection(moveX);

        // 수직방향 움직임. 가속도 운동
        if (Input.GetKey(KeyCode.UpArrow))
        {
            if (rb.linearVelocityY < maxFlySpeed)
            {
                rb.AddForce(Vector2.up * flyForce, ForceMode2D.Force);
            }

        }

        // 최대 낙하속도
        if( rb.linearVelocityY > maxFallSpeed)
        {
            rb.linearVelocityY = maxFallSpeed;
        }
    }
}
