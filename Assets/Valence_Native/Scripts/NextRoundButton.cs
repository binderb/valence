using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class NextRoundButton : MonoBehaviour {

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
		GameObject.Find ("GameController").GetComponent<Controller>().nextRound();
	}
	
	IEnumerator hitButton () {
		Vector3 popScale = new Vector3(1.05f,1.05f,1.0f);
		float t = 0;
		while (t < 1.0) {
			t+=3*Time.deltaTime;		
			transform.localScale = Vector3.Lerp (popScale, Vector3.one,Mathf.SmoothStep(0.0f,1.0f,t));
			GetComponent<RageSpline>().outlineColor1 = Color.Lerp (myHighlight, myColor, Mathf.SmoothStep(0.0f,1.0f,t));
			GameObject.Find("NextRoundLabel").GetComponent<Text>().color = Color.Lerp (myHighlight, myColor, Mathf.SmoothStep(0.0f,1.0f,t));
			GetComponent<RageSpline>().RefreshMesh(true,true,true);
			yield return 0;
		}
	}
}
