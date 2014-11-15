using UnityEngine;
using System.Collections;

public class keyboardCameraControler : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKey("up"))
			transform.position+=transform.forward;
		if(Input.GetKey("down"))
			transform.position-=transform.forward;
		if(Input.GetKey("left"))
			transform.Rotate(0,-1,0);
		if(Input.GetKey("right"))
			transform.Rotate(0,1,0);
		if(Input.GetKey("e"))
			transform.Rotate(-1,0,0);
		if(Input.GetKey("d"))
			transform.Rotate(1,0,0);
		if(Input.GetKey("z"))
			camera.fieldOfView += 0.5f;
			camera.fieldOfView = Mathf.Clamp(camera.fieldOfView, 0.1f, 179.9f);

		if(Input.GetKey("s"))
			camera.fieldOfView -= 0.5f;
			camera.fieldOfView = Mathf.Clamp(camera.fieldOfView, 0.1f, 179.9f);
	}
}
