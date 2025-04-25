using System;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public float linearMoveSpeed = 5f;
    private Rigidbody2D rb;
    private Vector2 movement;

    private bool isTouchingLeftWall = false;
    private bool isTouchingRightWall = false;

    public float flyForce = 30f;        // ���� ���ϴ� ��
    public float maxFlySpeed = 10f;     // �ִ� ���� �ӵ�
    public float maxFallSpeed = 30f;    // �ִ� ���� �ӵ�

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        
    }

    void FixedUpdate()
    {
        // ������� ������. ��ӵ� �
        float moveX = Input.GetAxisRaw("Horizontal");
        rb.linearVelocityX = moveX * linearMoveSpeed;

        // �������� ������. ���ӵ� �
        if (Input.GetKey(KeyCode.UpArrow))
        {
            if (rb.linearVelocityY < maxFlySpeed)
            {
                rb.AddForce(Vector2.up * flyForce, ForceMode2D.Force);
            }

        }

        // �ִ� ���ϼӵ�
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
                // ���� �÷��̾��� �����ʿ� ���� ���
                if (contact.normal.x > 0.5f)
                    isTouchingLeftWall = isEntering;
                // ���� �÷��̾��� ���ʿ� ���� ���
                else if (contact.normal.x < -0.5f)
                    isTouchingRightWall = isEntering;
            }
        }
    }
}
