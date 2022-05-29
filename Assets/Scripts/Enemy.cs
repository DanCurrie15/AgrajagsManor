using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    private GameObject player;
    private float speed = 0.01f;
    private Animator animator;
    private NavMeshAgent agent;

    void Start()
    {
        EnemyManager.Instance.enemies.Add(this.gameObject);
        player = PlayerManager.Instance.GetClosestPlayer(this.gameObject.transform);
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        if (GameManager.Instance.GameOn)
        {
            if (!player.activeInHierarchy)
            {
                player = PlayerManager.Instance.GetClosestPlayer(this.gameObject.transform);
            }
            //transform.position = Vector3.MoveTowards(transform.position, player.transform.position, speed);
            agent.SetDestination(player.transform.position);
            transform.LookAt(player.transform, Vector3.up);

            if (Vector3.Distance(player.transform.position, this.transform.position) < 1.5f)
            {
                //Debug.Log("attack player");
                animator.SetBool("isAttacking", false);
            }
            else
            {
                //Debug.Log("stop attacking");
                animator.SetBool("isAttacking", true);
            }
        }        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Weapon"))
        {
            EnemyManager.Instance.enemies.Remove(this.gameObject);
            //this.gameObject.SetActive(false);
            Destroy(this.gameObject, 0.1f);
            GameManager.Instance.AddToKillCount();
        }
    }
}
