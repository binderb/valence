using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class EndGameButton : MonoBehaviour {

	// Use this for initialization
	void Start () {

	}

	// Update is called once per frame
	void Update () {

	}

	void OnMouseDown () {
		Text myText = GameObject.Find ("EndGame").GetComponent<Text>();
		if (myText.text == "End Game") {
			myText.text = "No";
			StartCoroutine (revealConfirmEnd());
		} else if (myText.text == "No") {
			myText.text = "End Game";
			StartCoroutine (revealOptionsBox());
		}
	}

	IEnumerator revealConfirmEnd () {
		GameObject.Find ("OptionsConfirm").GetComponent<Text>().text = "Are you sure?";
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

	IEnumerator revealOptionsBox () {
		GameObject.Find ("CloseOptions").GetComponent<Text>().text = "Resume";
		//GameObject.Find ("ResetCodex").GetComponent<Text>().text = "Reset Codex";
		//GameObject.Find ("ResetCodexButton").GetComponent<BoxCollider2D>().enabled = true;
		//GameObject.Find ("ResetCodexButton").GetComponent<MeshRenderer>().enabled = true;
		foreach (MeshRenderer m in GameObject.Find ("EmailButton").GetComponentsInChildren<MeshRenderer>()) m.enabled = true;
		GameObject.Find ("EmailButton").GetComponent<BoxCollider2D>().enabled = true;
		foreach (MeshRenderer m in GameObject.Find ("OptionsCodexRing").GetComponentsInChildren<MeshRenderer>()) m.enabled = true;
		GameObject.Find ("OptionsTitle").GetComponent<Text>().color = Color.white;
		GameObject.Find ("OptionsLevel").GetComponent<Text>().color = Color.white;
		GameObject.Find ("OptionsScore").GetComponent<Text>().color = Color.white;
		GameObject.Find ("OptionsCodex").GetComponent<Text>().color = Color.white;
		GameObject.Find ("OptionsConfirm").GetComponent<Text>().color = Color.clear;

		yield return 0;
	}

}
