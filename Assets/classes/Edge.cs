using UnityEngine;
using System.Collections;
using System.Xml;
using System.IO;
using System.Text;

class Edge
{
	private string id;
	private Node source;
	private Node target;

	public Edge(string id, Node src,Node tgt){
		this.id = id;
		source = src;
		target = tgt;
	}
	public string getId(){
		return id;
	}

	public override string ToString (){
		return string.Format ("[Edge] : {0} => {1}",source.getLabel(),target.getLabel());
	}

	public GameObject getShape(){
		GameObject s = new GameObject();
		s.AddComponent<LineRenderer>();
		LineRenderer lr = s.GetComponent<LineRenderer>();
		lr.SetWidth((float)0.01,(float)0.01);
		lr.SetPosition(0,source.getPosition());
		lr.SetPosition(1,target.getPosition());
		lr.material.SetColor(0,new Color(1,1,1));
		lr.material.shader = Shader.Find("Sprites/Default");
		lr.name = "edge_"+id;
		return s;
	}
}

