using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class shooting : MonoBehaviour
{
    public GameObject bullet;
    Transform firePoint;
    public float bulletSpeed;
    public bool coolDown = false;
    public Transform player;
    // Start is called before the first frame update
    void Start()
    {
        firePoint = GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 lookDirection = Camera.main.ScreenToWorldPoint(Input.mousePosition) - new Vector3(transform.position.x, transform.position.y);
        float lookAngle = Mathf.Atan2(lookDirection.y, lookDirection.x) * Mathf.Rad2Deg;
        firePoint.rotation = Quaternion.Euler(0, 0, lookAngle);
        if (Input.GetKey(KeyCode.Mouse0) && !coolDown)
        {
            coolDown = true;
            GameObject ball = Instantiate(bullet, firePoint.position, Quaternion.Euler(0, 0, lookAngle));
            ball.GetComponent<Rigidbody2D>().velocity = firePoint.right*bulletSpeed;
            StartCoroutine(Cool());
        }
    }

    IEnumerator Cool()
    {
        yield return new WaitForSeconds(0.5f);
        coolDown =false;
    }
}
