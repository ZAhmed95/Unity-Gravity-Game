using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour {

	public Transform player;

	public Rigidbody2D rb;

	public bool follow; //if true, camera will follow player

	//for mouse tracking function
	public float range; //range in which camera can follow mouse

	//for mouse zoom function
	public float minZoom;

	public float maxZoom;
	
	public Vector2 lastMousePos;

	//for lazy tracking function
	public float maxSpeed; //maximum speed camera can attain to catch up with player

	public float followSpeed; //current player tracking speed of camera, this is a function of distance from player
	//the further the camera is from player, the higher the follow speed, to ensure camera doesnt lose player

	public float coef; //coefficient for use in calculating follow speed

	// Use this for initialization
	void Start () {
		rb = player.GetComponent<Rigidbody2D> ();
	}
	
	// Update is called once per frame
	void Update()
	{
		//TrackMouse ();

		//StartCoroutine(MouseZoom ());
	}

	void LateUpdate () {

		//LazyTrack ();

		if(follow)
		{
			FollowTarget ();
		}
	}

	void FollowTarget()
	{
		transform.position = player.transform.position + Vector3.back * 10;
	}

	//camera lazy tracking player (i.e. when player moves, camera will gradually move to catch up with player, instead of staying right on top
	//of player the entire time
	void LazyTrack()
	{
		//vector from camera to player
		Vector2 toPlayer = player.position - transform.position;
		//2D distance from camera to player (neglecting z axis)
		float sqrDistance = toPlayer.sqrMagnitude;

		if(sqrDistance != 0)
		{
			//calculate follow speed, which is related to square of distance between camera and player
			followSpeed = coef * sqrDistance;
			//clamp follow speed between min and max values
			followSpeed = Mathf.Clamp (followSpeed, 0, maxSpeed);
			
			Vector2 moveDir = toPlayer.normalized * followSpeed * Time.deltaTime;
			
			transform.position += (Vector3)moveDir;
		}
	}

	//experimental function that makes camera follow mouse movement
	void TrackMouse()
	{
		Vector3 newPos = transform.position + new Vector3 (Input.GetAxis ("Mouse X"), Input.GetAxis ("Mouse Y"), 0);

		Vector2 toPlayer = newPos - (player.position + (Vector3)rb.velocity * Time.fixedDeltaTime);

		float dist = toPlayer.magnitude;

		if (dist > range)
		{
			Vector2 clampedPos = Vector2.ClampMagnitude(toPlayer, range) + (Vector2)player.position;

			newPos.x = clampedPos.x;
			newPos.y = clampedPos.y;
		}

		transform.position = newPos;
	}

	//experimental function that makes camera zoom out as mouse goes further from player
	IEnumerator MouseZoom()
	{
		Vector2 mousePos = Camera.main.ScreenToWorldPoint (Input.mousePosition);

		if(mousePos != lastMousePos && (Input.GetAxis ("Mouse X") != 0 || Input.GetAxis ("Mouse Y") != 0))
		{
			float zoomFraction = (mousePos.magnitude - minZoom) / (maxZoom - minZoom);

			zoomFraction = Mathf.Clamp01(zoomFraction);

			Camera.main.orthographicSize = Mathf.Lerp(minZoom, maxZoom, zoomFraction);
		}

		yield return new WaitForEndOfFrame();
		
		lastMousePos = mousePos;
	}
}
