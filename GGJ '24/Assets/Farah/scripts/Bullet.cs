using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Vector2 moveDirection;
    private float moveSpeed;
    private Rigidbody2D rb;

    private void OnEnable()
    {
        rb = GetComponent<Rigidbody2D>();
        Invoke("Destroy", 3f);
    }

    private void Start()
    {
        moveSpeed = 5f;
    }

    private void Update()
    {
        transform.Translate(moveDirection * moveSpeed * Time.deltaTime);
    }

    public void SetMoveDirection(Vector2 dir)
    {
        moveDirection = dir;
    }

    private void Destroy()
    {
        gameObject.SetActive(false);
    }

    private void OnDisable()
    {
        CancelInvoke();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            gameObject.SetActive(false);
        }
    }
}
