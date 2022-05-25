using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowingProjectile : MonoBehaviour {

	public GameObject enemyToFollow;

	public float moveSpeed = 15;
	
	// Update is called once per frame
	void Update ()
	{
		if (enemyToFollow == null) {
			this.gameObject.SetActive (false);
		}
		else
		{
			transform.right = enemyToFollow.transform.position - transform.position;
			GetComponent<Rigidbody> ().velocity = transform.right * moveSpeed;
		}
	}

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
			this.gameObject.SetActive(false);
        }
    }
}
