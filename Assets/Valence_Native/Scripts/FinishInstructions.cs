using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class FinishInstructions : MonoBehaviour {

	private Transform myAnchor;
	public Color myColor;
	public Color myHighlight;
	
	// Use this for initialization
	void Start () {
		myAnchor = transform.parent;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	
	void OnMouseDown () {
		//if (transform.) {
		StartCoroutine (closeCanvas());
		//}
	}
	
	IEnumerator hitButton () {
		Vector3 popScale = new Vector3(1.05f,1.05f,1.0f);
		myAnchor.localScale = popScale;
		float t = 0;
		while (t < 1.0) {
			t+=3*Time.deltaTime;		
			myAnchor.localScale = Vector3.Lerp (popScale, Vector3.one,Mathf.SmoothStep(0.0f,1.0f,t));
			GetComponent<RageSpline>().outlineColor1 = Color.Lerp (myHighlight, myColor, Mathf.SmoothStep(0.0f,1.0f,t));
			GameObject.Find("FinishText").GetComponent<Text>().color = Color.Lerp (myHighlight, myColor, Mathf.SmoothStep(0.0f,1.0f,t));
			GetComponent<RageSpline>().RefreshMesh(true,true,true);
			yield return 0;
		}
	}
	
	IEnumerator closeCanvas () {
		Vector3 popScale = new Vector3(1.05f,1.05f,1.0f);
		myAnchor.localScale = popScale;
		float t = 0;
		while (t < 1.0) {
			t+=3*Time.deltaTime;		
			myAnchor.localScale = Vector3.Lerp (popScale, Vector3.one,Mathf.SmoothStep(0.0f,1.0f,t));
			GetComponent<RageSpline>().outlineColor1 = Color.Lerp (myHighlight, myColor, Mathf.SmoothStep(0.0f,1.0f,t));
			GameObject.Find("FinishText").GetComponent<Text>().color = Color.Lerp (myHighlight, myColor, Mathf.SmoothStep(0.0f,1.0f,t));
			GetComponent<RageSpline>().RefreshMesh(true,true,true);
			yield return 0;
		}
		StartCoroutine(shrinkToZero(GameObject.Find("Panel").transform));
		StartCoroutine(shrinkToZero(GameObject.Find("PreviousAnchor").transform));
		StartCoroutine(shrinkToZero(GameObject.Find("NextAnchor").transform));
		StartCoroutine(shrinkToZero(GameObject.Find("FinishAnchor").transform));
		t = 0;
		while (t < 1.0) {
			t+=3*Time.deltaTime;		
			GameObject.Find("InstructionCanvas").GetComponent<CanvasGroup>().alpha = Mathf.Lerp (1, 0, t);
			GameObject.Find("InstructionGraphic").GetComponent<SpriteRenderer>().color = Color.Lerp (new Color(1,1,1,1), new Color(1,1,1,0), t);

			yield return 0;
		}

		if (GameObject.Find ("GameController").GetComponent<Controller>().previous_scene == "Title") {
			GameObject.Find("GameController").GetComponent<Controller>().showTitle();
		} else if (GameObject.Find ("GameController").GetComponent<Controller>().previous_scene == "Game") {
			GameObject.Find("GameController").GetComponent<Controller>().showGame();
		}


		/*float t = 0;
		while (t < 1.0) {
			t+=3*Time.deltaTime;		
			GameObject.Find("FadeBackdrop").GetComponent<SpriteRenderer>().color = Color.Lerp (new Color(1,1,1,0), new Color(1,1,1,1), t);
			GameObject.Find("TitleCanvas").GetComponent<CanvasGroup>().alpha = Mathf.Lerp (1, 0, t);
			GameObject.Find("TitleUI").transform.localPosition = Vector3.Lerp (Vector3.zero,Vector3.left*20,Mathf.SmoothStep(0.0f,1.0f,t));
			yield return 0;
		}
		GameObject.Find("TitleUI").transform.localPosition = Vector3.zero;
		GameObject.Find("GameController").GetComponent<Controller>().showInstructions();*/
	}

	IEnumerator shrinkToZero(Transform trans) {
		Vector3 zeroScale = new Vector3(1.0f,0.0f,1.0f);
		Vector3 initScale = trans.localScale;
		float t = 0;
		while (t < 1.0) {
			t+=6*Time.deltaTime;		
			trans.localScale = Vector3.Lerp (initScale, zeroScale,Mathf.SmoothStep(0.0f,1.0f,t));
			yield return 0;
		}
	}
}
