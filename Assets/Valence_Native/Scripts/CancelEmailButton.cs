using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CancelEmailButton : MonoBehaviour {

	// Use this for initialization
	void Start () {

	}

	// Update is called once per frame
	void Update () {

	}

	void OnMouseDown () {
			StartCoroutine (showOptionsBox());
	}

	IEnumerator showOptionsBox () {
		GameObject.Find ("InputField").GetComponent<InputField>().text = "";
		GameObject.Find ("OptionsBox").transform.position = new Vector3(-480,320,-200);
		GameObject.Find ("EmailBox").transform.position = new Vector3(-480,2000,-200);

		yield return 0;
	}

}
