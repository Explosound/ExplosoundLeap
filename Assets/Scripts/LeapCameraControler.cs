using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

using Leap;

public class LeapCameraControler : MonoBehaviour {
	// Moving parameters :
	public float drivingDistance = 30; // TODO : évaluer cette distance dans unity !!
	public float drivingVelocity = 0.06F;
	public float drivingAngle = (float)Math.PI / 6F; // 30°

	public Rect driveRect;
	public GameObject NodeView;
	public GameObject anchor;

	public Leap.Controller controller;
	public LeapEventListener listener;
	public GameObject graph;
	public GraphControler graphController;

	public bool gestureDetected = false;
	public float startGestureTime;

	public bool driving = false;
	public bool grabbing = false;
	public bool zooming = false;
	public bool askForValidation = false;

	public Vector drivingOriginPosition;
	public Vector drivingOriginDirection;
	
	private Plane[] planes;

	// Use this for initialization
	public void Start () {
		graph = GameObject.Find ("Graph");
		graphController = graph.GetComponent<GraphControler> ();

		listener = new LeapEventListener();
		controller = new Controller(listener);
		Debug.Log("Leap start");

	}

	void startDrivingMode (Hand hand) {
		if (!driving) {
			driving = true;
			// GameObject anchor = (GameObject)Instantiate(NodeView,hand.PalmPosition.ToUnityScaled(),new Quaternion()); TODO

		}
	}
	void stopDrivingMode() {
		if (driving) {
			driving = false;	
		}
		Destroy (anchor);
	}


	public void Update () {
		// Play nearest node :
		GameObject playingNode = null;
		float minDistance = 0F;
		float currentDistance = 0F;

		foreach (GameObject node in graphController.getVisibleNode()) {
			currentDistance = Vector3.Distance(camera.transform.position, node.transform.position);

			// First iteration
			if (minDistance == 0F) { minDistance = currentDistance; }

			else {
				if (currentDistance < minDistance) {
					playingNode = node;
					minDistance = currentDistance;
				}
			}
		}
		if (playingNode) {
			playingNode.GetComponent<NodeController> ().PlayMusic (askForValidation);
		}


		

		Frame frame = controller.Frame();

		HandList hands = frame.Hands;

		if(hands.Count == 1)
		{
			foreach(Hand hand in hands)
			{
				if(hand.GrabStrength == 0 || hand.GrabStrength == 1)
				{
					if (!gestureDetected) {
						gestureDetected = true;
						startGestureTime = Time.time;
					}
					else if (Time.time - startGestureTime > 1) {
						// Driving enabled : handling user control

						if(hand.GrabStrength == 0 && !askForValidation) {
							// Open Hand : driving
							if (!driving) {
								drivingOriginPosition = hand.PalmPosition;
								drivingOriginDirection = hand.Direction;
								startDrivingMode(hand);
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
						} else if(hand.GrabStrength == 1 && !grabbing) {
							grabbing = true;
							if (!askForValidation) {
								askForValidation = true;
							} else {
								askForValidation = false;
							}
						}


						// TODO
					}
				} else {
					startGestureTime = 0;
					gestureDetected = false;
					stopDrivingMode();
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
						startDrivingMode(hand);
						
						// Driving enabled : handling user control
						
						// TODO
					}
				} else {
					startGestureTime = 0;
					gestureDetected = false;
					stopDrivingMode();
					grabbing = false;
				}
			}
		} else {
			startGestureTime = 0;
			gestureDetected = false;
			stopDrivingMode();
			grabbing = false;
		}
	}
	
	public void OnGUI () {
		if (driving) { GUI.Label(new Rect(10,10,60,20), "Driving Enabled");	}
		if (askForValidation) { GUI.Label(new Rect(10,10,180,20), "Confirm Saving this sound ?");	}
	}
}