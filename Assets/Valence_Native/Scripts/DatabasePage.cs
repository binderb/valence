using UnityEngine;
using System.Collections;

public class DatabasePage : MonoBehaviour {

	public int direction;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnMouseDown () {
		if (!Controller.inputLock) {
			Controller.inputLock = true;
			StartCoroutine (pageDatabase());
			StartCoroutine (hitButton());
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

	IEnumerator pageDatabase () {
		Camera camera2 = GameObject.Find ("Secondary Camera").GetComponent<Camera>();
		Vector3 startPos = camera2.transform.position;
		float t = 0;
		while (t < 1.0) {
			t+=3*Time.deltaTime;		
			camera2.transform.position = Vector3.Lerp (startPos, startPos+Vector3.down*direction*180, Mathf.SmoothStep(0f,1f,t));
			yield return 0;
		}
		GameObject.Find ("GameController").GetComponent<Controller>().checkDatabaseArrows();
		Controller.inputLock = false;
	}
}
