using UnityEngine;
using System.Collections;
using System.Xml;
using System.IO;
using System.Text;
using System.Collections.Generic;

public class GraphControler : MonoBehaviour {

	Dictionary<string, Node> nodeList = new Dictionary<string, Node>();
	Dictionary<string, Edge> edgeList = new Dictionary<string, Edge>();

	Node currentNode;
	Edge currentEdge;

	public GameObject NodeView;

	private List<GameObject> visibleNodes;

	int edgeIndex = 0;

	// Use this for initialization
	void Start () {
		clearGraph();
		loadGraph("test");
		visibleNodes = new List<GameObject> ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void clearGraph ()
	{
		for(int i =0;i<this.transform.childCount;i++){
			Destroy(this.transform.GetChild(i).gameObject);
		}
	}

	void loadGraph(string name){
		XmlReader xReader = XmlReader.Create("Graph/" + name + ".gexf");
		while (xReader.Read())
		{
			switch (xReader.NodeType)
			{
			case XmlNodeType.Element:
				if(xReader.LocalName == "node")
					currentNode = new Node (xReader.GetAttribute("id"),xReader.GetAttribute("label"));
				else if(xReader.LocalName == "size"){
					currentNode.setSize( float.Parse(xReader.GetAttribute("value")));
				}
				else if(xReader.LocalName == "position")
					currentNode.setPosition(
						float.Parse(xReader.GetAttribute("x")),
						float.Parse(xReader.GetAttribute("y")),
						float.Parse(xReader.GetAttribute("z"))
						);
				else if(xReader.LocalName == "color")
					currentNode.setColor(
						int.Parse(xReader.GetAttribute("r")),
						int.Parse(xReader.GetAttribute("g")),
						int.Parse(xReader.GetAttribute("b"))
						);
				else if(xReader.LocalName == "edge"){
					string id = (edgeIndex++).ToString();
					currentEdge = new Edge(id,
					                       this.nodeList[xReader.GetAttribute("source")],
					                       this.nodeList[xReader.GetAttribute("target")]);
				}
				break;
			case XmlNodeType.EndElement:
				if(xReader.LocalName == "node"){
					nodeList.Add(currentNode.getId(),currentNode);
					GameObject node = (GameObject)Instantiate(NodeView,currentNode.getPosition(),new Quaternion());

					node.name = currentNode.getLabel();
					WWW w = new WWW("file://" + node.name);
					AudioClip myClip = w.audioClip;
					node.audio.clip = myClip;

					node.transform.localScale = currentNode.getSize();
					node.renderer.material.color = currentNode.getColor();
					node.SetActive(true);
					node.transform.parent = this.transform;
				}
				else if(xReader.LocalName == "edge"){
					edgeList.Add(currentEdge.getId(),currentEdge);
					GameObject edge = currentEdge.getShape();
					edge.SetActive(true);
					edge.transform.parent = this.transform;
				}
				break;
			}
		}
	}

	
	public void registerVisibleNode(GameObject node) {
		visibleNodes.Add (node);
	}
	
	public void removeVisibleNode(GameObject node) {
		visibleNodes.Remove (node);
	}

	public bool isVisibleNode(GameObject node) {
		return visibleNodes.Contains (node);
	}

	public List<GameObject> getVisibleNode() {
				return visibleNodes;
		}
}
