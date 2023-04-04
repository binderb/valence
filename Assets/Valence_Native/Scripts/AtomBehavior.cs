using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class AtomBehavior : MonoBehaviour {

	public Vector3 mouseDownPosition;
	public Vector3 initPos;
	public Color myColor;
	public Color myHighlight;
	public Transform myOrbit;
	public Transform elec;
	public Transform normalbond;
	public Transform scoreFlag;
	public string element;
	public int weight;
	public int valence;
	public int availableValence;
	public bool growing;
	public bool shrinking;
	public bool orbitVisible;
	public bool rearranging;
	public GameObject bondTarget;
	public GameObject molecule;
	private GameObject atomText;
	private GameObject valenceText;
	private GameObject controller;
	public int bondTargetOrder;
	public bool mouseLocked = false;
	public bool isMolecule = false;
	public List<string> myBondMates = new List<string>();
	public List<Transform> myBondAtoms = new List<Transform>();
	public List<int> myBondOrders = new List<int>();
	private List<GameObject> substituentChain = new List<GameObject>();
	private float BondLength;
	public List<string> builtMolecules = new List<string>();
	public List<string> newCompounds = new List<string>();
	public bool uniqueStructure = false;
	public string moleculeName;
	private bool moving;


	// Use this for initialization
	void Start () {
		GetComponent<RageSpline>().fillColor1 = myColor;
		atomText = GameObject.Find("AtomText");
		valenceText = GameObject.Find("ValenceText");
		controller = GameObject.Find ("GameController");
	}

	// Update is called once per frame
	void Update () {

	}

	void OnMouseEnter() {
		if (!Controller.inputLock && !(Input.GetMouseButton(0)) && !mouseLocked && !controller.GetComponent<Controller>().locked) {
			atomText.GetComponent<Text>().text = "Atom: " + element;
			valenceText.GetComponent<Text>().text = "Valence: " + valence;
			transform.localScale = new Vector3(1.0f,1.0f,1.0f);
			GetComponent<RageSpline>().fillColor1 = myHighlight;
			GetComponent<RageSpline>().RefreshMesh(true, true, false);
			shrinking = false;
			if (transform.parent == null || transform.parent.tag != "archived") {
				StartCoroutine (RevealOrbit());
			} else if (transform.parent != null) {
				transform.parent.GetComponent<MoleculeBehavior>().startRevealOrbit();
			}
		}

	}

	void OnMouseExit() {
		if (!Controller.inputLock && !mouseLocked && !(Input.GetMouseButton(0))) {
			atomText.GetComponent<Text>().text = "Atom:";
			valenceText.GetComponent<Text>().text = "Valence:";
			transform.localScale = new Vector3(1.0f,1.0f,1.0f);
			GetComponent<RageSpline>().fillColor1 = myColor;
			GetComponent<RageSpline>().RefreshMesh(true, true, false);
			if (transform.parent == null || transform.parent.tag != "archived") {
				startHideOrbit();
			} else if (transform.parent != null) {
				transform.parent.GetComponent<MoleculeBehavior>().startHideOrbit();
			}

		}
	}

	void OnMouseDown() {
		if (!Controller.inputLock) {
			if (transform.parent == null || transform.parent.tag != "archived") {
				if (!mouseLocked && !mouseLocked && !controller.GetComponent<Controller>().locked) {
					mouseDownPosition = Input.mousePosition;
					if (!isMolecule) {
						initPos = transform.position;
					} else {
						initPos = transform.parent.transform.position;
					}
				}
			} else if (transform.parent != null && transform.parent.tag == "archived" && controller.GetComponent<Controller>().codexDictionary.ContainsKey(transform.parent.GetComponent<MoleculeBehavior>().myMoleculeName))  {
				controller.GetComponent<Controller>().showCodex(transform.parent.GetComponent<MoleculeBehavior>(),"Database");
			}
		}
	}

	void OnMouseDrag() {
		if (!Controller.inputLock) {
			if (transform.parent == null || transform.parent.tag != "archived") {
				if (!mouseLocked && !controller.GetComponent<Controller>().locked) {
					Vector3 mouseDiff = new Vector3((int)(Input.mousePosition.x - mouseDownPosition.x),(int)(Input.mousePosition.y - mouseDownPosition.y),0.0f);
					if (!isMolecule) {
						transform.position = initPos + mouseDiff;
						checkNearbyAtoms();
					} else {
						transform.parent.transform.position = initPos + mouseDiff;
					}
					// Update valence if there's a bond partner nearby
					if (myOrbit.GetComponent<OrbitBehavior>().mode == "spin") {
						valenceText.GetComponent<Text>().text = "Valence: " + valence;
					} else {
						valenceText.GetComponent<Text>().text = "Valence: " + (valence+bondTargetOrder);
					}

					// Restrict movement to window bounds
					if (!isMolecule) {
						if (transform.position.x < (-960f+(GetComponent<Renderer>().bounds.size.x/2))) {transform.position = new Vector3(-960f+(GetComponent<Renderer>().bounds.size.x/2),transform.position.y,transform.position.z);}
						if (transform.position.x > (0f-(GetComponent<Renderer>().bounds.size.x/2))) {transform.position = new Vector3(0f-(GetComponent<Renderer>().bounds.size.x/2),transform.position.y,transform.position.z);}
						if (transform.position.y < (140f+(GetComponent<Renderer>().bounds.size.y/2))) {transform.position = new Vector3(transform.position.x,140f+(GetComponent<Renderer>().bounds.size.y/2),transform.position.z);}
						if (transform.position.y > (470f-(GetComponent<Renderer>().bounds.size.y/2))) {transform.position = new Vector3(transform.position.x,470f-(GetComponent<Renderer>().bounds.size.y/2),transform.position.z);}
					} else {
						float minX = transform.parent.GetComponentInChildren<Transform>().position.x;
						float maxX = transform.parent.GetComponentInChildren<Transform>().position.x;
						float minY = transform.parent.GetComponentInChildren<Transform>().position.y;
						float maxY = transform.parent.GetComponentInChildren<Transform>().position.y;
						foreach (Transform child in transform.parent) {
							if ((child.transform.position.x - (child.GetComponent<Renderer>().bounds.size.x/2) ) < minX) {minX = child.transform.position.x - (child.GetComponent<Renderer>().bounds.size.x/2);}
							if ((child.transform.position.x + (child.GetComponent<Renderer>().bounds.size.x/2) ) > maxX) {maxX = child.transform.position.x + (child.GetComponent<Renderer>().bounds.size.x/2);}
							if ((child.transform.position.y - (child.GetComponent<Renderer>().bounds.size.y/2) ) < minY) {minY = child.transform.position.y - (child.GetComponent<Renderer>().bounds.size.y/2);}
							if ((child.transform.position.y + (child.GetComponent<Renderer>().bounds.size.y/2) ) > maxY) {maxY = child.transform.position.y + (child.GetComponent<Renderer>().bounds.size.y/2);}
						}
						float moleculeWidth = maxX - minX;
						float moleculeHeight = maxY - minY;
						if (transform.parent.position.x < (-960f+(moleculeWidth/2))) {transform.parent.position = new Vector3(-960f+(moleculeWidth/2),transform.parent.position.y,transform.parent.position.z);}
						if (transform.parent.position.x > (0f-(moleculeWidth/2))) {transform.parent.position = new Vector3(0f-(moleculeWidth/2),transform.parent.position.y,transform.parent.position.z);}
						if (transform.parent.position.y < (140f+(moleculeHeight/2))) {transform.parent.position = new Vector3(transform.parent.position.x,140f+(moleculeHeight/2),transform.parent.position.z);}
						if (transform.parent.position.y > (470f-(moleculeHeight/2))) {transform.parent.position = new Vector3(transform.parent.position.x,470f-(moleculeHeight/2),transform.parent.position.z);}
					}
				}
			}
		}
	}

	void OnMouseUp() {
		if (!Controller.inputLock) {
			if (transform.parent == null || transform.parent.tag != "archived") {
				if (bondTarget != null && !isMolecule) {
					GameObject theMolecule = GameObject.FindGameObjectWithTag("molecule");
					if (theMolecule == null || (theMolecule !=null && bondTarget.transform.parent != null && bondTarget.transform.parent == theMolecule.transform)) {
						StartCoroutine (BondAtoms());
					}
					startHideOrbit();
					bondTarget.GetComponent<AtomBehavior>().startHideOrbit();
				}
				RaycastHit hit;
				Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
				if (!Physics.Raycast ( ray,out hit,1000f)) {
					startHideOrbit();
				}
			}
		}
	}

	IEnumerator BondAtoms() {
		mouseLocked = true;
		GameObject theMolecule = null;
		if (bondTarget.name == "Hydrogen(Clone)" || transform.name == "Hydrogen(Clone)") {BondLength = 60.0f;} else {BondLength = 80.0f;}
		float bondTransformY = BondLength/3.0f * (1.0f / 52.0f);

		/* NEW IN VERSION 2: If the user adds atoms to an existing molecule,
		 * rearrange appropriate groups before performing the bonding routine,
		 * so that appropriate bonding angles will be used. */
		if (bondTarget.GetComponent<AtomBehavior>().isMolecule) {
			rearranging = true;
			StartCoroutine (OptimizeAngles());
			while(rearranging) {
				yield return 0;
			}
		}

		/* NEW IN VERSION 2: Since atoms are now multilayered objects with different
		 * z-positions, it is necessary to perform all rotations and translations
		 * carefully in the 2D plane, as opposed to using objects' 3D coordinates. */

		Vector2 interBond = ((Vector2)bondTarget.transform.position - (Vector2)transform.position).normalized;
		Vector2 interPoint = ((Vector2)bondTarget.transform.position + (Vector2)transform.position)/2;
		Vector2 BondPos = interPoint;
		float t = 0;

		myBondMates.Add(bondTarget.GetComponent<AtomBehavior>().element);
		myBondOrders.Add(bondTargetOrder);
		myBondAtoms.Add(bondTarget.transform);
		bondTarget.GetComponent<AtomBehavior>().myBondMates.Add(GetComponent<AtomBehavior>().element);
		bondTarget.GetComponent<AtomBehavior>().myBondOrders.Add(bondTargetOrder);
		bondTarget.GetComponent<AtomBehavior>().myBondAtoms.Add(transform);

		if (!isMolecule && !bondTarget.GetComponent<AtomBehavior>().isMolecule) { //is the target atom completely unbonded? Am I?
			if (GameObject.FindGameObjectsWithTag("molecule").Length == 0) {
				theMolecule = (GameObject)Instantiate(molecule);
				theMolecule.GetComponent<MoleculeBehavior>().myAtoms.Add (bondTarget);
				Vector2 initTarget = (Vector2)bondTarget.transform.position;
				Vector2 initPos = (Vector2)transform.position;
				while (t < 1.0) {
					t+=6*Time.deltaTime;
					bondTarget.transform.position = Vector3.Lerp (new Vector3(initTarget.x,initTarget.y,bondTarget.transform.position.z), new Vector3(interPoint.x,interPoint.y,bondTarget.transform.position.z)+(Vector3)interBond*BondLength/2.0f,t);
					transform.position = Vector3.Lerp (new Vector3(initPos.x,initPos.y,transform.position.z), new Vector3(interPoint.x,interPoint.y,transform.position.z)-(Vector3)interBond*BondLength/2.0f,t);
					yield return 0;
				}
			} else {
				goto moleculeAlreadyExists;
			}
			theMolecule.transform.position = interPoint;
		} else if (bondTarget.GetComponent<AtomBehavior>().isMolecule) {
			theMolecule = bondTarget.transform.parent.gameObject;
			BondPos = (Vector2)bondTarget.transform.position - interBond*BondLength/2.0f;
		}

		transform.parent = theMolecule.transform; //set participating atom properties
		theMolecule.GetComponent<MoleculeBehavior>().myAtoms.Add(gameObject);
		bondTarget.transform.parent = theMolecule.transform;
		isMolecule = true;
		bondTarget.GetComponent<AtomBehavior>().isMolecule = true;
		valence = valence + bondTargetOrder;
		bondTarget.GetComponent<AtomBehavior>().valence = bondTarget.GetComponent<AtomBehavior>().valence + bondTargetOrder;
		availableValence = availableValence - bondTargetOrder;
		bondTarget.GetComponent<AtomBehavior>().availableValence = bondTarget.GetComponent<AtomBehavior>().availableValence - bondTargetOrder;

		t = 0; //Create Bonds
		Vector3 initScale = new Vector3(0.0f,bondTransformY,1.0f);
		float BondRot = Vector3.Angle(Vector3.down,(Vector3)interBond);
		if (Vector3.Cross(Vector3.down, interBond).z > 0) {BondRot = -BondRot;}
		switch (bondTargetOrder) {
		case 1:
			Transform single = (Transform) Instantiate(normalbond);
			single.GetComponent<RageSpline>().SetFill(RageSpline.Fill.Gradient);
			single.GetComponent<RageSpline>().fillColor1 = GetComponent<RageSpline>().outlineColor1;
			single.GetComponent<RageSpline>().fillColor2 = bondTarget.GetComponent<RageSpline>().outlineColor1;
			single.GetComponent<RageSpline>().RefreshMesh(true,true,false);
			single.position = new Vector3(BondPos.x,BondPos.y,200);
			single.localScale = initScale;
			single.Rotate (Vector3.back,BondRot);
			while (t < 1.0) {
				t+=15*Time.deltaTime;
				single.localScale = Vector3.Slerp (initScale,new Vector3(1.0f,bondTransformY,1.0f),t);
				yield return 0;
			}
			single.parent = theMolecule.transform;
			break;
		case 2:
			Transform double1 = (Transform) Instantiate(normalbond);
			Transform double2 = (Transform) Instantiate(normalbond);
			double1.GetComponent<RageSpline>().SetFill(RageSpline.Fill.Gradient);
			double1.GetComponent<RageSpline>().fillColor1 = GetComponent<RageSpline>().outlineColor1;
			double1.GetComponent<RageSpline>().fillColor2 = bondTarget.GetComponent<RageSpline>().outlineColor1;
			double1.GetComponent<RageSpline>().RefreshMesh(true,true,false);
			double2.GetComponent<RageSpline>().SetFill(RageSpline.Fill.Gradient);
			double2.GetComponent<RageSpline>().fillColor1 = GetComponent<RageSpline>().outlineColor1;
			double2.GetComponent<RageSpline>().fillColor2 = bondTarget.GetComponent<RageSpline>().outlineColor1;
			double2.GetComponent<RageSpline>().RefreshMesh(true,true,false);
			double1.position = (new Vector3(BondPos.x,BondPos.y,200) + Vector3.Cross (Vector3.back,interBond)*5);
			double2.position = (new Vector3(BondPos.x,BondPos.y,200) + Vector3.Cross (Vector3.back,interBond)*-5);
			double1.localScale = initScale;
			double2.localScale = initScale;
			double1.Rotate (Vector3.back,BondRot);
			double2.Rotate (Vector3.back,BondRot);
			while (t < 1.0) {
				t+=15*Time.deltaTime;
				double1.localScale = Vector3.Slerp (initScale,new Vector3(0.5f,bondTransformY,1.0f),t);
				double2.localScale = Vector3.Slerp (initScale,new Vector3(0.5f,bondTransformY,1.0f),t);
				yield return 0;
			}
			double1.parent = theMolecule.transform;
			double2.parent = theMolecule.transform;
			break;
		case 3:
			Transform triple1 = (Transform) Instantiate(normalbond);
			Transform triple2 = (Transform) Instantiate(normalbond);
			Transform triple3 = (Transform) Instantiate(normalbond);
			triple1.GetComponent<RageSpline>().SetFill(RageSpline.Fill.Gradient);
			triple1.GetComponent<RageSpline>().fillColor1 = GetComponent<RageSpline>().outlineColor1;
			triple1.GetComponent<RageSpline>().fillColor2 = bondTarget.GetComponent<RageSpline>().outlineColor1;
			triple1.GetComponent<RageSpline>().RefreshMesh(true,true,false);
			triple2.GetComponent<RageSpline>().SetFill(RageSpline.Fill.Gradient);
			triple2.GetComponent<RageSpline>().fillColor1 = GetComponent<RageSpline>().outlineColor1;
			triple2.GetComponent<RageSpline>().fillColor2 = bondTarget.GetComponent<RageSpline>().outlineColor1;
			triple2.GetComponent<RageSpline>().RefreshMesh(true,true,false);
			triple3.GetComponent<RageSpline>().SetFill(RageSpline.Fill.Gradient);
			triple3.GetComponent<RageSpline>().fillColor1 = GetComponent<RageSpline>().outlineColor1;
			triple3.GetComponent<RageSpline>().fillColor2 = bondTarget.GetComponent<RageSpline>().outlineColor1;
			triple3.GetComponent<RageSpline>().RefreshMesh(true,true,false);
			triple1.position = new Vector3(BondPos.x,BondPos.y,200);
			triple2.position = (new Vector3(BondPos.x,BondPos.y,200) + Vector3.Cross (Vector3.back,interBond)*10);
			triple3.position = (new Vector3(BondPos.x,BondPos.y,200) + Vector3.Cross (Vector3.back,interBond)*-10);
			triple1.localScale = initScale;
			triple2.localScale = initScale;
			triple3.localScale = initScale;
			triple1.Rotate (Vector3.back,BondRot);
			triple2.Rotate (Vector3.back,BondRot);
			triple3.Rotate (Vector3.back,BondRot);
			while (t < 1.0) {
				t+=15*Time.deltaTime;
				triple1.localScale = Vector3.Slerp (initScale,new Vector3(0.33f,bondTransformY,1.0f),t);
				triple2.localScale = Vector3.Slerp (initScale,new Vector3(0.33f,bondTransformY,1.0f),t);
				triple3.localScale = Vector3.Slerp (initScale,new Vector3(0.33f,bondTransformY,1.0f),t);
				yield return 0;
			}
			triple1.parent = theMolecule.transform;
			triple2.parent = theMolecule.transform;
			triple3.parent = theMolecule.transform;
			break;
		}

		//recalculate molecule position
		float minX = theMolecule.GetComponentInChildren<Transform>().position.x;
		float maxX = theMolecule.GetComponentInChildren<Transform>().position.x;
		float minY = theMolecule.GetComponentInChildren<Transform>().position.y;
		float maxY = theMolecule.GetComponentInChildren<Transform>().position.y;
		foreach (Transform child in theMolecule.transform) {
			if (child.transform.position.x < minX) {minX = child.transform.position.x;}
			if (child.transform.position.x > maxX) {maxX = child.transform.position.x;}
			if (child.transform.position.y < minY) {minY = child.transform.position.y;}
			if (child.transform.position.y > maxY) {maxY = child.transform.position.y;}
		}
		Vector3 moleculeCenter = new Vector3((minX+maxX)/2, (minY+maxY)/2, theMolecule.transform.position.z);
		Vector3 toCenter = moleculeCenter - theMolecule.transform.position;
		theMolecule.transform.position += toCenter;
		foreach (Transform child in theMolecule.transform) {
			child.localPosition -= toCenter;
		}
		theMolecule.GetComponent<MoleculeBehavior>().checkValence();

		//assign score
		int score = bondTargetOrder*50;
		if (bondTarget.GetComponent<AtomBehavior>().element == "Hydrogen" || element == "Hydrogen") {
			score = 15;
		}
		controller.GetComponent<Controller>().showFlag(interPoint,score);
		controller.GetComponent<Controller>().addScore(score);


		// Reset colors
		GetComponent<RageSpline>().SetFillColor1(myColor);
		GetComponent<RageSpline>().RefreshMesh(true,true,true);
		foreach (Transform a in transform.parent) {
			if (a.GetComponent<AtomBehavior>() != null) {
				a.GetComponent<RageSpline>().SetFillColor1(a.GetComponent<AtomBehavior>().myColor);
				a.GetComponent<RageSpline>().RefreshMesh (true,true,true);
			}
		}

	moleculeAlreadyExists:
			mouseLocked = false;
	}


	public void startRevealOrbit() {
		shrinking = false;
		StartCoroutine (RevealOrbit());
	}

	public void startHideOrbit() {
		growing = false;
		GameObject[] electrons = GameObject.FindGameObjectsWithTag("electron");
		foreach (GameObject e in electrons) {
			if (e.transform.parent == myOrbit.transform) {
				Destroy (e);
			}
		}
		StartCoroutine (HideOrbit());
	}

	IEnumerator RevealOrbit() {
		if (isMolecule && transform.parent.GetComponent<MoleculeBehavior>() != null) {
			transform.parent.GetComponent<MoleculeBehavior>().startRevealOrbit();
		}
		if (availableValence > 0) {
			if (!isMolecule) {
				orbitVisible = true;
				growing = true;
				float t = 0;
				Vector3 initScale = myOrbit.transform.localScale;
				while (t < 1.0 && growing) {
					t+=6*Time.deltaTime;
					myOrbit.transform.localScale = Vector3.Lerp (initScale, new Vector3(0.55f,0.55f,0.55f),t);
					yield return 0;
				}
				if (growing) {
					for (int i=0;i<availableValence;i++) {
						Transform myElec = (Transform)Instantiate (elec);
						myElec.parent = myOrbit.transform;
						myElec.localPosition = new Vector3(Mathf.Cos((i*Mathf.PI*2/availableValence)+(Mathf.PI/2)),Mathf.Sin((i*Mathf.PI*2/availableValence)+(Mathf.PI/2)),0f) * 91.0f;
					}
				}
				growing = false;
			} else {
				orbitVisible = true;
				growing = true;
				float t = 0;
				Vector3 initScale = myOrbit.transform.localScale;
				while (t < 1.0 && growing) {
					t+=6*Time.deltaTime;
					myOrbit.transform.localScale = Vector3.Lerp (initScale, new Vector3(0.55f,0.55f,0.55f),t);
					yield return 0;
				}
				if (growing) {
					for (int i=0;i<availableValence;i++) {
						Transform myElec = (Transform)Instantiate (elec);
						myElec.parent = myOrbit.transform;
						myElec.localPosition = new Vector3(Mathf.Cos((i*Mathf.PI*2/availableValence)+(Mathf.PI/2)),Mathf.Sin((i*Mathf.PI*2/availableValence)+(Mathf.PI/2)),0f) * 91.0f;
					}
				}
				growing = false;
			}
		}
	}

	IEnumerator HideOrbit() {
		if (isMolecule && transform.parent.GetComponent<MoleculeBehavior>() != null) {
			transform.parent.GetComponent<MoleculeBehavior>().startHideOrbit();
		}

		shrinking = true;
		float t = 0;
		Vector3 initScale = myOrbit.transform.localScale;
		while (t < 1.0 && shrinking) {
			t+=6*Time.deltaTime;
			myOrbit.transform.localScale = Vector3.Lerp (initScale, new Vector3(0.0f,0.0f,0.8f),t);
			yield return 0;
		}
		shrinking = false;
		orbitVisible = false;
		myOrbit.GetComponent<OrbitBehavior>().mode = "spin";
	}

	void checkNearbyAtoms() {
		GameObject[] atoms = GameObject.FindGameObjectsWithTag("atom");
		bool searching = true;
		bool found = false;
		foreach (GameObject a in atoms) {
			if (searching && a.gameObject != this.gameObject && Vector3.Distance (a.transform.position,transform.position) < 140.0f && a.GetComponent<AtomBehavior>().availableValence > 0 && a.GetComponent<AtomBehavior>().valence < 8 && availableValence > 0 && valence < 8) {
				if (!a.GetComponent<AtomBehavior>().orbitVisible && !a.GetComponent<AtomBehavior>().growing) {
					a.GetComponent<AtomBehavior>().startRevealOrbit();
					a.GetComponent<AtomBehavior>().bondTargetOrder = 1;
					bondTargetOrder = 1;
					myOrbit.GetComponent<OrbitBehavior>().seek (a);
					bondTarget = a;
				}
				if (a.GetComponent<AtomBehavior>().orbitVisible) {
					if (Vector2.Distance ((Vector2)a.transform.position,(Vector2)transform.position) < 120.0f && a.GetComponent<AtomBehavior>().availableValence > 1 && a.GetComponent<AtomBehavior>().valence < 7 && availableValence > 1 && valence < 7) {
						if (Vector2.Distance ((Vector2)a.transform.position,(Vector2)transform.position) < 100.0f && a.GetComponent<AtomBehavior>().availableValence > 2 && a.GetComponent<AtomBehavior>().valence < 6 && availableValence > 2 && valence < 6) {
							//present triple bond option
							a.GetComponent<AtomBehavior>().bondTargetOrder = 3;
							bondTargetOrder = 3;
							if (a.GetComponentInChildren<OrbitBehavior>().mode != "homing") {
								a.GetComponent<AtomBehavior>().lookTo(this.gameObject);
							}
						} else {
							//present double bond option
							a.GetComponent<AtomBehavior>().bondTargetOrder = 2;
							bondTargetOrder = 2;
							if (a.GetComponentInChildren<OrbitBehavior>().mode != "homing") {
								a.GetComponent<AtomBehavior>().lookTo(this.gameObject);
							}
						}
					} else {
						//present single bond option
						a.GetComponent<AtomBehavior>().bondTargetOrder = 1;
						bondTargetOrder = 1;
						if (a.GetComponentInChildren<OrbitBehavior>().mode != "homing") {
							a.GetComponent<AtomBehavior>().lookTo(this.gameObject);
						}
						a.GetComponent<RageSpline>().fillColor1 = a.GetComponent<AtomBehavior>().myColor;
					}
				}

				searching = false;
				found = true;

			} else {
				if (a.gameObject == this.gameObject) {
					a.GetComponent<RageSpline>().fillColor1 = a.GetComponent<AtomBehavior>().myHighlight;
				} else {
					a.GetComponent<RageSpline>().fillColor1 = a.GetComponent<AtomBehavior>().myColor;
					if (a.GetComponent<AtomBehavior>().orbitVisible && !a.GetComponent<AtomBehavior>().shrinking) {
						a.GetComponent<AtomBehavior>().startHideOrbit();
					}
				}
			}
		}
		if (!found) {
			myOrbit.GetComponent<OrbitBehavior>().mode = "spin";
			GameObject[] electrons = GameObject.FindGameObjectsWithTag("electron");
			foreach (GameObject e in electrons) {
				if (e.transform.parent == myOrbit.transform) {
					e.GetComponent<RageSpline>().fillColor1 = e.GetComponent<ElectronBehavior>().myColor;
					e.GetComponent<RageSpline>().outlineColor1 = e.GetComponent<ElectronBehavior>().myOutline;
					e.GetComponent<RageSpline>().RefreshMesh(true,true,false);
				}
			}
			bondTarget = null;
		}

	}


	public void lookTo(GameObject target) {
		myOrbit.GetComponent<OrbitBehavior>().lookTo(target);
	}



	IEnumerator OptimizeAngles () {
		// First, get a list of the target's substituent objects.
		List<AtomBehavior> substituentAtoms = new List<AtomBehavior>();
		foreach (Transform s in bondTarget.GetComponent<AtomBehavior>().myBondAtoms) {
			substituentAtoms.Add(s.GetComponent<AtomBehavior>());
		}

		// If there is only one substituent atom, we need only adjust the atom the user is currently adding.
		if (substituentAtoms.Count == 1) {
			float myInitRadius = Vector3.Distance (transform.position,bondTarget.transform.position);
			float myTargetRadius = BondLength;
			Vector3 myInitDirection = Vector3.Normalize (transform.position - bondTarget.transform.position);
			Vector3 myTargetDirection = Vector3.Normalize (bondTarget.transform.position - substituentAtoms[0].transform.position);
			float myAngle = Vector3.Angle(myInitDirection,myTargetDirection);
			if (bondTarget.GetComponent<AtomBehavior>().element == "Oxygen") {myAngle -= (180-109);}
			bool turnright = Vector3.Cross(myTargetDirection, myInitDirection).z > 0;
			if (turnright) {
				myAngle = -myAngle;
			}
			float t = 0;
			while (t < 1.0) {
				t+=4*Time.deltaTime;
				float currentAngle = Mathf.LerpAngle(0,myAngle,Mathf.SmoothStep(0,1,t));
				float currentRadius = Mathf.Lerp(myInitRadius,myTargetRadius,Mathf.SmoothStep(0,1,t));
				Vector3 currentDirection = Quaternion.AngleAxis(currentAngle,Vector3.forward) * myInitDirection;
				transform.position = bondTarget.transform.position + (currentDirection*currentRadius);
				yield return 0;
			}
		}
		/* If, however, there are two or more substituents, a shift is necessary to accommodate
		 * the new atom. */
		if (substituentAtoms.Count > 1) {

			Transform myMolecule = substituentAtoms[0].transform.parent;

			// Generate parent transforms
			List<GameObject> parents = new List<GameObject>();
			for (int i=0; i<substituentAtoms.Count; i++) {
				GameObject parent_i = new GameObject();
				parent_i.transform.SetParent(myMolecule);
				parent_i.transform.position = bondTarget.transform.position;
				parent_i.name = "RotParent" + i.ToString();
				parents.Add (parent_i);
			}
			// Calculate vectors to delineate the partitions
			List<Vector2> subDirs = new List<Vector2>();
			List<Vector2> partitions = new List<Vector2>();
			float partition_angle = 360.0f / (substituentAtoms.Count * 2);
			foreach (AtomBehavior a in substituentAtoms) {
				Vector2 subDir_i = (Vector2)(a.transform.position - bondTarget.transform.position).normalized;
				subDirs.Add(subDir_i);
				Vector2 partition_i = (Vector2)(Quaternion.AngleAxis(partition_angle,Vector3.back) * subDir_i);
				partitions.Add(partition_i);
			}
			// Sort all objects into parents based on partitions
			for (int i=0; i<substituentAtoms.Count; i++) {
				foreach (AtomBehavior o in substituentAtoms) {
					if (o.gameObject != bondTarget && o.transform.parent == myMolecule) {
						Vector2 oDir = (Vector2)(o.transform.position - bondTarget.transform.position).normalized;
						float object_angle = Vector2.Angle(partitions[i],oDir);
						if (Vector3.Cross((Vector3)oDir,(Vector3)partitions[i]).z <= 0) {object_angle = -object_angle;}
						if (object_angle > 0 && object_angle <= partition_angle*2) {
							o.transform.SetParent(parents[i].transform);
							// Recursively add the rest of each substituent chain, if it exists (including primary bonds)
							substituentChain = new List<GameObject>();
							if (o.tag == "atom") {buildSubstituentChain(o.gameObject);}
							foreach (GameObject g in substituentChain) {g.transform.SetParent(parents[i].transform);}
						}
					}
				}
			}
			// Find the vector of the longest chain, highest bond order, or heaviest atomic substituent
			int maxChainLength = 1;
			int priorityIndex = -1;
			for (int i=0; i<parents.Count; i++) {
				if (parents[i].GetComponentsInChildren<AtomBehavior>().Length > maxChainLength) {
					maxChainLength = parents[i].GetComponentsInChildren<AtomBehavior>().Length;
					for (int j=0; j<bondTarget.GetComponent<AtomBehavior>().myBondAtoms.Count;j++) {
						if (bondTarget.GetComponent<AtomBehavior>().myBondAtoms[j].parent == parents[i].transform) {
							priorityIndex = j;
						}
					}
				}
			}
			if (priorityIndex == -1) {
				int maxBondOrder = 1;
				for (int i=0; i<bondTarget.GetComponent<AtomBehavior>().myBondOrders.Count; i++) {
					if (bondTarget.GetComponent<AtomBehavior>().myBondOrders[i] > maxBondOrder) {
						maxBondOrder = bondTarget.GetComponent<AtomBehavior>().myBondOrders[i];
						priorityIndex = i;
					}
				}
				if (priorityIndex == -1) {
					int maxAtomWeight = 1;
					for (int i=0; i<bondTarget.GetComponent<AtomBehavior>().myBondAtoms.Count; i++) {
						if (bondTarget.GetComponent<AtomBehavior>().myBondAtoms[i].GetComponent<AtomBehavior>().weight > maxAtomWeight) {
							maxAtomWeight = bondTarget.GetComponent<AtomBehavior>().myBondAtoms[i].GetComponent<AtomBehavior>().weight;
							priorityIndex = i;
						}
					}
					if (priorityIndex == -1) {priorityIndex = 0;}
				}
			}
			Vector2 priorityDir = ((Vector2)(bondTarget.GetComponent<AtomBehavior>().myBondAtoms[priorityIndex].position - bondTarget.transform.position)).normalized;
			//Debug.DrawLine (bondTarget.transform.position,bondTarget.transform.position+((Vector3)priorityDir)*50,Color.green,100);


			// Determine a list of equidistant directions, starting with the vector determined above
			float priorityAngle = Vector2.Angle(Vector2.up,priorityDir);
			if (Vector3.Cross(Vector3.up,(Vector3)priorityDir).z < 0) {priorityAngle = -priorityAngle;}
			List<Vector2> newSubDirs = new List<Vector2>();
			for (int i=0; i<substituentAtoms.Count+1; i++) {
				newSubDirs.Add(((Vector2)(Quaternion.AngleAxis(i*(360f/(substituentAtoms.Count+1)),Vector3.forward) * priorityDir)).normalized);
				//Debug.DrawLine (bondTarget.transform.position,bondTarget.transform.position+((Vector3)newSubDirs[newSubDirs.Count-1]).normalized*50,Color.green,100);
				//print ((Vector3)newSubDirs[newSubDirs.Count-1]*50);
			}
			// Determine optimal angles for substituent movement
			List<float> parent_angles = new List<float>();
			for (int i=0; i<parents.Count; i++) {
				Vector2 substituent_i_dir = Vector2.zero;
				foreach (AtomBehavior a in substituentAtoms) {
					if (a.transform.IsChildOf(parents[i].transform)) {
						substituent_i_dir = ((Vector2)(a.transform.position - bondTarget.transform.position)).normalized;
					}
				}
				float bestAngle = 360f;
				int bestDirIndex = 0;
				for (int j=0; j<newSubDirs.Count; j++) {
					if (Vector2.Angle (substituent_i_dir,newSubDirs[j]) < bestAngle) {
						bestAngle = Vector2.Angle (substituent_i_dir,newSubDirs[j]);
						bestDirIndex = j;
					}
				}
				if (Vector3.Cross ((Vector3)newSubDirs[bestDirIndex],(Vector3)substituent_i_dir).z > 0) {bestAngle = -bestAngle;}
				parent_angles.Add(bestAngle);
				newSubDirs.RemoveAt(bestDirIndex);
			}
			// The new atom will be targeted to the direction that's left over from the above process
			Vector2 leftOver = newSubDirs[0];
			// Perform the animation for the new atom and the substituent groups, simultaneously
			float myInitRadius = Vector2.Distance ((Vector2)transform.position,(Vector2)bondTarget.transform.position);
			float myTargetRadius = BondLength;
			Vector2 myInitDirection = ((Vector2)(transform.position - bondTarget.transform.position)).normalized;
			Vector2 myTargetDirection = leftOver;
			float myAngle = Vector2.Angle(myInitDirection,myTargetDirection);
			bool turnright = Vector3.Cross((Vector3)myTargetDirection,(Vector3)myInitDirection).z > 0;
			if (turnright) {myAngle = -myAngle;}
			float t = 0;
			while (t < 1.0) {
				t+=4*Time.deltaTime;
				// Adjust the existing substituents
				for (int i=0; i<parents.Count; i++) {
					parents[i].transform.rotation = Quaternion.Lerp (Quaternion.identity,Quaternion.AngleAxis(parent_angles[i],Vector3.forward),t);
					foreach (Transform trans in parents[i].transform) {if (trans.GetComponent<AtomBehavior>() != null) {trans.rotation = Quaternion.identity;}}
				}
				// Adjust the new atom
				float currentAngle = Mathf.LerpAngle(0,myAngle,Mathf.SmoothStep(0,1,t));
				float currentRadius = Mathf.Lerp(myInitRadius,myTargetRadius,Mathf.SmoothStep(0,1,t));
				Vector3 currentDirection = Quaternion.AngleAxis(currentAngle,Vector3.forward) * myInitDirection;
				transform.position = bondTarget.transform.position + (currentDirection*currentRadius);
				yield return 0;
			}
			// Restore proper hierarchy, and destroy temporary parents
			foreach (GameObject o in GameObject.FindObjectsOfType<GameObject>()) {
				if (o.transform.parent != null && parents.Contains(o.transform.parent.gameObject)) {o.transform.SetParent(myMolecule);}
			}
			bondTarget.transform.SetParent (myMolecule);
			foreach (GameObject p in parents) {Destroy(p);}


		}

		rearranging = false;
		yield return 0;
	}

	void buildSubstituentChain (GameObject centerObject) {
		// Find all objects overlapping the target, add them to the List, and perpetuate
		foreach (GameObject g in GameObject.FindObjectsOfType<GameObject>()) {
			if (g.transform.parent != null && g.transform.parent.tag == "molecule" && g != bondTarget) {
				if (g.GetComponent<Collider>() != null && g.GetComponent<Collider>().bounds.Intersects(centerObject.GetComponent<Collider>().bounds)) {
					if (!substituentChain.Contains(g)) {
						substituentChain.Add (g);
						buildSubstituentChain(g);
					}
				}
			}
		}
	}


}
