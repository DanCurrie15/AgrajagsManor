using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
            if (shovel.transform.eulerAngles.y < 45)
            {
                shovel.transform.RotateAround(shovelPivot.position, Vector3.up, 90);
            }
            else
            {
                shovel.transform.RotateAround(shovelPivot.position, Vector3.up, -90);
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
