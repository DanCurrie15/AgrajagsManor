using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.HealthSystemCM;

public enum PlayerType { Human, Bookcase, Lamp}

public class Player : MonoBehaviour, IGetHealthSystem
{
    private CharacterController controller;
    private Vector3 playerVelocity;
    private float playerSpeed = 2.0f;
    private float gravityValue = -9.81f;
    private int playerHealth = 10;
    private int damageAmount = 0;
    private float damageRate = 1.5f;
    private float nextDamage = 0f;
    private Animator animator;
    private HealthSystem healthSystem;
    private float projectileRate = 0.5f;
    private float nextprojectile = 0;

    [SerializeField]
    private Transform shovelPivot;
    [SerializeField]
    private GameObject shovel;
    [SerializeField]
    private GameObject projectile;
    [SerializeField]
    private Transform projectileFireLocaiton;
    [SerializeField]
    private GameObject deadPlayer;
    [SerializeField]
    private PlayerType playerType;

    private void Awake()
    {
        healthSystem = new HealthSystem(playerHealth);
    }

    void Start()
    {
        controller = gameObject.GetComponent<CharacterController>();
        UIManager.Instance.SetPlayerMaxHealth(playerHealth);

        if (playerType == PlayerType.Human)
        {
            animator = GetComponent<Animator>();
        }
    }

    void Update()
    {
        if (GameManager.Instance.GameOn && GameManager.Instance.playerChar == this.gameObject)
        {
            Vector3 move = new Vector3(-Input.GetAxis("Horizontal"), 0, -Input.GetAxis("Vertical"));
            controller.Move(move * Time.deltaTime * playerSpeed);

            if (move != Vector3.zero)
            {
                gameObject.transform.forward = move;
            }

            playerVelocity.y += gravityValue * Time.deltaTime;
            controller.Move(playerVelocity * Time.deltaTime);

            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (playerType == PlayerType.Human)
                {
                    animator.SetTrigger("attack");
                    shovel.tag = "Weapon";
                    StartCoroutine(EndShovelAttack());
                }
                else if (playerType == PlayerType.Bookcase)
                {
                    if (Time.time > nextprojectile)
                    {
                        nextprojectile = Time.time + projectileRate;
                        GameObject bookProjectile = Instantiate(projectile, projectileFireLocaiton.position, projectileFireLocaiton.rotation);
                    }                    
                }
                else if (playerType == PlayerType.Lamp)
                {
                    if (Time.time > nextprojectile)
                    {
                        nextprojectile = Time.time + projectileRate;
                        GameObject lampProjectile = Instantiate(projectile, projectileFireLocaiton.position, projectileFireLocaiton.rotation);
                        lampProjectile.GetComponent<FollowingProjectile>().enemyToFollow = EnemyManager.Instance.GetClosestEnemy(this.gameObject.transform);
                    }                    
                }
            }
        }
        if (damageAmount > 0)
        {
            if (Time.time > nextDamage && GameManager.Instance.GameOn)
            {
                if (!EnemyManager.Instance.GetClosestEnemy(this.gameObject.transform)
                    || Vector3.Distance(this.transform.position, EnemyManager.Instance.GetClosestEnemy(this.gameObject.transform).transform.position) > 1f)
                {
                    damageAmount = 0;
                }
                nextDamage = Time.time + damageRate;
                LoseHealth();
            }
        }
    }

    IEnumerator EndShovelAttack()
    {
        yield return new WaitForSeconds(1.5f);
        shovel.tag = "Untagged";
    }

    private void LoseHealth()
    {
        playerHealth -= damageAmount;
        healthSystem.Damage(damageAmount);
        SoundManager.Instance.PlaySoundEffect(SoundEffect.HitHurt);
        //UIManager.Instance.SetPlayerCurrentHealth(playerHealth);
        if (playerHealth < 1)
        {
            Instantiate(deadPlayer, this.transform.position, Quaternion.identity);
            PlayerManager.Instance.RemovePlayablePlayer(this.gameObject);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            ++damageAmount;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            --damageAmount;
            if (damageAmount < 1)
            {
                damageAmount = 0;
            }
        }
    }

    public HealthSystem GetHealthSystem()
    {
        return healthSystem;
    }
}
