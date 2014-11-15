using UnityEngine;
using System.Collections;

public class LabelDisplayer : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnGUI(){
		if(renderer.isVisible){
			Vector3 dist = Camera.main.transform.position - transform.position;
			if(dist.magnitude < 12){
				GUIStyle style = new GUIStyle();
				style.contentOffset = new Vector2(this.transform.localScale.x,this.transform.localScale.y);
				style.normal.textColor = new Color(1,1,1);

				Vector2 newpos = Camera.main.WorldToScreenPoint(transform.position);
				
				GUI.Label(new Rect(newpos.x, Screen.height - newpos.y,100,60),this.name,style);
			}
		}
	}
}
