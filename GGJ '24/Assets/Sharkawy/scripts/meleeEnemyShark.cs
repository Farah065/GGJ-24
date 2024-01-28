using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class meleeEnemyShark : MonoBehaviour
{
    Rigidbody2D enemy;
    Rigidbody2D player;
    Vector2 trajectory;
    public float enemySpeed;
    Animator anim;
    Transform tr;
    // Start is called before the first frame update
    void Start()
    {
        enemy = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody2D>();
        trajectory = (player.transform.position - enemy.transform.position).normalized;

        anim = GetComponent<Animator>();
        tr = GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 scale = tr.localScale;
        scale.x = Mathf.Sign(trajectory.x);
        tr.localScale = scale;
        float d = Mathf.Sqrt(Mathf.Pow(player.position.x - transform.position.x, 2) + Mathf.Pow(player.position.y - transform.position.y, 2));
        if (d > 3)
        {
            enemy.velocity = enemySpeed * trajectory;
            Vector2 optimal = (player.transform.position - enemy.transform.position).normalized;
            trajectory = (Vector2.Lerp(trajectory, optimal, 0.2f)).normalized;
        }
        else
        {
            enemy.velocity = Vector3.zero;
        }

        float s = Mathf.Sqrt(Mathf.Pow(enemy.velocity.x, 2) + Mathf.Pow(enemy.velocity.y, 2));
        anim.SetFloat("speed", s);
    }
}
