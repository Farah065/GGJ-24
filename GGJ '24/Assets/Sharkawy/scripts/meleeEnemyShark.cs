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
    // Start is called before the first frame update
    void Start()
    {
        enemy = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody2D>();
        trajectory = (player.transform.position - enemy.transform.position).normalized;
    }

    // Update is called once per frame
    void Update()
    {
        enemy.velocity = enemySpeed * trajectory;
        Vector2 optimal = (player.transform.position - enemy.transform.position).normalized;
        trajectory = (Vector2.Lerp(trajectory, optimal, 0.2f)).normalized;
    }
}
