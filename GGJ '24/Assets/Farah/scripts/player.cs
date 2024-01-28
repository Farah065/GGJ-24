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
    public Sprite[] clubs;
    public Sprite[] diamonds;
    public Sprite[] hearts;
    public Sprite[] spades;

    public GameObject club;
    private SpriteRenderer cRen;
    public GameObject diamond;
    private SpriteRenderer dRen;
    public GameObject heart;
    private SpriteRenderer hRen;
    public GameObject spade;
    private SpriteRenderer sRen;

    private ParticleSystem ps;
    public Material playerMaterial;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        audioSrc = GetComponent<AudioSource>();
        ps = GetComponent<ParticleSystem>();

        cRen = club.GetComponent<SpriteRenderer>();
        dRen = diamond.GetComponent<SpriteRenderer>();
        hRen = heart.GetComponent<SpriteRenderer>();
        sRen = spade.GetComponent<SpriteRenderer>();

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
        if (hp == 8)
        {
            sRen.sprite = spades[0];
        }
        else if (hp == 7)
        {
            sRen.sprite = spades[1];
        }
        else if(hp == 6)
        {
            sRen.sprite = spades[2];
            hRen.sprite = hearts[0];
        }
        else if(hp == 5)
        {
            hRen.sprite = hearts[1];
        }
        else if (hp == 4)
        {
            hRen.sprite = hearts[2];
            dRen.sprite = diamonds[0];
        }
        else if (hp == 3)
        {
            dRen.sprite = diamonds[1];
        }
        else if (hp == 2)
        {
            dRen.sprite = diamonds[2];
            cRen.sprite = clubs[0];
        }
        else if (hp == 1)
        {
            cRen.sprite = clubs[1];
        }
        else if (hp == 0)
        {
            cRen.sprite = clubs[2];
        }
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
                    SpriteRenderer ren = gameObject.GetComponent<SpriteRenderer>();
                    ren.enabled = false;

                    if (hp == 0)
                    {
                        ps.Stop();
                        ParticleSystem.Burst[] bursts = {new ParticleSystem.Burst(0, 1, 1, 1, 0.2f)};
                        ps.emission.SetBursts(bursts);
                        var main = ps.main;
                        main.startLifetime = 3;
                        main.startSpeed = 10;
                        main.startSize = 1.3f;
                        main.gravityModifier = 3;
                        var size = ps.sizeOverLifetime;
                        size.enabled = false;
                        var rotation = ps.rotationOverLifetime;
                        rotation.enabled = true;
                        ParticleSystemRenderer pr = ps.GetComponent<ParticleSystemRenderer>();
                        pr.material = playerMaterial;
                        ps.Play();
                    }
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
