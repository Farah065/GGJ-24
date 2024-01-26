using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class FireBullets : MonoBehaviour
{
    [SerializeField]
    private int bulletAmount;

    [SerializeField]
    private float startAngle, endAngle;

    private Vector2 bulletMoveDirection;

    private GameObject player;

    private void Start()
    {
        player = GameObject.FindWithTag("Player");

        InvokeRepeating("Fire", 0f, 2f);

        Vector3 directionToTarget = player.transform.position - transform.position;

        float angleRadians = Mathf.Atan2(directionToTarget.y, directionToTarget.x);
        float angleDegrees = angleRadians * Mathf.Rad2Deg;

        startAngle = -angleDegrees + 30;
        endAngle = -angleDegrees + 150;
    }

    private void Update()
    {
        Vector3 directionToTarget = player.transform.position - transform.position;

        float angleRadians = Mathf.Atan2(directionToTarget.y, directionToTarget.x);
        float angleDegrees = angleRadians * Mathf.Rad2Deg;

        startAngle = -angleDegrees + 30;
        endAngle = -angleDegrees + 150;
    }

    private void Fire()
    {
        float angleStep = (endAngle - startAngle) / bulletAmount;
        float angle = startAngle;

        for (int i = 0; i <= bulletAmount; i++)
        {
            float bulDirX = transform.position.x + Mathf.Sin((angle * Mathf.PI) / 180f);
            float bulDirY = transform.position.y + Mathf.Cos((angle * Mathf.PI) / 180f);

            Vector3 bulMoveVector = new Vector3(bulDirX, bulDirY, 0f);
            Vector2 bulDir = (bulMoveVector - transform.position).normalized;

            GameObject bul = BulletPool.bulletPoolInstance.GetBullet();
            bul.transform.position = transform.position;
            bul.transform.rotation = transform.rotation;
            bul.SetActive(true);
            bul.GetComponent<Bullet>().SetMoveDirection(bulDir);

            angle += angleStep;
        }
    }
}
