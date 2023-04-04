using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class InstructionsButton : MonoBehaviour {

	public Color myColor;
	public Color myHighlight;
	public Color myLabelColor;
	
	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		
	}
	
	void OnMouseDown () {
		if (!Controller.inputLock) {
			StartCoroutine (hitButton());
			StartCoroutine (fadeCanvas());
		}
	}

	void OnMouseOver () {
		if (!Controller.inputLock) {
			GameObject.Find ("AddHLabel").GetComponent<Text>().color = Color.clear;
			GameObject.Find ("DatabaseLabel").GetComponent<Text>().color = Color.clear;
			GameObject.Find ("ResetBondsLabel").GetComponent<Text>().color = Color.clear;
			GameObject.Find ("InstructionsLabel").GetComponent<Text>().color = myLabelColor;
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
	
	IEnumerator hitButton () {
		Vector3 popScale = new Vector3(1.2f,1.2f,1.0f);
		transform.localScale = popScale;
		float t = 0;
		while (t < 1.0) {
			t+=3*Time.deltaTime;		
			transform.localScale = Vector3.Lerp (popScale, Vector3.one,Mathf.SmoothStep(0.0f,1.0f,t));
			GetComponent<RageSpline>().fillColor1 = Color.Lerp (myHighlight, myColor, Mathf.SmoothStep(0.0f,1.0f,t));
			//GameObject.Find("InstructionsText").GetComponent<Text>().color = Color.Lerp (myHighlight, myColor, Mathf.SmoothStep(0.0f,1.0f,t));
			GetComponent<RageSpline>().RefreshMesh(true,true,true);
			yield return 0;
		}
	}
	
	IEnumerator fadeCanvas () {
		yield return new WaitForSeconds(0.1f);
		GameObject.Find ("GameController").GetComponent<Controller>().hideGameHUD();

		float t = 0;
		while (t < 1.0) {
			t+=3*Time.deltaTime;		
			//GameObject.Find("FadeBackdrop").GetComponent<SpriteRenderer>().color = Color.Lerp (new Color(1,1,1,0), new Color(1,1,1,1), t);
			GameObject.Find("GameCanvas").GetComponent<CanvasGroup>().alpha = Mathf.Lerp (1, 0, t);
			//GameObject.Find("TitleUI").transform.localPosition = Vector3.Lerp (Vector3.zero,Vector3.left*20,Mathf.SmoothStep(0.0f,1.0f,t));
			yield return 0;
		}
		//GameObject.Find("TitleUI").transform.localPosition = Vector3.zero;
		GameObject.Find("GameController").GetComponent<Controller>().previous_scene = "Game";
		GameObject.Find("GameController").GetComponent<Controller>().showInstructions();
	}
	
}
