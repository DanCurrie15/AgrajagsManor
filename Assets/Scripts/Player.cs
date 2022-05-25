using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerType { Human, Bookcase, Lamp}

public class Player : MonoBehaviour
{
    private CharacterController controller;
    private Vector3 playerVelocity;
    private float playerSpeed = 2.0f;
    private float gravityValue = -9.81f;
    private int playerHealth = 5;

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

    void Start()
    {
        controller = gameObject.GetComponent<CharacterController>();
        UIManager.Instance.SetPlayerMaxHealth(playerHealth);
    }

    void Update()
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
                if (shovel.transform.eulerAngles.y < 45)
                {
                    shovel.transform.RotateAround(shovelPivot.position, Vector3.up, 90);
                }
                else
                {
                    shovel.transform.RotateAround(shovelPivot.position, Vector3.up, -90);
                }
            }
            else if (playerType == PlayerType.Bookcase)
            {
                GameObject bookProjectile = Instantiate(projectile, projectileFireLocaiton.position, Quaternion.identity);
            }
            else if (playerType == PlayerType.Lamp)
            {
                GameObject lampProjectile = Instantiate(projectile, projectileFireLocaiton.position, Quaternion.identity);
                lampProjectile.GetComponent<FollowingProjectile>().enemyToFollow = EnemyManager.Instance.GetClosestEnemy(this.gameObject.transform);
            }
        }
    }

    private void LoseHealth()
    {
        playerHealth--;
        UIManager.Instance.SetPlayerCurrentHealth(playerHealth);
        if (playerHealth < 1)
        {
            Debug.Log("Possess New Object");
            Instantiate(deadPlayer, this.transform.position, Quaternion.identity);
            PlayerManager.Instance.RemovePlayablePlayer(this.gameObject);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Debug.Log("hit by enemy");
            LoseHealth();
        }
    }
}
