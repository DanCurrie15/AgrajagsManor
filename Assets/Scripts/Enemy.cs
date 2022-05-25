using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private GameObject player;
    private float speed = 0.01f;
    private Animator animator;

    void Start()
    {
        EnemyManager.Instance.enemies.Add(this.gameObject);
        player = PlayerManager.Instance.GetClosestPlayer(this.gameObject.transform);
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (GameManager.Instance.GameOn)
        {
            if (!player)
            {
                player = PlayerManager.Instance.GetClosestPlayer(this.gameObject.transform);
            }
            transform.position = Vector3.MoveTowards(transform.position, player.transform.position, speed);

            if (Vector3.Distance(player.transform.position, this.transform.position) > 2f)
            {
                animator.SetBool("isAttacking", false);
            }
            else
            {
                animator.SetBool("isAttacking", true);
            }
        }        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Weapon"))
        {
            EnemyManager.Instance.enemies.Remove(this.gameObject);
            this.gameObject.SetActive(false);
        }
    }
}
