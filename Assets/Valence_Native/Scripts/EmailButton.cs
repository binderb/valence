using UnityEngine;
using System.Collections;

public class EmailButton : MonoBehaviour {

	// Use this for initialization
	void Start () {

	}

	// Update is called once per frame
	void Update () {

	}

	void OnMouseDown () {
			StartCoroutine (showEmailBox());
	}

	IEnumerator showEmailBox () {
		GameObject.Find ("OptionsBox").transform.position = new Vector3(-480,1000,-200);
		GameObject.Find ("EmailBox").transform.position = new Vector3(-480,320,-200);

		yield return 0;
	}

}
