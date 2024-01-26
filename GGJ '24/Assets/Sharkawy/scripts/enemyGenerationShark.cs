using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class enemyGenerationShark : MonoBehaviour
{
    public Rigidbody2D rb;
    public float a;
    public float b;
    bool spawn = true;
    public GameObject enemy;
    public float initialRate;
    public float maxRate;
    public float currRate;
    public float increment;
    float timeLapsed = 0;
    float spawnRate;
    // Start is called before the first frame update
    void Start()
    {
        currRate = initialRate;
        spawnRate = 1 /currRate;
    }

    // Update is called once per frame
    void Update()
    {
        if (spawn)
        {
            StartCoroutine(SpawnNew());
        }
    }

    IEnumerator SpawnNew()
    {
        spawn = false;
        System.Random rnd = new System.Random();
        float x = (float)rnd.NextDouble() * 29 - 15;
        float y = b * b * (x * x / (a * a) - 1);
        Instantiate(enemy, new Vector3(rb.transform.position.x + x, rb.transform.position.y + y, 0), Quaternion.Euler(0, 0, 0));
        yield return new WaitForSeconds(spawnRate);
        currRate += increment;
        float rate = Mathf.Min(maxRate, currRate);
        spawnRate = 1 / rate;
        spawn = true;
    }
}
