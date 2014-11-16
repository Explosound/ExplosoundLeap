using UnityEngine;
using System.Collections;
using Leap;

public class LeapEventListener : Leap.Listener {
	public GameObject graph;
	public GameObject camera;
	public Transform transform;
	
	
	// Use this for initialization
	public void Start () {
		Debug.Log("Leap Listener starts");
		graph = GameObject.Find("Graph");
		camera = GameObject.Find("Main Camera");
		transform = camera.GetComponent<Transform>();
	}
	
	public override void OnConnect(Controller controller){
		Debug.Log("Leap Connected");

		controller.EnableGesture(Gesture.GestureType.TYPE_SWIPE,true);

	}
	
	// Update is called once per frame
	public override void OnFrame(Controller controller)
	{
		Frame frame = controller.Frame();

		// Handle gestures :
		GestureList gestures = frame.Gestures();
		for(int g = 0; g < gestures.Count; g++)
		{
			switch(gestures[g].Type){
				case Gesture.GestureType.TYPE_SWIPE:

					switch (frame.Gestures()[g].State)
					{
					case Gesture.GestureState.STATE_START:
						//Handle starting gestures
					Debug.Log("Swipe detected");
						break;

					case Gesture.GestureState.STATE_UPDATE:
						//Handle continuing gestures
						break;
					case Gesture.GestureState.STATE_STOP:
						//Handle ending gestures
						break;
					default:
						//Handle unrecognized states
						break;
					}

					break;
				default:
					break;
			}


		}
	}
}
