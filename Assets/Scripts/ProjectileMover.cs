using UnityEngine;
using System.Collections;

public class ProjectileMover : MonoBehaviour {

	public float speed;
	private Rigidbody rb;

    void Start ()
	{	
		rb = GetComponent<Rigidbody> ();

		rb.velocity = transform.forward * speed;
	}

	void Update () {
		if (gameObject.activeInHierarchy == true) {
			rb.velocity = transform.forward * speed;
		} else {
			rb.velocity = Vector3.zero;
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
