using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ReadMoreButton : MonoBehaviour {

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
	}
	
	IEnumerator hitButton () {
		Vector3 popScale = new Vector3(1.05f,1.05f,1.0f);
		float t = 0;
		while (t < 1.0) {
			t+=3*Time.deltaTime;		
			transform.localScale = Vector3.Lerp (popScale, Vector3.one,Mathf.SmoothStep(0.0f,1.0f,t));
			GetComponent<RageSpline>().outlineColor1 = Color.Lerp (myHighlight, myColor, Mathf.SmoothStep(0.0f,1.0f,t));
			GameObject.Find("ReadMoreLabel").GetComponent<Text>().color = Color.Lerp (myHighlight, myColor, Mathf.SmoothStep(0.0f,1.0f,t));
			GetComponent<RageSpline>().RefreshMesh(true,true,true);
			yield return 0;
		}
		t = 0;
		while (t < 1.0) {
			t+=3*Time.deltaTime;		
			GameObject.Find("GameCanvas").GetComponent<CanvasGroup>().alpha = Mathf.Lerp (1, 0, t);
			GameObject.Find("InfoCanvas").GetComponent<CanvasGroup>().alpha = Mathf.Lerp (1, 0, t);

			GameObject.Find("GameCover").GetComponent<SpriteRenderer>().color = Color.Lerp (new Color(1f,1f,1f,0f),Color.white,t);
			yield return 0;
		}
		GameObject.Find("GameController").GetComponent<Controller>().previous_scene = "NewCodexEntry";
		GameObject.Find ("GameController").GetComponent<Controller>().showDatabase();
		GameObject.Find ("GameController").GetComponent<Controller>().showCodex(GameObject.FindGameObjectWithTag("molecule").GetComponent<MoleculeBehavior>(),"GameWindow");

		t = 0;
		while (t < 1.0) {
			t+=3*Time.deltaTime;		
			GameObject.Find("DatabaseCanvas").GetComponent<CanvasGroup>().alpha = Mathf.Lerp (0, 1, t);
			GameObject.Find("DatabaseCover").GetComponent<SpriteRenderer>().color = Color.Lerp (Color.white,new Color(1f,1f,1f,0f),t);
			yield return 0;
		}
	}
}
