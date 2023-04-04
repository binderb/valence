using UnityEngine;
using System.Collections;

public class ElectronBehavior : MonoBehaviour {
	
	private int degrees;
	public Color myColor;
	public Color myOutline;
	public Color myHighlightColor;
	public Color myHighlightOutline;
	
	
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		transform.rotation = Quaternion.identity;
	}
}