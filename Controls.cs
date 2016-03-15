using UnityEngine;
using System.Collections;

public class Controls : MonoBehaviour {

	public Rigidbody2D rb;

	public Bubble b; //plasma bubble of this planet

	public float launchSpeed; //projectile launch speed, this divided by mass also recoils player

	public float closeRange; //clicking within this range stops planet instead of shooting

	//for projectile handling
	public GameObject projectilePF; //prefab of projectile to be shot

	public int poolAmount; //how many objects in shotPool

	public GameObject[] shotPool; //object pool of projectiles to be shot

	public int index; //current index of shotPool

	//for mouse position
	public Vector2 cursor; //world mouse position

	//for gravity control
	public GameObject gravityWell; //gravity well object

	public float maxForce; //maximum gravity force applicable

	public float force; //how much force gravity well applies

	public float radius; //how much area well affects

	public float G; //gravitational constant G (not the actual constant)

	public bool active; //whether a gravity well is currently being used

	// Use this for initialization
	void Start () {
		shotPool = new GameObject[poolAmount];

		for (int i = 0; i < poolAmount; i++)
		{
			GameObject shot = (GameObject)Instantiate (projectilePF, transform.position, Quaternion.identity);

			shotPool[i] = shot;

			shot.SetActive(false);
		}
	}
	
	// Update is called once per frame
	void Update () {
		MousePos ();

		gravityWell.transform.position = cursor;

		if (Input.GetMouseButton(0) && !active)
		{
			Gravity2(0);

			//StartCoroutine(Gravity(0));
		}

		if (Input.GetMouseButton(1) && !active)
		{
			Gravity2(1);

			//StartCoroutine(Gravity(1));
		}
	}

	void MousePos()
	{
		cursor = Camera.main.ScreenToWorldPoint (Input.mousePosition);
	}

	IEnumerator Gravity(int i)
	{
		active = true;

		gravityWell.SetActive (true);

		Gravity g = gravityWell.GetComponent<Gravity> ();

		g.dir = i == 0 ? -1 : 1;

		while (Input.GetMouseButton(i))
		{
			gravityWell.transform.position = cursor;

			yield return null;
		}

		gravityWell.SetActive (false);

		active = false;
	}

	//almost same as above function, except this runs once on a mouse click while the above function runs as long as mouse button is pressed
	void Gravity2(int i)
	{	
		Gravity g = gravityWell.GetComponent<Gravity> ();
		
		g.dir = i == 0 ? -1 : 1;

		gravityWell.transform.position = cursor;

		g.ApplyForce ();
	}

	//launches projectile as well as giving recoil for movement
	void Shoot()
	{
		Vector2 toCursor = cursor - (Vector2)transform.position;

		//if clicked outside of close range, shoot projectile and move planet
		if(toCursor.magnitude > closeRange)
		{
			GameObject shot = shotPool [index];
			
			shot.SetActive (true);
			
			shot.transform.position = transform.position;
			
			Rigidbody2D r = shot.GetComponent<Rigidbody2D> ();
			
			r.velocity = Vector2.zero;
			
			//get vector to mouse click position
			Vector2 launchDir = (toCursor).normalized;
			
			//launch projectile towards click position
			r.velocity += rb.velocity + launchDir * launchSpeed;
			
			float shotSpeed = Mathf.Clamp (r.velocity.magnitude, 0.9f * launchSpeed, 1.1f * launchSpeed);
			
			r.velocity = r.velocity.normalized * shotSpeed;
			
			//launch player in opposite direction
			rb.velocity += -launchDir * launchSpeed / 20;
			
			float ownSpeed = Mathf.Clamp (rb.velocity.magnitude, 0, 0.11f * launchSpeed);
			
			rb.velocity = rb.velocity.normalized * ownSpeed;

			index++;
			
			if (index == shotPool.Length)
			{
				index = 0;
			}
		}
		//if clicked inside close range then stop planet
		else
		{
			rb.velocity = Vector2.zero;
		}
		
		//each action takes 1% of current plasma
		b.plasma *= 0.99f; 
	}
}
