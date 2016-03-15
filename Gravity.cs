using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Gravity : MonoBehaviour {

	public Transform effectsObject; //child object carrying particle effects

	public ParticleSystem effects;

	public Transform visual;

	public float dir; //direction, can be -1 (attractive) or 1 (repulsive)

	public float radius; //radius of area of effect

	public float force; //gravitational force to use

	public float maxForce; //to limit "odd" behaviors (forces going off to infinity, along with whatever object was affected)

	public List<Body> bodies; //list of all bodies being acted upon by gravity

	public int numBodies; //how many bodies in the list

	public float coolDown; //regular cool down time

	public float timer; //cool down timer

	// Use this for initialization
	void Start () {
	
	}

	void OnEnable()
	{
		GetComponent<CircleCollider2D> ().radius = radius;

		effectsObject.localScale = Vector3.one * radius;

		effects.startSpeed = - radius / effects.startLifetime;

		visual.localScale = Vector3.one * radius * 10;
	}

	// Update is called once per frame
	void Update () {
		CoolDown ();
	}

	void FixedUpdate()
	{
//		if (numBodies > 0)
//		{
//			ApplyForce();
//		}
	}

	void OnTriggerEnter2D(Collider2D col)
	{
		bodies.Add (col.attachedRigidbody.GetComponent<Body>());

		numBodies++;
	}

	void OnTriggerExit2D(Collider2D col)
	{
		bodies.Remove (col.attachedRigidbody.GetComponent<Body>());

		numBodies--;
	}

	void CoolDown()
	{
		if(timer != 0)
		{
			timer -= Time.deltaTime;
			
			timer = Mathf.Clamp (timer, 0, coolDown);
		}
	}
	
	public void ApplyForce()
	{
		if(timer == 0)
		{
			//effects.Play ();

			timer = coolDown;

			for (int i = 0; i < numBodies; i++)
			{
				Body r = bodies[i];
				
				Vector2 toBody = r.transform.position - transform.position;
				
				float F = force / toBody.sqrMagnitude; //simplified gravitational force calculation
				//note the label of "force" is misleading, F will in fact be used as acceleration
				
				F = Mathf.Clamp(F, 0, maxForce * Time.fixedDeltaTime);
				
				r.accel += toBody.normalized * F * dir;
			}
		}
	}

	void OnDisable()
	{
		bodies.Clear ();

		numBodies = 0;
	}
}
