using UnityEngine;
using System.Collections;

public class Bubble : MonoBehaviour {

	public Collider2D col; //own collider

	public float plasma; //how much plasma is in bubble

	public float lastPlasma; //stores previous value of plasma

	public float maxPull; //pull at zero bubble size

	public float coef; //dropoff coefficient for pull equation

	public float pull; //strength of bubble to absorb other bubbles

	public Color[] colors; //progression colors, plasma starts at colors[0] and progresses up as more plasma is added

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		UpdateColor ();
	}

	void FixedUpdate()
	{
		UpdateSize ();

		StartCoroutine (CalculatePull ());
	}

	void OnTriggerEnter2D(Collider2D col)
	{
		Bubble b = col.GetComponent<Bubble> ();

		if (b != null)
		{
			StartCoroutine(Pull(b));
		}
	}

	//Called to pull material from given bubble
	IEnumerator Pull(Bubble b)
	{
		Collider2D c = b.GetComponent<Collider2D> ();

		while(true)
		{
			if(!c.IsTouching(col))
			{
				break;
			}

			float pullAmount = Mathf.Clamp (b.plasma, 0, pull * Time.deltaTime);
			
			b.plasma -= pullAmount;
			
			plasma += pullAmount;

			yield return null;
		}
	}

	IEnumerator CalculatePull()
	{
		yield return new WaitForFixedUpdate ();
		//pull is inversely proportional to amount, in a function of the form y = k/x (actually (x+1) to get rid of the asymptote)
		pull = maxPull / (coef * plasma + 1);
	}

	void UpdateSize()
	{
		//calculate radius as cube root of amount, since amount represents volume
		float radius = Mathf.Pow (plasma, 1 / 3f);

		transform.localScale = Vector3.one * radius;
	}

	void UpdateColor()
	{
		if (plasma == lastPlasma) 
		{
			return;
		}
		else
		{
			lastPlasma = plasma;

			int index = Mathf.FloorToInt (plasma / 100);
			
			float lerp = (plasma - index * 100) / 100;
			
			Color c = Color.Lerp (colors [index], colors [index + 1], lerp);
			
			GetComponent<SpriteRenderer> ().color = c;
		}
	}
}
