using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements.Experimental;

public class Player : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator anim;

    public AudioClip[] stepClips;
    private AudioSource audioSrc;

    public float moveSpeed;
    public float forceDamping;
    private Vector2 forceToApply;
    private Vector2 PlayerInput;

    public float dashTimer;
    public float dashBoost;
    public bool dashing;

    public bool canMove;
    public bool invincible;

    public int hp;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        audioSrc = GetComponent<AudioSource>();

        canMove = true;
        invincible = false;
        dashing = false;

        dashTimer = 0;
        dashBoost = 20;

        hp = 8;

        StartCoroutine(steps());
    }
    void Update()
    {
        PlayerInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;
        
    }
    void FixedUpdate()
    {
        if (canMove)
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

            if (dashTimer <= 0 && Input.GetKey(KeyCode.Space) && rb.velocity != Vector2.zero)
            {
                StartCoroutine(Dash(rb.velocity.normalized));
            }

            if (rb.velocity.x > 1)
                anim.SetInteger("x", 1);
            else if(rb.velocity.x < -1)
                anim.SetInteger("x", -1);
            else
                anim.SetInteger("x", 0);

            if (rb.velocity.y > 1)
                anim.SetInteger("y", 1);
            else if (rb.velocity.y < -1)
                anim.SetInteger("y", -1);
            else
                anim.SetInteger("y", 0);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Bullet"))
        {
            if (!invincible)
            {
                collision.gameObject.SetActive(false);
                hp -= 1;
                if (hp <= 0)
                {
                    canMove = false;
                    Debug.Log("you died");
                    rb.velocity = new Vector2(0, 0);
                    Collider2D c = gameObject.GetComponent<Collider2D>();
                    c.enabled = false;
                    return;
                }
                forceToApply = new Vector2(collision.rigidbody.velocity.x * 1.3f, collision.rigidbody.velocity.y * 1.3f);
                StartCoroutine(hit());
                StartCoroutine(stopMove());
            }
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

    IEnumerator steps()
    {
        while (Mathf.Abs(rb.velocity.x) <= 1 && Mathf.Abs(rb.velocity.y) <= 1)
            yield return null;
        int i = UnityEngine.Random.Range(0, 4);
        audioSrc.PlayOneShot(stepClips[i]);
        yield return new WaitForSeconds(0.3f);
        StartCoroutine(steps());
    }
}
