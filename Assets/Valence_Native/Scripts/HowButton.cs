﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class HowButton : MonoBehaviour {
	
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
			StartCoroutine (hitButton());
		    StartCoroutine (fadeCanvas());
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
			GameObject.Find("HowText").GetComponent<Text>().color = Color.Lerp (myHighlight, myColor, Mathf.SmoothStep(0.0f,1.0f,t));
			GetComponent<RageSpline>().RefreshMesh(true,true,true);
			yield return 0;
		}
	}

	IEnumerator fadeCanvas () {
		yield return new WaitForSeconds(0.1f);
		float t = 0;
		while (t < 1.0) {
			t+=3*Time.deltaTime;		
			GameObject.Find("FadeBackdrop").GetComponent<SpriteRenderer>().color = Color.Lerp (new Color(1,1,1,0), new Color(1,1,1,1), t);
			GameObject.Find("TitleCanvas").GetComponent<CanvasGroup>().alpha = Mathf.Lerp (1, 0, t);
			GameObject.Find("TitleUI").transform.localPosition = Vector3.Lerp (Vector3.zero,Vector3.left*20,Mathf.SmoothStep(0.0f,1.0f,t));
			yield return 0;
		}
		GameObject.Find("TitleUI").transform.localPosition = Vector3.zero;
		GameObject.Find("GameController").GetComponent<Controller>().previous_scene = "Title";
		GameObject.Find("GameController").GetComponent<Controller>().showInstructions();
	}

}
