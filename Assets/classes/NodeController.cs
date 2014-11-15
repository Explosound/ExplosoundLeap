using UnityEngine;
using System.Collections;

public class NodeController : MonoBehaviour {

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		if (!audio.isPlaying) {
						audio.Play ();
				}
	}
}
