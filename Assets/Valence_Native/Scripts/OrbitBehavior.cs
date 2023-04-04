using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class OrbitBehavior : MonoBehaviour {
	
	public string mode;
	public GameObject myTarget;
	public Vector2 myTargetPos;
	public GameObject myClosest;
	public GameObject elec;
	
	// Use this for initialization
	void Start () {
		mode = "spin";
	}
	
	// Update is called once per frame
	void Update () {
		if (mode == "spin") {
			transform.RotateAround(transform.parent.transform.position,Vector3.back,12*Time.deltaTime);
			if (transform.parent.GetComponent<AtomBehavior>().orbitVisible && !transform.parent.GetComponent<AtomBehavior>().growing && !transform.parent.GetComponent<AtomBehavior>().shrinking) {
				spaceEvenly();
			}
		} else if (mode == "homing") {
			if (myClosest != null) {
				myTargetPos = new Vector2(myTarget.transform.position.x,myTarget.transform.position.y);
				Vector2 interDistanceAbsolute = myTargetPos - (new Vector2(transform.position.x,transform.position.y));
				Vector2 interDistance = interDistanceAbsolute.normalized;
				Vector2 centerToElec = (myClosest.transform.position - transform.position).normalized;
				float degreesToGo = 0;
				if (interDistanceAbsolute.magnitude > 20) {
					if (Vector2.Dot(interDistance,centerToElec) > -1 && Vector2.Dot(interDistance,centerToElec) < 1) {
						float radiansToGo = Mathf.Acos (Vector2.Dot(interDistance,centerToElec));
						degreesToGo = radiansToGo*180/Mathf.PI;
						bool turnright = Vector3.Cross((Vector3)interDistance,(Vector3)centerToElec).z > 0;
						if (turnright) {
							degreesToGo = -degreesToGo;
						}
					}
					transform.rotation = transform.rotation*Quaternion.AngleAxis(degreesToGo,Vector3.forward);
					
					if (transform.parent.GetComponent<AtomBehavior>().bondTargetOrder == 1) {
						rearrangeForSingle();
					} else if (transform.parent.GetComponent<AtomBehavior>().bondTargetOrder == 2) {
						rearrangeForDouble();
					} else if (transform.parent.GetComponent<AtomBehavior>().bondTargetOrder == 3) {
						rearrangeForTriple();
					}
				}
			} else {
				//mode = "spin";
			}
		}
		
		
	}
	
	public void seek(GameObject target) {
		myTarget = target;
		myTargetPos = new Vector2(myTarget.transform.position.x,myTarget.transform.position.y);
		GameObject elec = findClosest (myTargetPos);
		if (elec != null) {
			myClosest = elec;
			Vector3 interDistance = (myTargetPos - (Vector2)transform.position).normalized;
			Vector3 centerToElec = ((Vector2)elec.transform.position - (Vector2)transform.position).normalized;
			float radiansToGo = Mathf.Acos (Vector2.Dot(interDistance,centerToElec));
			float degreesToGo = radiansToGo*180/Mathf.PI;
			bool turnright = Vector3.Cross((Vector3)interDistance,(Vector3)centerToElec).z > 0;
			if (turnright) {
				degreesToGo = -degreesToGo;
			}
			StartCoroutine (movebyDegrees(degreesToGo));
		}
	}
	
	public void lookTo(GameObject target) {
		myTarget = target;
		myTargetPos = myTarget.transform.position;
		GameObject elec = findClosest (myTargetPos);
		if (elec != null) {
			myClosest = elec;
			Vector3 interDistance = ((Vector2)myTargetPos - (Vector2)transform.position).normalized;
			Vector3 centerToElec = ((Vector2)elec.transform.position - (Vector2)transform.position).normalized;
			float radiansToGo = Mathf.Acos (Vector2.Dot(interDistance,centerToElec));
			float degreesToGo = radiansToGo*180/Mathf.PI;
			bool turnright = Vector3.Cross((Vector3)interDistance,(Vector3)centerToElec).z > 0;
			if (turnright) {
				degreesToGo = -degreesToGo;
			}
			transform.rotation = transform.rotation*Quaternion.AngleAxis(degreesToGo,Vector3.forward);
			mode = "homing";
		}
	}
	
	IEnumerator movebyDegrees (float degrees) {
		float t = 0;
		Quaternion initRot = transform.rotation;
		while (t < 1.0) {
			t+=6*Time.deltaTime;
			transform.rotation = Quaternion.Lerp (initRot,initRot*Quaternion.AngleAxis(degrees,Vector3.forward),t);
			yield return 0;
		}
		mode = "homing";
	}
	
	GameObject findClosest (Vector2 targetPos) {
		GameObject[] electrons = GameObject.FindGameObjectsWithTag("electron");
		float minDistance = 500.0f;
		int minIndex = -1;
		for (int i=0;i<electrons.Length;i++) {
			if (electrons[i].transform.parent == gameObject.transform) {
				electrons[i].GetComponent<RageSpline>().fillColor1 = electrons[i].GetComponent<ElectronBehavior>().myColor;
				electrons[i].GetComponent<RageSpline>().outlineColor1 = electrons[i].GetComponent<ElectronBehavior>().myOutline;
				electrons[i].GetComponent<RageSpline>().RefreshMesh(true,true,false);
				if (Vector2.Distance((Vector2)electrons[i].transform.position, targetPos) < minDistance) {
					minDistance = Vector2.Distance((Vector2)electrons[i].transform.position, targetPos);
					minIndex = i;
				}
			}
		}
		if (minIndex > -1) {
			//print(transform.parent.GetComponent<tk2dSprite>().CurrentSprite.name);
			//print(transform.parent.GetComponent<AtomBehavior>().bondTargetOrder);
			return electrons[minIndex];
		} else {
			return null;	
		}
	}
	
	void rearrangeForSingle() {
		
		GameObject[] electrons = GameObject.FindGameObjectsWithTag("electron");
		List<GameObject> myElectrons = new List<GameObject>();
		for (int i=0;i<electrons.Length;i++) {
			if (electrons[i].transform.parent == transform) {
				myElectrons.Add(electrons[i]);
				electrons[i].GetComponent<RageSpline>().fillColor1 = electrons[i].GetComponent<ElectronBehavior>().myColor;
				electrons[i].GetComponent<RageSpline>().outlineColor1 = electrons[i].GetComponent<ElectronBehavior>().myOutline;
				electrons[i].GetComponent<RageSpline>().RefreshMesh(true,true,false);
			}
		}
		int availableValence = transform.parent.GetComponent<AtomBehavior>().availableValence;
		if (availableValence < myElectrons.Count) {
			Destroy (myElectrons[myElectrons.Count-1]);
		}
		
		for (int i=0;i<availableValence;i++) {
			float rotAmount = (360*i)/availableValence;
			myElectrons[i].transform.localPosition = myClosest.transform.localPosition;
			myElectrons[i].transform.RotateAround(transform.parent.transform.position,Vector3.back,rotAmount);
		}
		
		myClosest = myElectrons[0];
		myClosest.GetComponent<Renderer>().enabled = true;
		myClosest.GetComponent<RageSpline>().fillColor1 = myClosest.GetComponent<ElectronBehavior>().myHighlightColor;
		myClosest.GetComponent<RageSpline>().outlineColor1 = myClosest.GetComponent<ElectronBehavior>().myHighlightOutline;
		myClosest.GetComponent<RageSpline>().RefreshMesh(true,true,false);
	}
	
	void rearrangeForDouble() {
		
		GameObject[] electrons = GameObject.FindGameObjectsWithTag("electron");
		List<GameObject> myElectrons = new List<GameObject>();
		for (int i=0;i<electrons.Length;i++) {
			if (electrons[i].transform.parent == transform) {
				myElectrons.Add(electrons[i]);
				electrons[i].GetComponent<Renderer>().enabled = true;
				electrons[i].GetComponent<RageSpline>().fillColor1 = electrons[i].GetComponent<ElectronBehavior>().myColor;
				electrons[i].GetComponent<RageSpline>().outlineColor1 = electrons[i].GetComponent<ElectronBehavior>().myOutline;
				electrons[i].GetComponent<RageSpline>().RefreshMesh(true,true,false);
			}
		}
		int availableValence = transform.parent.GetComponent<AtomBehavior>().availableValence;
		int spacedValence = availableValence-1;
		
		for (int i=0;i<spacedValence;i++) {
			float rotAmount = (360*i)/spacedValence;
			myElectrons[i].transform.localPosition = myClosest.transform.localPosition;
			myElectrons[i].transform.RotateAround(transform.parent.transform.position,Vector3.back,rotAmount);
		}
		myClosest = myElectrons[0];
		myClosest.GetComponent<Renderer>().enabled = false;
		myElectrons[spacedValence].transform.localPosition = myClosest.transform.localPosition;
		myElectrons[spacedValence].transform.RotateAround(transform.parent.transform.position,Vector3.back,10);
		myElectrons[spacedValence].GetComponent<RageSpline>().fillColor1 = myElectrons[spacedValence].GetComponent<ElectronBehavior>().myHighlightColor;
		myElectrons[spacedValence].GetComponent<RageSpline>().outlineColor1 = myElectrons[spacedValence].GetComponent<ElectronBehavior>().myHighlightOutline;
		myElectrons[spacedValence].GetComponent<RageSpline>().RefreshMesh(true,true,false);

		GameObject secondE;
		if (myElectrons.Count == availableValence) {
			GameObject newElectron = (GameObject)Instantiate (elec);
			newElectron.transform.parent = transform;
			secondE = newElectron;
		} else {
			secondE = myElectrons[availableValence];
		}
		secondE.transform.localPosition = myClosest.transform.localPosition;
		secondE.transform.RotateAround(transform.parent.transform.position,Vector3.back,-10);
		secondE.GetComponent<RageSpline>().fillColor1 = secondE.GetComponent<ElectronBehavior>().myHighlightColor;
		secondE.GetComponent<RageSpline>().outlineColor1 = secondE.GetComponent<ElectronBehavior>().myHighlightOutline;
		secondE.GetComponent<RageSpline>().RefreshMesh(true,true,false);

		myClosest.GetComponent<RageSpline>().fillColor1 = myClosest.GetComponent<ElectronBehavior>().myHighlightColor;
		myClosest.GetComponent<RageSpline>().outlineColor1 = myClosest.GetComponent<ElectronBehavior>().myHighlightOutline;
		myClosest.GetComponent<RageSpline>().RefreshMesh(true,true,false);
		
	}
	
	void rearrangeForTriple() {
		
		GameObject[] electrons = GameObject.FindGameObjectsWithTag("electron");
		List<GameObject> myElectrons = new List<GameObject>();
		for (int i=0;i<electrons.Length;i++) {
			if (electrons[i].transform.parent == transform) {
				myElectrons.Add(electrons[i]);
				electrons[i].GetComponent<Renderer>().enabled = true;
				electrons[i].GetComponent<RageSpline>().fillColor1 = electrons[i].GetComponent<ElectronBehavior>().myColor;
				electrons[i].GetComponent<RageSpline>().outlineColor1 = electrons[i].GetComponent<ElectronBehavior>().myOutline;
				electrons[i].GetComponent<RageSpline>().RefreshMesh(true,true,false);
			}
		}
		int availableValence = transform.parent.GetComponent<AtomBehavior>().availableValence;
		if (availableValence < myElectrons.Count) {
			Destroy (myElectrons[myElectrons.Count-1]);
		}
		int spacedValence = availableValence-2;
		
		for (int i=0;i<spacedValence;i++) {
			float rotAmount = (360*i)/spacedValence;
			myElectrons[i].transform.localPosition = myClosest.transform.localPosition;
			myElectrons[i].transform.RotateAround(transform.parent.transform.position,Vector3.back,rotAmount);
		}
		myClosest = myElectrons[0];
		myElectrons[spacedValence].transform.localPosition = myClosest.transform.localPosition;
		myElectrons[spacedValence].transform.RotateAround(transform.parent.transform.position,Vector3.back,20);
		myElectrons[spacedValence].GetComponent<RageSpline>().fillColor1 = myElectrons[spacedValence].GetComponent<ElectronBehavior>().myHighlightColor;
		myElectrons[spacedValence].GetComponent<RageSpline>().outlineColor1 = myElectrons[spacedValence].GetComponent<ElectronBehavior>().myHighlightOutline;
		myElectrons[spacedValence].GetComponent<RageSpline>().RefreshMesh(true,true,false);
		myElectrons[spacedValence+1].transform.localPosition = myClosest.transform.localPosition;
		myElectrons[spacedValence+1].transform.RotateAround(transform.parent.transform.position,Vector3.back,-20);
		myElectrons[spacedValence+1].GetComponent<RageSpline>().fillColor1 = myElectrons[spacedValence+1].GetComponent<ElectronBehavior>().myHighlightColor;
		myElectrons[spacedValence+1].GetComponent<RageSpline>().outlineColor1 = myElectrons[spacedValence+1].GetComponent<ElectronBehavior>().myHighlightOutline;
		myElectrons[spacedValence+1].GetComponent<RageSpline>().RefreshMesh(true,true,false);

		myClosest.GetComponent<RageSpline>().fillColor1 = myClosest.GetComponent<ElectronBehavior>().myHighlightColor;
		myClosest.GetComponent<RageSpline>().outlineColor1 = myClosest.GetComponent<ElectronBehavior>().myHighlightOutline;
		myClosest.GetComponent<RageSpline>().RefreshMesh(true,true,false);
	}

	void spaceEvenly () {
		GameObject[] electrons = GameObject.FindGameObjectsWithTag("electron");
		for (int i=0;i<electrons.Length;i++) {
			if (electrons[i].transform.parent == transform) {
				electrons[i].transform.localPosition = new Vector3(Mathf.Cos((i*Mathf.PI*2/transform.parent.GetComponent<AtomBehavior>().availableValence)+(Mathf.PI/2)),Mathf.Sin((i*Mathf.PI*2/transform.parent.GetComponent<AtomBehavior>().availableValence)+(Mathf.PI/2)),0.0f) * 91.0f;
			}
		}
	}
	
}
