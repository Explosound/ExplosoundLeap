using UnityEngine;
using System.Collections;
using System;

using Leap;

public class LeapCameraControler : MonoBehaviour {
	// Moving parameters :
	public float drivingDistance = 30; // TODO : évaluer cette distance dans unity !!
	public float drivingVelocity = 0.06F;
	public float drivingAngle = (float)Math.PI / 6F; // 30°

	public Rect driveRect;

	public Leap.Controller controller;
	public LeapEventListener listener;
	public GameObject graph;

	public bool gestureDetected = false;
	public float startGestureTime;

	public bool driving = false;
	public bool grabbing = false;
	public bool zooming = false;

	public Vector drivingOriginPosition;
	public Vector drivingOriginDirection;


	// Use this for initialization
	public void Start () {
		listener = new LeapEventListener();
		controller = new Controller(listener);
		Debug.Log("Leap start");
	}

	public void Update () {
		Frame frame = controller.Frame();

		HandList hands = frame.Hands;

		if(hands.Count == 1)
		{
			foreach(Hand hand in hands)
			{
				if(hand.GrabStrength == 0 || hand.PinchStrength == 1)
				{
					if (!gestureDetected) {
						gestureDetected = true;
						startGestureTime = Time.time;
					}
					else if (Time.time - startGestureTime > 1) {
						// Driving enabled : handling user control


						// Open Hand : driving
						if(hand.GrabStrength == 0) {
							if (!driving) {
								GUI.Label(new Rect(10,10,60,20), "Driving Enabled");
								drivingOriginPosition = hand.PalmPosition;
								drivingOriginDirection = hand.Direction;
								driving = true;
							}

							// Transform commands :
							float deltaX = drivingOriginPosition.x - hand.PalmPosition.x;
							float deltaY = drivingOriginPosition.y - hand.PalmPosition.y;
							float deltaZ = drivingOriginPosition.z - hand.PalmPosition.z;

							if (Math.Abs(deltaX) > drivingDistance) {
								transform.position-=transform.right * drivingVelocity * (deltaX / drivingDistance);
							}
							if (Math.Abs(deltaY) > drivingDistance) {
								transform.position-=transform.up * drivingVelocity * (deltaY / drivingDistance);
							}
							if (Math.Abs(deltaZ) > drivingDistance) {
								transform.position+=transform.forward * drivingVelocity * (deltaZ / drivingDistance);
							}


							// Rotation commands :
							if (Math.Abs(hand.Direction.Pitch) > drivingAngle) {
								float sign = hand.Direction.Pitch / Math.Abs(hand.Direction.Pitch);
								transform.Rotate(-1 * sign,0,0);
							}
							if (Math.Abs(hand.Direction.Yaw) > drivingAngle) {
								float sign = hand.Direction.Yaw / Math.Abs(hand.Direction.Yaw);
								transform.Rotate(0,1 * sign,0);
							}
						}


						// TODO
					}
				} else {
					startGestureTime = 0;
					gestureDetected = false;
					driving = false;
					grabbing = false;
				}
			}
		} else if (hands.Count == 2) {
			foreach(Hand hand in hands)
			{
				if(hand.GrabStrength == 0 || hand.GrabStrength == 1)
				{
					if (!gestureDetected) {
						gestureDetected = true;
						startGestureTime = Time.time;
					}
					else if (Time.time - startGestureTime > 3) {
						if(hand.GrabStrength == 0) {
							transform.position+=transform.forward*0.1F;
						} else 
						if(hand.GrabStrength == 1) {
							transform.position-=transform.forward*0.1F;
						}
						driving = true;
						
						// Driving enabled : handling user control
						
						// TODO
					}
				} else {
					startGestureTime = 0;
					gestureDetected = false;
					driving = false;
					grabbing = false;
				}
			}
		} else {
			startGestureTime = 0;
			gestureDetected = false;
			driving = false;
			grabbing = false;
		}
		
		// -------- TO REMOVE :
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
