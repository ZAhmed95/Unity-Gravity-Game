using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class TestCursor : NetworkBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if(isLocalPlayer)
		{
			TrackMouse ();
		}
	}

	void TrackMouse()
	{
		Vector2 mousePos = Camera.main.ScreenToWorldPoint (Input.mousePosition);

		transform.position = mousePos;
	}
}
