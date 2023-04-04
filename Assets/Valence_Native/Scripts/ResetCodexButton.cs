using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ResetCodexButton : MonoBehaviour {

	// Use this for initialization
	void Start () {

	}

	// Update is called once per frame
	void Update () {

	}

	void OnMouseDown () {
		Text myText = GameObject.Find ("ResetCodex").GetComponent<Text>();
		if (myText.text == "Reset Codex") {
			StartCoroutine (showConfirm());
		}
	}


	IEnumerator showConfirm () {

		GameObject.Find ("OptionsConfirm").GetComponent<Text>().text = "Clear all found Codex entries?";
		GameObject.Find ("EndGame").GetComponent<Text>().text = "No";
		GameObject.Find ("CloseOptions").GetComponent<Text>().text = "Yes";
		GameObject.Find ("ResetCodex").GetComponent<Text>().text = "";
		GameObject.Find ("ResetCodexButton").GetComponent<BoxCollider2D>().enabled = false;
		GameObject.Find ("ResetCodexButton").GetComponent<MeshRenderer>().enabled = false;
		foreach (MeshRenderer m in GameObject.Find ("EmailButton").GetComponentsInChildren<MeshRenderer>()) m.enabled = false;
		GameObject.Find ("EmailButton").GetComponent<BoxCollider2D>().enabled = false;
		foreach (MeshRenderer m in GameObject.Find ("OptionsCodexRing").GetComponentsInChildren<MeshRenderer>()) m.enabled = false;
		GameObject.Find ("OptionsTitle").GetComponent<Text>().color = Color.clear;
		GameObject.Find ("OptionsLevel").GetComponent<Text>().color = Color.clear;
		GameObject.Find ("OptionsScore").GetComponent<Text>().color = Color.clear;
		GameObject.Find ("OptionsCodex").GetComponent<Text>().color = Color.clear;
		GameObject.Find ("OptionsConfirm").GetComponent<Text>().color = Color.white;


		yield return 0;
	}

	IEnumerator resetBoxForEndgame () {

		// Reset visual elements
		GameObject.Find ("CloseOptions").GetComponent<Text>().text = "Resume";
		GameObject.Find ("EndGame").GetComponent<Text>().text = "End Game";
		GameObject.Find ("ResetCodex").GetComponent<Text>().text = "Reset Codex";
		GameObject.Find ("ResetCodexButton").GetComponent<BoxCollider2D>().enabled = true;
		GameObject.Find ("ResetCodexButton").GetComponent<MeshRenderer>().enabled = true;
		foreach (MeshRenderer m in GameObject.Find ("EmailButton").GetComponentsInChildren<MeshRenderer>()) m.enabled = true;
		GameObject.Find ("EmailButton").GetComponent<BoxCollider2D>().enabled = true;
		foreach (MeshRenderer m in GameObject.Find ("OptionsCodexRing").GetComponentsInChildren<MeshRenderer>()) m.enabled = true;
		GameObject.Find ("OptionsTitle").GetComponent<Text>().color = Color.white;
		GameObject.Find ("OptionsLevel").GetComponent<Text>().color = Color.white;
		GameObject.Find ("OptionsScore").GetComponent<Text>().color = Color.white;
		GameObject.Find ("OptionsCodex").GetComponent<Text>().color = Color.white;
		GameObject.Find ("OptionsConfirm").GetComponent<Text>().color = Color.clear;

		// Move the box, and unlock the game screen
		GameObject.Find ("OptionsBox").transform.position = new Vector3(-480,1000,-200);

		// Call reboot
		GameObject.Find ("GameController").GetComponent<Controller>().reboot();

		yield return 0;
	}


}
