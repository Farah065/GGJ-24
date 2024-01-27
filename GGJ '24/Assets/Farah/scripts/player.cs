using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Rigidbody2D rb;
    public float moveSpeed;
    public Vector2 forceToApply;
    public Vector2 PlayerInput;
    public float forceDamping;
    public bool canMove;
    public bool invincible;

    private void Start()
    {
        canMove = true;
        invincible = false;
    }
    void Update()
    {
        PlayerInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;
    }
    void FixedUpdate()
    {
        if (canMove)
        {
            Vector2 moveForce = PlayerInput * moveSpeed;
            moveForce += forceToApply;
            forceToApply /= forceDamping;
            if (Mathf.Abs(forceToApply.x) <= 0.01f && Mathf.Abs(forceToApply.y) <= 0.01f)
            {
                forceToApply = Vector2.zero;
            }
            rb.velocity = moveForce;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Bullet"))
        {
            if (!invincible)
            {
                forceToApply = new Vector2(collision.rigidbody.velocity.x * 1.3f, collision.rigidbody.velocity.y * 1.3f);
                StartCoroutine(hit());
                StartCoroutine(stopMove());
            }
            collision.gameObject.SetActive(false);
        }
    }

    private IEnumerator hit()
    {
        invincible = true;
        for (int i = 0; i < 3; i++)
        {
            Color tmp = GetComponent<SpriteRenderer>().color;
            yield return new WaitForSeconds(0.2f);
            tmp.a = 0.6f;
            GetComponent<SpriteRenderer>().color = tmp;

            yield return new WaitForSeconds(0.2f);
            tmp.a = 1f;
            GetComponent<SpriteRenderer>().color = tmp;
        }
        invincible = false;
    }

    private IEnumerator stopMove()
    {
        canMove = false;
        yield return new WaitForSeconds(0.2f);
        canMove = true;
    }
}
