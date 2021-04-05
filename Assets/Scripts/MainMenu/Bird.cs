using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bird : MonoBehaviour
{
    private Vector3 direction;
    private float speed;

    public float suicideTimer = 10f;

    void Start()
    {
        direction = new Vector3(Random.Range(80f, 120f), Random.Range(60f, 100f), transform.position.z);
        speed = Random.Range(35f, 55f);
        Destroy(gameObject, suicideTimer);
    }

    void FixedUpdate()
    {
        speed += 0.02f;
        float step = speed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, direction, step);
    }
}
