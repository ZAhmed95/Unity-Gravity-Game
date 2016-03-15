using UnityEngine;
using System.Collections;

public class Extractor : MonoBehaviour {

	//this script controls the automatic extraction of minerals from asteroids near player's planet

	public float mineral; //current mineral storage

	public float range; //range at which extraction begins

	public float extractionTime; //time it takes to complete extraction, planet will have to stay in range of mineral for this amount of time

	public CircleCollider2D ownCol;
	// Use this for initialization
	void Start () {
		ownCol = GetComponent<CircleCollider2D> ();

		transform.localScale = Vector3.one * 2 * range;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter2D(Collider2D col)
	{
		if(col.GetComponent<Mineral>() != null)
		{
			StartCoroutine (Extract (col));
		}
	}
	
	IEnumerator Extract(Collider2D col)
	{
		yield return new WaitForSeconds (extractionTime);

		if(col.IsTouching(ownCol))
		{
			Mineral m = col.GetComponent<Mineral>();

			if(!m.isDepleted)
			{
				//extract all minerals from asteroid
				mineral += m.mineral;
				
				m.mineral = 0;
				
				StartCoroutine(m.Deplete());
			}
		}
	}
}
