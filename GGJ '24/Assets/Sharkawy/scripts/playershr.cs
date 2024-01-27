using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class playershr : MonoBehaviour
{
    public Rigidbody2D rb;
    public float moveSpeed;
    public Vector2 forceToApply;
    public Vector2 PlayerInput;
    public float forceDamping;
    public float dashTimer = 0;
    public float dashBoost;
    public bool dashing = false;
    void Update()
    {
        PlayerInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;
    }
    void FixedUpdate()
    {
        if (!dashing)
        {
            Vector2 moveForce = PlayerInput * moveSpeed;
            if (PlayerInput == Vector2.zero)
            {
                moveForce = rb.velocity * 0.9f;
            }
            moveForce += forceToApply;
            forceToApply /= forceDamping;
            if (Mathf.Abs(forceToApply.x) <= 0.01f && Mathf.Abs(forceToApply.y) <= 0.01f)
            {
                forceToApply = Vector2.zero;
            }
            rb.velocity = moveForce;
            dashTimer -= Time.deltaTime;
        }
        if(dashTimer <= 0 && Input.GetKey(KeyCode.Space) && rb.velocity != Vector2.zero) {
            StartCoroutine(Dash(rb.velocity.normalized));
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Bullet"))
        {
            // temp code, replace with some sort of condition for bullet direction
            forceToApply = new Vector2(PlayerInput.x * -15f, PlayerInput.y * -15f);
            Destroy(collision.gameObject);
        }
    }
    IEnumerator Dash(Vector2 dir)
    {
        dashTimer = 3;
        dashing = true;
        rb.velocity = dir.normalized * dashBoost;
        yield return new WaitForSeconds(0.1f);
        dashing = false;
    }
    
}
