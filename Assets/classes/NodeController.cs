using UnityEngine;
using System.Collections;

public class NodeController : MonoBehaviour {
	GraphControler graphControler;

	// Use this for initialization
	void Start () {
		graphControler = transform.parent.gameObject.GetComponent<GraphControler> ();
	}

	public void PlayMusic(bool askForValidation) {
		if (!audio.isPlaying) {
			audio.Play ();
		}
		if (!particleSystem.isPlaying) {
			particleSystem.Play ();
		}

		if (askForValidation) {
			particleSystem.startColor = new Color (211F / 255, 0F / 255, 0F / 255 ,255F / 255); // Green
		} else {
			particleSystem.startColor = new Color (243 / 255, 255F / 255, 38F / 255, 255F / 255); // Yellow
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (renderer.isVisible && !graphControler.isVisibleNode (transform.gameObject)) {
			graphControler.registerVisibleNode(transform.gameObject);
		}

		if (!renderer.isVisible && graphControler.isVisibleNode(transform.gameObject)) 
		{
			graphControler.removeVisibleNode(transform.gameObject);
		}

		// Switching off particules if not playing audio
		if (!audio.isPlaying) {
			particleSystem.Stop();
		}
	}
}
