using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator anim;

    public float moveSpeed;
    public float forceDamping;
    private Vector2 forceToApply;
    private Vector2 PlayerInput;

    public bool canMove;
    public bool invincible;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
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
            
            if(rb.velocity.x > 0)
            {
                anim.SetInteger("x", 1);
            }
            else if(rb.velocity.x < 0)
            {
                anim.SetInteger("x", -1);
            }
            else
            {
                anim.SetInteger("x", 0);
            }

            if (rb.velocity.y > 0)
            {
                anim.SetInteger("y", 1);
            }
            else if (rb.velocity.y < 0)
            {
                anim.SetInteger("y", -1);
            }
            else
            {
                anim.SetInteger("y", 0);
            }
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
