using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private GameObject player;
    private float speed = 0.01f;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, player.transform.position, speed);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Weapon"))
        {
            this.gameObject.SetActive(false);
        }
    }
}
