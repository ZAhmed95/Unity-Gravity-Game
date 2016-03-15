using UnityEngine;
using System.Collections;

public class Mineral : MonoBehaviour {

	//this script holds information for mineral content of an asteroid, which is extracted when near player planets

	public float mineral; //how much mineral content in this asteroid

	public Color full; //color of mineral rich asteroid

	public Color depleted; //color of asteroid with no minerals left in it

	public bool isDepleted; //true if all minerals have been extracted

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public IEnumerator Deplete()
	{
		isDepleted = true;

		int timeFrame = 20;

		SpriteRenderer s = GetComponent<SpriteRenderer> ();

		for (int t = 1; t <= timeFrame; t++)
		{
			s.color = Color.Lerp(full, depleted, (float)t / timeFrame);

			yield return null;
		}
	}
}
