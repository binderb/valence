using UnityEngine;
using System.Collections;

public class CloseCodexButton : MonoBehaviour {
	
	public int direction;
	
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	
	void OnMouseDown () {
		if (!Controller.inputLock) {
			StartCoroutine (closeCodex());
		}
		
	}
	
	IEnumerator hitButton () {
		Vector3 popScale = new Vector3(1.005f,1.3f,1.0f);
		transform.localScale = popScale;
		float t = 0;
		while (t < 1.0) {
			t+=3*Time.deltaTime;		
			transform.localScale = Vector3.Lerp (popScale, Vector3.one,Mathf.SmoothStep(0.0f,1.0f,t));
			yield return 0;
		}
	}
	
	IEnumerator closeCodex () {
		GameObject.Find ("CodexCanvas").GetComponent<CanvasGroup>().alpha = 0;
		GameObject.Find ("Secondary Camera").GetComponent<Camera>().enabled = true;
		GameObject.Find ("GameController").GetComponent<Controller>().resetDatabase();
		yield return 0;
	}
}
