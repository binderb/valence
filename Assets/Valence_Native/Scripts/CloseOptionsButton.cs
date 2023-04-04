using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class CloseOptionsButton : MonoBehaviour {

	public int direction;

	// Use this for initialization
	void Start () {

	}

	// Update is called once per frame
	void Update () {

	}

	void OnMouseDown () {

		Text myText = GameObject.Find ("CloseOptions").GetComponent<Text>();
		Text confirmText = GameObject.Find ("OptionsConfirm").GetComponent<Text>();
		if (myText.text == "Resume") {
			StartCoroutine (closeOptions());
		} else if (myText.text == "Yes") {
			if (confirmText.text == "Are you sure?") StartCoroutine (resetBoxForEndgame());
			else if (confirmText.text == "Clear all found Codex entries?") {
				// Clear codex entries, and reveal confirmation
				GameObject.Find("GameController").GetComponent<Controller>().foundCodexEntries = new List<int>();
				StartCoroutine(revealOk());
			}
		} else if (myText.text == "OK") {
			StartCoroutine(revealOptionsBox());
		}
	}


	IEnumerator closeOptions () {
		Controller.inputLock = false;
		GameObject.Find ("OptionsBox").transform.position = new Vector3(-480,1000,-200);
		GameObject.Find ("GameCover").GetComponent<SpriteRenderer>().color = Color.clear;
		GameObject.Find ("GameCanvas").GetComponent<CanvasGroup>().alpha = 1.0f;
		yield return 0;
	}

	IEnumerator resetBoxForEndgame () {

		// Reset visual elements
		GameObject.Find ("CloseOptions").GetComponent<Text>().text = "Resume";
		GameObject.Find ("EndGame").GetComponent<Text>().text = "End Game";
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

		// Move the box, and unlock the game screen
		GameObject.Find ("OptionsBox").transform.position = new Vector3(-480,1000,-200);
		//GameObject.Find ("GameCover").GetComponent<SpriteRenderer>().color = Color.clear;
		//GameObject.Find ("GameCanvas").GetComponent<CanvasGroup>().alpha = 1.0f;

		// Call reboot
		GameObject.Find ("GameController").GetComponent<Controller>().reboot();

		yield return 0;
	}

	IEnumerator revealOk () {

		// Set visual elements
		GameObject.Find ("OptionsConfirm").GetComponent<Text>().text = "Codex entries cleared!";
		GameObject.Find ("CloseOptions").GetComponent<Text>().text = "OK";
		GameObject.Find ("EndGame").GetComponent<Text>().text = "";
		GameObject.Find ("ResetCodex").GetComponent<Text>().text = "";
		GameObject.Find ("ResetCodexButton").GetComponent<BoxCollider2D>().enabled = false;
		GameObject.Find ("ResetCodexButton").GetComponent<MeshRenderer>().enabled = false;
		GameObject.Find ("EndGameButton").GetComponent<BoxCollider2D>().enabled = false;
		GameObject.Find ("EndGameButton").GetComponent<MeshRenderer>().enabled = false;

		yield return 0;
	}

	IEnumerator revealOptionsBox () {
		GameObject.Find ("CloseOptions").GetComponent<Text>().text = "Resume";
		//GameObject.Find ("ResetCodex").GetComponent<Text>().text = "Reset Codex";
		GameObject.Find ("EndGame").GetComponent<Text>().text = "End Game";
		//GameObject.Find ("ResetCodexButton").GetComponent<BoxCollider2D>().enabled = true;
		//GameObject.Find ("ResetCodexButton").GetComponent<MeshRenderer>().enabled = true;
		GameObject.Find ("EndGameButton").GetComponent<BoxCollider2D>().enabled = true;
		GameObject.Find ("EndGameButton").GetComponent<MeshRenderer>().enabled = true;

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
