using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SendEmailButton : MonoBehaviour {

	// Use this for initialization
	void Start () {

	}

	// Update is called once per frame
	void Update () {

	}

	void OnMouseDown () {
			StartCoroutine (sendMyMessage());
	}

	IEnumerator sendMyMessage () {

		GameObject.Find("SendingMessage").GetComponent<Text>().color = Color.white;
		yield return new WaitForSeconds(0.02f);

		string myMessage = GameObject.Find("InputField").GetComponent<InputField>().text;

		GameObject.Find("GameController").GetComponent<Controller>().sendMail(myMessage);

		yield return 0;
	}

}
