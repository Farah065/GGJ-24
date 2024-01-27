using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class FireBullets : MonoBehaviour
{
    [SerializeField]
    private int bulletAmount;

    [SerializeField]
    private float angleToPlayer, spread;

    [SerializeField]
    private float bulSpeed;

    private GameObject player;

    private void Start()
    {
        player = GameObject.FindWithTag("Player");

        InvokeRepeating("Fire", 0f, 2f);

        Vector3 directionToTarget = player.transform.position - transform.position;
        angleToPlayer = Mathf.Atan2(directionToTarget.y, directionToTarget.x) * Mathf.Rad2Deg;
    }

    private void Update()
    {
        Vector3 directionToTarget = player.transform.position - transform.position;
        angleToPlayer = Mathf.Atan2(directionToTarget.y, directionToTarget.x) * Mathf.Rad2Deg;
    }

    private void Fire()
    {
        float angleStep = (spread) / bulletAmount - 1;
        float startAngle = -(angleToPlayer - spread);
        float curAngle = startAngle - 90;

        for (int i = 0; i < bulletAmount; i++)
        {
            float bulDirX = transform.position.x + Mathf.Sin((curAngle * Mathf.PI) / 180f);
            float bulDirY = transform.position.y + Mathf.Cos((curAngle * Mathf.PI) / 180f);

            Vector3 bulMoveVector = new Vector3(bulDirX, bulDirY, 0f);
            Vector2 bulDir = (bulMoveVector - transform.position).normalized;

            GameObject bul = BulletPool.bulletPoolInstance.GetBullet();
            Rigidbody2D bulRb = bul.GetComponent<Rigidbody2D>();
            bul.transform.position = transform.position;
            bul.transform.rotation = transform.rotation;
            bul.SetActive(true);
            bulRb.velocity = bulDir * bulSpeed;

            curAngle += angleStep;
        }
    }
}
