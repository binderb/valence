using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class AddH : MonoBehaviour {

	public Color myColor;
	public Color myHighlight;
	public Color myLabelColor;

	// Use this for initialization
	void Start () {

	}

	// Update is called once per frame
	void Update () {

	}

	void OnMouseOver () {
		if (!Controller.inputLock) {
			GameObject.Find ("AddHLabel").GetComponent<Text>().color = myLabelColor;
			GameObject.Find ("DatabaseLabel").GetComponent<Text>().color = Color.clear;
			GameObject.Find ("ResetBondsLabel").GetComponent<Text>().color = Color.clear;
			GameObject.Find ("InstructionsLabel").GetComponent<Text>().color = Color.clear;
			GameObject.Find ("ExitLabel").GetComponent<Text>().color = Color.clear;
		}
	}

	void OnMouseExit () {
		GameObject.Find ("AddHLabel").GetComponent<Text>().color = Color.clear;
		GameObject.Find ("DatabaseLabel").GetComponent<Text>().color = Color.clear;
		GameObject.Find ("ResetBondsLabel").GetComponent<Text>().color = Color.clear;
		GameObject.Find ("InstructionsLabel").GetComponent<Text>().color = Color.clear;
		GameObject.Find ("ExitLabel").GetComponent<Text>().color = Color.clear;
	}

	void OnMouseDown () {
		if (!Controller.inputLock) {
			StartCoroutine (hitButton());
		}
	}

	IEnumerator hitButton () {

		Color highlightColor = myHighlight;
		GameObject[] atoms = GameObject.FindGameObjectsWithTag("atom");
		int freeHydrogens = 0;
		foreach (GameObject a in atoms) {
			if (a.GetComponent<AtomBehavior>().element == "Hydrogen" && a.transform.parent == null) {
				freeHydrogens++;
			}
		}
		if (freeHydrogens < 4) {
			GameObject.Find ("GameController").GetComponent<Controller>().spawnHydrogen();
		} else {
			highlightColor = Color.red;
		}

		Vector3 popScale = new Vector3(1.2f,1.2f,1.0f);
		transform.localScale = popScale;
		float t = 0;
		while (t < 1.0) {
			t+=3*Time.deltaTime;
			transform.localScale = Vector3.Lerp (popScale, Vector3.one,Mathf.SmoothStep(0.0f,1.0f,t));
			GetComponent<RageSpline>().fillColor1 = Color.Lerp (highlightColor, myColor, Mathf.SmoothStep(0.0f,1.0f,t));
			//GameObject.Find("AddHText").GetComponent<Text>().color = Color.Lerp (myHighlight, myColor, Mathf.SmoothStep(0.0f,1.0f,t));
			GetComponent<RageSpline>().RefreshMesh(true,true,true);
			yield return 0;
		}
	}

}
