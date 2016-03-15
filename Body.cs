using UnityEngine;
using System.Collections;

public class Body : MonoBehaviour {

	public float maxSpeed;

	public Vector2 accel; //sum of all accelerations this body experiences from any nearby gravities

	public Rigidbody2D rb; 
	// Use this for initialization
	void Start () {
		rb = GetComponent<Rigidbody2D> ();

		maxSpeed = 3;

		StartCoroutine (UpdateVelocity ());
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnCollisionEnter2D()
	{
		gameObject.SetActive (false);
	}

	IEnumerator UpdateVelocity()
	{
		while (true)
		{
			yield return new WaitForFixedUpdate();
			
			//update velocity
			rb.velocity += accel * Time.fixedDeltaTime;

			accel = Vector2.zero;

			//if speed exceeds max speed
			if(rb.velocity.magnitude > maxSpeed)
			{	
				rb.velocity = rb.velocity.normalized * maxSpeed;
			}
		}
	}
}
