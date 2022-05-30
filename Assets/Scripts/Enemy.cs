using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using CodeMonkey.HealthSystemCM;

public class Enemy : MonoBehaviour, IGetHealthSystem
{
    private GameObject player;
    private float speed = 0.01f;
    private Animator animator;
    private NavMeshAgent agent;
    private float enemyHealth = 2;
    private HealthSystem healthSystem;

    private void Awake()
    {
        healthSystem = new HealthSystem(enemyHealth);
        healthSystem.OnDead += HealthSystem_OnDead;
    }

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
            Debug.Log("Hit");
            healthSystem.Damage(1);
            --enemyHealth;
            if (enemyHealth < 1)
            {
                SoundManager.Instance.PlaySoundEffect(SoundEffect.ZombieGroan);
                Destroy(this.gameObject, 0.1f);
                GameManager.Instance.AddToKillCount();
                EnemyManager.Instance.enemies.Remove(this.gameObject);
            }            
        }
    }

    private void HealthSystem_OnDead(object sender, System.EventArgs e)
    {
        Destroy(gameObject);
    }

    public HealthSystem GetHealthSystem()
    {
        return healthSystem;
    }
}
