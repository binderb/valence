using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CodexLight : MonoBehaviour {

	private Controller controller;
	private Text hintText;
	//private Transform databaseMark;
	//private Transform missingMark;
	//private SpriteRenderer structureMark;

	// Use this for initialization
	void Start () {
		controller = GameObject.Find ("GameController").GetComponent<Controller>();
		hintText = GameObject.Find ("CodexClue").GetComponent<Text>();
		//databaseMark = GameObject.Find ("CodexMarkDatabase").transform;
		//missingMark = GameObject.Find ("CodexMarkMissing").transform;
		//structureMark = GameObject.Find ("CodexMarkStructure").GetComponent<SpriteRenderer>();
	}

	void OnMouseEnter () {
		RageSpline parentSpline = transform.parent.GetComponent<RageSpline>();
		if (!controller.foundCodexEntries.Contains (int.Parse(transform.parent.name.Split('t')[1])-1)) {
			//foreach (MeshRenderer d in databaseMark.GetComponentsInChildren<MeshRenderer>()) d.enabled = false;
			//foreach (MeshRenderer m in missingMark.GetComponentsInChildren<MeshRenderer>()) m.enabled = true;
			//structureMark.sprite = null;
			parentSpline.fillColor1 = controller.lightOverColor;
		} else {
			//structureMark.sprite = Resources.Load<Sprite>(controller.GetComponent<AtomDictionary>().lightsDictionary[int.Parse(transform.parent.name.Split('t')[1])-1] + "_structure");
			//print (controller.GetComponent<AtomDictionary>().lightsDictionary[int.Parse(transform.parent.name.Split('t')[1])-1] + "_structure");
			//GameObject structureSprite = (GameObject)Instantiate(Resources.Load<GameObject>(controller.GetComponent<AtomDictionary>().lightsDictionary[int.Parse(transform.parent.name.Split('t')[1])-1] + "_structure"));
			//structureSprite.parent = GameObject.Find("CodexRing").transform;
			//structureSprite.transform.position = new Vector3(2715,360,-100);
			//foreach (MeshRenderer d in databaseMark.GetComponentsInChildren<MeshRenderer>()) d.enabled = false;
			//foreach (MeshRenderer m in missingMark.GetComponentsInChildren<MeshRenderer>()) m.enabled = false;
			parentSpline.fillColor1 = controller.lightLitOverColor;
		}
		parentSpline.RefreshMesh(true,true,true);
	}

	void OnMouseExit () {
		RageSpline parentSpline = transform.parent.GetComponent<RageSpline>();
		if (!controller.foundCodexEntries.Contains (int.Parse(transform.parent.name.Split('t')[1])-1)) {
			parentSpline.fillColor1 = controller.lightColor;
			//foreach (MeshRenderer d in databaseMark.GetComponentsInChildren<MeshRenderer>()) d.enabled = true;
			//foreach (MeshRenderer m in missingMark.GetComponentsInChildren<MeshRenderer>()) m.enabled = false;
			//structureMark.sprite = null;
			//foreach (GameObject s in GameObject.FindGameObjectsWithTag("database_structure")) {if (s.transform.position.x == 2715) GameObject.Destroy(s);}
		} else {
			//foreach (MeshRenderer d in databaseMark.GetComponentsInChildren<MeshRenderer>()) d.enabled = true;
			//foreach (MeshRenderer m in missingMark.GetComponentsInChildren<MeshRenderer>()) m.enabled = false;
			//structureMark.sprite = null;
			parentSpline.fillColor1 = controller.lightLitColor;
			//foreach (GameObject s in GameObject.FindGameObjectsWithTag("database_structure")) {if (s.transform.position.x == 2715) GameObject.Destroy(s);}
		}
		parentSpline.RefreshMesh(true,true,true);
	}

	void OnMouseDown () {
		RageSpline parentSpline = transform.parent.GetComponent<RageSpline>();
		if (parentSpline.fillColor1 == controller.lightLitOverColor) {
				//controller.showCodex(int.Parse(transform.parent.name.Split('t')[1])-1));

		} else if (parentSpline.fillColor1 == controller.lightOverColor) {
			hintText.text = controller.GetComponent<AtomDictionary>().clueDictionary[int.Parse(transform.parent.name.Split('t')[1])-1];

		}

	}

	// Update is called once per frame
	void Update () {

	}
}
