using UnityEngine;
using System.Collections;

public class PulseColors : MonoBehaviour {
	
	private RageSpline mySpline;
	public Color myDarkColor;
	public Color myLightColor;
	public Color myDarkOutline;
	public Color myLightOutline;
	private float t = 0;
	private float s = 0;
	private bool direction = true;
	private bool outlineDirection = true;


	// Use this for initialization
	void Start () {
		mySpline = GetComponent<RageSpline>();
	}
	
	// Update is called once per frame
	void Update () {

		// Update stroke width
		/*if (outlineDirection) {
			mySpline.OutlineWidth = Mathf.Lerp (11,10,s);
		} else {
			mySpline.OutlineWidth = Mathf.Lerp (10,11,s);
		}*/

		// Update color
		if (direction) {
			//mySpline.fillColor1 = Color.Lerp (myDarkColor,myLightColor,t);
			mySpline.outlineColor1 = Color.Lerp (myDarkOutline,myLightOutline,t);
		} else {
			//mySpline.fillColor1 = Color.Lerp (myLightColor,myDarkColor,t);
			mySpline.outlineColor1 = Color.Lerp (myLightOutline,myDarkOutline,t);
		}
		mySpline.RefreshMesh(true,true,true);
		if (t < 1) {
			t += Time.deltaTime / 2.0f;
		} else if (t >= 1) {
			t = 0;
			direction = !direction;
		}
		if (s < 1) {
			s += Time.deltaTime * 8;
		} else if (s >= 1) {
			s = 0;
			outlineDirection = !outlineDirection;
		}
	}
}
