using System;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public float linearMoveSpeed = 5f;
    private Rigidbody2D rb;
    private Vector2 movement;

    private bool isTouchingLeftWall = false;
    private bool isTouchingRightWall = false;

    public float flyForce = 30f;        // 위로 가하는 힘
    public float maxFlySpeed = 10f;     // 최대 비행 속도
    public float maxFallSpeed = 30f;    // 최대 낙하 속도

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        
    }

    void FixedUpdate()
    {
        // 수평방향 움직임. 등속도 운동
        float moveX = Input.GetAxisRaw("Horizontal");
        rb.linearVelocityX = moveX * linearMoveSpeed;

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

    void OnCollisionEnter2D(Collision2D collision)
    {
        CheckWallCollision(collision, true);
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        CheckWallCollision(collision, false);
    }

    void CheckWallCollision(Collision2D collision, bool isEntering)
    {
        if (collision.gameObject.CompareTag("Wall"))
        {
            foreach (ContactPoint2D contact in collision.contacts)
            {
                // 벽이 플레이어의 오른쪽에 있을 경우
                if (contact.normal.x > 0.5f)
                    isTouchingLeftWall = isEntering;
                // 벽이 플레이어의 왼쪽에 있을 경우
                else if (contact.normal.x < -0.5f)
                    isTouchingRightWall = isEntering;
            }
        }
    }
}
