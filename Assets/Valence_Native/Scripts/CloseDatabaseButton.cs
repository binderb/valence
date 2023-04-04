using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CloseDatabaseButton : MonoBehaviour {

	public Color myColor;
	public Color myHighlight;
	
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	
	void OnMouseDown () {
		StartCoroutine (hitButton());
		StartCoroutine (fadeCanvas());
	}
	
	IEnumerator hitButton () {
		Vector3 popScale = new Vector3(1.05f,1.05f,1.0f);
		transform.localScale = popScale;
		float t = 0;
		while (t < 1.0) {
			t+=3*Time.deltaTime;		
			transform.localScale = Vector3.Lerp (popScale, Vector3.one,Mathf.SmoothStep(0.0f,1.0f,t));
			GetComponent<RageSpline>().outlineColor1 = Color.Lerp (myHighlight, myColor, Mathf.SmoothStep(0.0f,1.0f,t));
			GameObject.Find("CloseDatabaseLabel").GetComponent<Text>().color = Color.Lerp (myHighlight, myColor, Mathf.SmoothStep(0.0f,1.0f,t));
			GetComponent<RageSpline>().RefreshMesh(true,true,true);
			yield return 0;
		}
	}
	
	IEnumerator fadeCanvas () {
		yield return new WaitForSeconds(0.1f);
		GameObject.Find ("Secondary Camera").GetComponent<Camera>().enabled = false;
		CanvasGroup codexCanvas = GameObject.Find ("CodexCanvas").GetComponent<CanvasGroup>();
		float t = 0;
		while (t < 1.0) {
			t+=3*Time.deltaTime;		
			GameObject.Find("DatabaseCanvas").GetComponent<CanvasGroup>().alpha = Mathf.Lerp (1, 0, t);
			GameObject.Find("DatabaseCover").GetComponent<SpriteRenderer>().color = Color.Lerp (new Color(1f,1f,1f,0f),Color.white,t);
			if (codexCanvas.alpha != 0) {
				codexCanvas.alpha = Mathf.Lerp (1,0,t);
			}
			yield return 0;
		}
		GameObject.Find ("GameController").GetComponent<Controller>().resetDatabase();
		GameObject.Find ("GameController").GetComponent<Controller>().showGame();
	}

}