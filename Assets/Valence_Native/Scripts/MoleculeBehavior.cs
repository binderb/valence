using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MoleculeBehavior : MonoBehaviour {
	
	public List<GameObject> myAtoms = new List<GameObject>();
	public List<GameObject> myBonds = new List<GameObject>();
	public List<string> myAtomTypes = new List<string>();
	private Vector3 initPos;
	private Vector3 mouseDownPosition;
	public Transform starShape;
	public GameObject controller;
	public Color myArchiveOutlineColor;
	public Color myArchiveOutlineHighlight;
	public string myMoleculeName = "";
	
	// Use this for initialization
	void Start () {
		controller = GameObject.Find("GameController");
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	
	/*void OnMouseDrag() {
		//if (!mouseLocked && !isMolecule) {
		Vector3 mouseDiff = new Vector3((int)(Input.mousePosition.x - mouseDownPosition.x),(int)(Input.mousePosition.y - mouseDownPosition.y),initPos.z);
		transform.position = initPos + mouseDiff;
		
		//checkNearbyAtoms();
		//}
	}*/
	
	public void startRevealOrbit() {
		GameObject[] allAtoms = GameObject.FindGameObjectsWithTag("atom");
		//List<GameObject> myAtoms = new List<GameObject>();
		for (int i=0;i<allAtoms.Length;i++) {
			if (allAtoms[i].transform.parent == transform) {
				//myAtoms.Add(electrons[i]);
				allAtoms[i].GetComponent<RageSpline>().fillColor1 = allAtoms[i].GetComponent<AtomBehavior>().myHighlight;
				allAtoms[i].GetComponent<RageSpline>().RefreshMesh(true,true,true);
			}
		}
		if (tag == "archived") {
			foreach (Transform o in transform) {
				if (o.GetComponent<RageSpline>() != null) {
					if (o.GetComponent<AtomBehavior>() != null) {
						o.GetComponent<RageSpline>().outlineColor1 = myArchiveOutlineHighlight;
					} else {
						o.GetComponent<RageSpline>().fillColor1 = myArchiveOutlineHighlight;
						o.GetComponent<RageSpline>().fillColor2 = myArchiveOutlineHighlight;
					}
				}
				o.GetComponent<RageSpline>().RefreshMesh(true,true,true);
			}
		}
	}
	
	public void startHideOrbit() {
		GameObject[] allAtoms = GameObject.FindGameObjectsWithTag("atom");
		//List<GameObject> myAtoms = new List<GameObject>();
		for (int i=0;i<allAtoms.Length;i++) {
			if (allAtoms[i].transform.parent == transform) {
				//myAtoms.Add(electrons[i]);
				allAtoms[i].GetComponent<RageSpline>().fillColor1 = allAtoms[i].GetComponent<AtomBehavior>().myColor;
				allAtoms[i].GetComponent<RageSpline>().RefreshMesh(true,true,true);
			}
		}
		if (tag == "archived") {
			foreach (Transform o in transform) {
				if (o.GetComponent<RageSpline>() != null) {
					if (o.GetComponent<AtomBehavior>() != null) {
						o.GetComponent<RageSpline>().outlineColor1 = myArchiveOutlineColor;
					} else {
						o.GetComponent<RageSpline>().fillColor1 = myArchiveOutlineColor;
						o.GetComponent<RageSpline>().fillColor2 = myArchiveOutlineColor;
					}
				}
				o.GetComponent<RageSpline>().RefreshMesh(true,true,true);
			}
		}
	}
	
	public void checkValence() {
		bool complete = true;
		foreach (Transform child in transform) {
			if (child.GetComponent<AtomBehavior>() != null) {
				if (child.GetComponent<AtomBehavior>().valence < 8 && child.GetComponent<AtomBehavior>().element != "Hydrogen") {complete = false;}
				else if (child.GetComponent<AtomBehavior>().valence < 2 && child.GetComponent<AtomBehavior>().element == "Hydrogen") {complete = false;}
			}
		}
		if (complete) {
			controller.GetComponent<Controller>().uniqueStructure = checkIdentity();
			controller.GetComponent<Controller>().endRound();	
		}
		
	}
	
	bool checkIdentity () {
		foreach (Transform child in transform) {
			if (child.GetComponent<AtomBehavior>() != null) {
				myAtomTypes.Add(getBondingType(child));
			}
		}
		myAtomTypes.Sort ();
		string myAtomTypeString = string.Join (",",myAtomTypes.ToArray());
		controller.GetComponent<Controller>().moleculeFormula = getMolecularFormula();
		Dictionary<string,string> moleculeDictionary = controller.GetComponent<AtomDictionary>().moleculeDictionary;
		Dictionary<string,string> codexDictionary = controller.GetComponent<AtomDictionary>().codexDictionary;
		List<string> lightsDictionary = controller.GetComponent<AtomDictionary>().lightsDictionary;
		if (moleculeDictionary.ContainsKey(myAtomTypeString)) {
			myMoleculeName = moleculeDictionary[myAtomTypeString];
			controller.GetComponent<Controller>().moleculeName = moleculeDictionary[myAtomTypeString];
			if (codexDictionary.ContainsKey(myMoleculeName) && !controller.GetComponent<Controller>().foundCodexEntries.Contains(lightsDictionary.IndexOf(myMoleculeName))) {
				controller.GetComponent<Controller>().foundCodexEntries.Add (lightsDictionary.IndexOf(myMoleculeName));
			}
		} else {
			controller.GetComponent<Controller>().moleculeName = "Unknown";
			print ("Compound not found in dictionary. Send this code to the programmer:\n"+myAtomTypeString);
			controller.GetComponent<Controller>().newCompounds.Add(myAtomTypeString);
		}
		if (controller.GetComponent<Controller>().builtMolecules.Contains(myAtomTypeString)) {
			return false;
		} else {
			controller.GetComponent<Controller>().builtMolecules.Add (myAtomTypeString);
			return true;
		}
		
	}
	
	string getBondingType (Transform atom) {
		List<string> myType = new List<string>();
		string myElement = atom.GetComponent<AtomBehavior>().element;
		List<string> myBondMates = atom.GetComponent<AtomBehavior>().myBondMates;
		List<int> myBondOrders = atom.GetComponent<AtomBehavior>().myBondOrders;
		for (int i=0;i<myBondMates.Count;i++) {
			string myBonds = "";
			switch(myElement) {
			case "Hydrogen": myBonds += "H"; break;
			case "Oxygen": myBonds += "O"; break;
			case "Nitrogen": myBonds += "N"; break;
			case "Carbon": myBonds += "C"; break;
			}
			switch(myBondMates[i]) {
			case "Hydrogen": myBonds += "H"; break;
			case "Oxygen": myBonds += "O"; break;
			case "Nitrogen": myBonds += "N"; break;
			case "Carbon": myBonds += "C"; break;
			}
			myBonds += myBondOrders[i].ToString();
			myType.Add (myBonds);
		}
		myType.Sort();
		
		string myTypeString = string.Join ("",myType.ToArray());
		return myTypeString;
		
	}

	public string getMolecularFormula () {
		int Ccount = 0;
		int Hcount = 0;
		int Ncount = 0;
		int Ocount = 0;
		foreach (GameObject a in myAtoms) {
			if (a.GetComponent<AtomBehavior>().element == "Carbon") {Ccount++;}
			if (a.GetComponent<AtomBehavior>().element == "Hydrogen") {Hcount++;}
			if (a.GetComponent<AtomBehavior>().element == "Nitrogen") {Ncount++;}
			if (a.GetComponent<AtomBehavior>().element == "Oxygen") {Ocount++;}
		}
		string formula = "";
		if (Ccount>0) {formula = formula + "C" + getSubscript(Ccount);}
		if (Hcount>0) {formula = formula + "H" + getSubscript(Hcount);}
		if (Ncount>0) {formula = formula + "N" + getSubscript(Ncount);}
		if (Ocount>0) {formula = formula + "O" + getSubscript(Ocount);}
		return formula;
	}

	string getCondensedFormula (string typeString) {
		return "";
	}

	string getSubscript (int number) {
		switch (number) {
		case 1: return "";
		case 2: return "\u2082";
		case 3: return "\u2083";
		case 4: return "\u2084";
		case 5: return "\u2085";
		case 6: return "\u2086";
		case 7: return "\u2087";
		case 8: return "\u2088";
		case 9: return "\u2089";
		case 10: return "\u2081\u2080";
		case 11: return "\u2081\u2081";
		case 12: return "\u2081\u2082";
		case 13: return "\u2081\u2083";
		case 14: return "\u2081\u2084";
		case 15: return "\u2081\u2085";
		case 16: return "\u2081\u2086";
		default: return "";
		}
	}
	
}

