using UnityEngine;
using System.Collections;

public class StarSpin : MonoBehaviour {

	public int rotDirection = 1;
	
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		transform.Rotate (Vector3.back, 12*Time.deltaTime*rotDirection);
	}
}