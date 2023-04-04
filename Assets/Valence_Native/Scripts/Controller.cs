using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;

public class Controller : MonoBehaviour {

	public Sprite instr1;
	public Sprite instr2;
	public Sprite instr3;
	public Sprite instr4;
	public Sprite instr5;
	private string instrtxt1 = "Create molecules by dragging atoms together!\n\nRemember, molecules are formed when atoms share electrons, creating covalent bonds.\n\nDepending on the type of atoms involved, they can share 1, 2, or 3 pairs. Hold an atom further away and release to form a lower-order bond!";
	private string instrtxt2 = "Mouse-over an atom to see its name and current number of valence electrons. Atoms are stabilized when they fill their valence shells!\n\nMost common atoms have a shell capacity of 8. Hydrogen is the exception, with a capacity of 2.";
	private string instrtxt3 = "Atoms with completed valence shells won't bond anymore! When all atoms in a molecule have filled shells, the molecule is finished and the round ends.\n\nScore more points each round by using higher-order bonds and building larger molecules. When enough points are earned, the next level will be unlocked!\n\nOn higher levels, you have more atoms to work with, allowing you to build more complex structures!";
	private string instrtxt4 = "Hydrogen is kind of special; in organic systems, there tends to be a lot of it kicking around.\n\nDuring each round, you'll get 4 hydrogen atoms initially. To receive more, click on the H button in the lower right corner.\n\nNo more than 4 nonbonded hydrogens can be in play at once. Don't get greedy!";
	private string instrtxt5 = "Last but not least... creativity counts! You only receive points for structures you haven't already made in previous rounds!\n\nIf you form a compound of particular relevance to everyday life, you may unlock a Codex entry, which will tell you more about it.\n\nCheck your Database to review built molecules and receive clues on unlocking new Codex entries!";
	private int instruction_page = 0;
	public string previous_scene = "Title";
	public bool locked = false;
	public Transform Carbon;
	public Transform Nitrogen;
	public Transform Oxygen;
	public Transform Hydrogen;
	public bool uniqueStructure = false;
	public string moleculeName;
	public string moleculeFormula;
	public List<string> builtMolecules = new List<string>();
	public List<int> foundCodexEntries = new List<int>();
	public List<string> newCompounds = new List<string>();
	private int zStack = 0;
	public Text roundScoreText;
	public Text gameScoreText;
	public Text atomText;
	public Text valenceText;
	public int RoundNumber = 0;
	public int Level = 0;
	public int gameScore = 0;
	public int roundScore = 0;
	public Color scoreBoxColor;
	public Color scoreBoxAddColor;
	public Color scoreBoxSubtractColor;
	public Color lightColor;
	public Color lightOverColor;
	public Color lightLitColor;
	public Color lightLitOverColor;
	public Transform starShape;
	public Transform databaseMark;
	public Transform ring;
	private Coroutine ringCoroutine;
	public Color purpleUIColor;
	public Color archivedOutlineColor;
	public Dictionary<string,string> codexDictionary;
	public List<string> lightsDictionary;
	public static bool inputLock = false;



	// Use this for initialization
	void Start () {
		codexDictionary = GetComponent<AtomDictionary>().codexDictionary;
		lightsDictionary = GetComponent<AtomDictionary>().lightsDictionary;
	}

	// Update is called once per frame
	void Update () {

	}

	public void showTitle() {
		GameObject.Find("Main Camera").transform.position = new Vector3(480,300,-300);
		StartCoroutine(showTitleRoutine());
	}
	IEnumerator showTitleRoutine() {
		float t = 0;
		while (t < 1.0) {
			t+=3*Time.deltaTime;
			GameObject.Find("TitleCanvas").GetComponent<CanvasGroup>().alpha = Mathf.Lerp (0, 1, t);
			GameObject.Find("FadeBackdrop").GetComponent<SpriteRenderer>().color = Color.Lerp (new Color(1,1,1,1), new Color(1,1,1,0), t);
			GameObject.Find("TitleUI").transform.localPosition = Vector3.Lerp (Vector3.right*20,Vector3.zero,Mathf.SmoothStep(0.0f,1.0f,t));
			yield return 0;
		}
	}

	public void showInstructions() {
		instruction_page = 1;
		GameObject.Find("InstructionGraphic").GetComponent<SpriteRenderer>().sprite = instr1;
		GameObject.Find("InstructionText").GetComponent<Text>().text = instrtxt1;
		GameObject.Find("InstructionGraphic").GetComponent<SpriteRenderer>().color = new Color(1,1,1,0);
		GameObject.Find("InstructionCanvas").GetComponent<CanvasGroup>().alpha = 0;
		GameObject.Find("PreviousButton").GetComponent<MeshRenderer>().enabled = false;
		GameObject.Find("NextButton").GetComponent<MeshRenderer>().enabled = false;
		GameObject.Find("FinishButton").GetComponent<MeshRenderer>().enabled = false;
		GameObject.Find("PreviousText").GetComponent<Text>().enabled = false;
		StartCoroutine(showInstructionsRoutine());
	}
	IEnumerator showInstructionsRoutine() {
		GameObject.Find("NextButton").GetComponent<MeshRenderer>().enabled = true;
		GameObject.Find("NextButton").GetComponent<BoxCollider2D>().enabled = true;
		GameObject.Find("NextText").GetComponent<Text>().enabled = true;
		GameObject.Find("FinishButton").GetComponent<MeshRenderer>().enabled = true;
		GameObject.Find("Main Camera").transform.position = new Vector3(1440,300,-300);
		StartCoroutine(growFromZero(GameObject.Find("Panel").transform));
		StartCoroutine(growFromZero(GameObject.Find("PreviousAnchor").transform));
		StartCoroutine(growFromZero(GameObject.Find("NextAnchor").transform));
		StartCoroutine(growFromZero(GameObject.Find("FinishAnchor").transform));

		yield return new WaitForSeconds(0.1f);
		float t = 0;
		while (t < 1.0) {
			t+=4*Time.deltaTime;
			GameObject.Find("InstructionCanvas").GetComponent<CanvasGroup>().alpha = Mathf.Lerp (0, 1, t);
			GameObject.Find("InstructionGraphic").GetComponent<SpriteRenderer>().color = Color.Lerp (new Color(1,1,1,0), new Color(1,1,1,1), t);
			yield return 0;
		}
	}

	public void pageInstructions (int pageDirection) {
		instruction_page += pageDirection;
		switch (instruction_page) {
		case 1:
			GameObject.Find("InstructionText").GetComponent<Text>().text = instrtxt1;
			GameObject.Find("InstructionGraphic").GetComponent<SpriteRenderer>().sprite = instr1;
			GameObject.Find("PreviousButton").GetComponent<MeshRenderer>().enabled = false;
			GameObject.Find("PreviousButton").GetComponent<BoxCollider2D>().enabled = false;
			GameObject.Find("PreviousText").GetComponent<Text>().enabled = false;
			break;
		case 2:
			GameObject.Find("InstructionText").GetComponent<Text>().text = instrtxt2;
			GameObject.Find("InstructionGraphic").GetComponent<SpriteRenderer>().sprite = instr2;
			GameObject.Find("PreviousButton").GetComponent<MeshRenderer>().enabled = true;
			GameObject.Find("PreviousButton").GetComponent<BoxCollider2D>().enabled = true;
			GameObject.Find("PreviousText").GetComponent<Text>().enabled = true;
			break;
		case 3:
			GameObject.Find("InstructionText").GetComponent<Text>().text = instrtxt3;
			GameObject.Find("InstructionGraphic").GetComponent<SpriteRenderer>().sprite = instr3;
			break;
		case 4:
			GameObject.Find("InstructionText").GetComponent<Text>().text = instrtxt4;
			GameObject.Find("InstructionGraphic").GetComponent<SpriteRenderer>().sprite = instr4;
			GameObject.Find("NextButton").GetComponent<MeshRenderer>().enabled = true;
			GameObject.Find("NextButton").GetComponent<BoxCollider2D>().enabled = true;
			GameObject.Find("NextText").GetComponent<Text>().enabled = true;
			break;
		case 5:
			GameObject.Find("InstructionText").GetComponent<Text>().text = instrtxt5;
			GameObject.Find("InstructionGraphic").GetComponent<SpriteRenderer>().sprite = instr5;
			GameObject.Find("NextButton").GetComponent<MeshRenderer>().enabled = false;
			GameObject.Find("NextButton").GetComponent<BoxCollider2D>().enabled = false;
			GameObject.Find("NextText").GetComponent<Text>().enabled = false;
			break;
		}
	}

	public void showDatabase() {
		GameObject.Find ("Main Camera").transform.position = new Vector3(2400,300,-300);
		Camera camera2 = GameObject.Find ("Secondary Camera").GetComponent<Camera>();
		camera2.transform.position = new Vector3(2295,420,501);
		int builtCount = builtMolecules.Count;
		float myY = 180*(((builtCount-1)/4)-2);
		if (myY < 0) myY = 0;
		camera2.transform.position += Vector3.down*myY;
		camera2.enabled = true;
		checkDatabaseArrows();
		RageSpline[] lights = GameObject.Find("CodexRing").transform.GetComponentsInChildren<RageSpline>();
		for (int i=0; i<50; i++) {
			lights[i].fillColor1 = lightColor;
			lights[i].RefreshMesh(true,true,true);
		}
		foreach (int i in foundCodexEntries) {
			lights[i].fillColor1 = lightLitColor;
			lights[i].RefreshMesh(true,true,true);
		}
		GameObject.Find("CodexCompletionText").GetComponent<Text>().text = "Codex Entries:\n" + foundCodexEntries.Count + "/30";
		GameObject.Find("CodexClue").GetComponent<Text>().text = "Select an empty codex entry to receive a clue.";
	}

	public void checkDatabaseArrows() {
		Transform camera2 = GameObject.Find ("Secondary Camera").transform;
		Transform pageUp = GameObject.Find ("UpDatabase").transform;
		Transform pageDown = GameObject.Find ("DownDatabase").transform;
		if (camera2.position.y >= 420) {pageUp.position = new Vector3(2255f,536f,-99f);} else {pageUp.position = new Vector3(2255f,536f,-101f);}
		if (builtMolecules.Count > 12 && camera2.position.y-420f > (((builtMolecules.Count-1) / 4)-2)*-180 ) {
			pageDown.position = new Vector3(2255f,158f,-101f);
		} else {
			pageDown.position = new Vector3(2255f,158f,-99f);
		}

	}

	public void showCodex(MoleculeBehavior molecule, string mySource) {
		string moleculeName = molecule.myMoleculeName;
		string moleculeFormula = molecule.getMolecularFormula();
		Camera camera2 = GameObject.Find ("Secondary Camera").GetComponent<Camera>();
		camera2.enabled = false;
		GameObject.Find("CodexPanelSprite").GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>(moleculeName);
		GameObject.Find("CodexPanelStructure").GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>(moleculeName + "_structure");
		GameObject.Find ("DatabasePanel").transform.localPosition = new Vector3(335.5f,347.5f,-1000f);
		GameObject.Find ("CodexPanel").transform.localPosition = new Vector3(335.5f,304,-100f);
		GameObject.Find ("CodexTitle").GetComponent<Text>().text = moleculeName;
		GameObject.Find ("CodexFormula").GetComponent<Text>().text = "Chemical Formula: " + moleculeFormula;
		GameObject.Find ("CodexText").GetComponent<Text>().text = codexDictionary[moleculeName];
		if (mySource == "Database") {
			GameObject.Find ("CloseCodexButton").GetComponent<MeshRenderer>().enabled = true;
			GameObject.Find ("CloseCodexButton").GetComponent<BoxCollider2D>().enabled = true;
			GameObject.Find ("CloseCodex").GetComponent<Text>().enabled = true;
		} else if (mySource == "GameWindow") {
			GameObject.Find ("CloseCodexButton").GetComponent<MeshRenderer>().enabled = false;
			GameObject.Find ("CloseCodexButton").GetComponent<BoxCollider2D>().enabled = false;
			GameObject.Find ("CloseCodex").GetComponent<Text>().enabled = false;
		}

		GameObject.Find ("CodexCanvas").GetComponent<CanvasGroup>().alpha = 1;
	}

	public void resetDatabase () {
		GameObject.Find ("DatabasePanel").transform.localPosition = new Vector3(335.5f,347.5f,-100f);
		GameObject.Find ("CodexPanel").transform.localPosition = new Vector3(335.5f,304,-1000f);
		foreach (GameObject m in GameObject.FindGameObjectsWithTag("archived")) {
			foreach (Transform o in m.transform) {
				if (o.GetComponent<RageSpline>() != null) {
					if (o.GetComponent<AtomBehavior>() != null) {
						o.GetComponent<RageSpline>().outlineColor1 = archivedOutlineColor;
						o.GetComponent<RageSpline>().fillColor1 = o.GetComponent<AtomBehavior>().myColor;
					} else {
						o.GetComponent<RageSpline>().fillColor1 = archivedOutlineColor;
						o.GetComponent<RageSpline>().fillColor2 = archivedOutlineColor;
					}
				}
				o.GetComponent<RageSpline>().RefreshMesh(true,true,true);
			}
		}
	}

	public void startGame() {
		GameObject.Find("Main Camera").transform.position = new Vector3(-480,300,-300);
		StartCoroutine(startGameRoutine());
	}
	public void showGame() {
		GameObject.Find("Main Camera").transform.position = new Vector3(-480,300,-300);
		StartCoroutine (startGameRoutine ());
	}

	IEnumerator startGameRoutine() {

		if (previous_scene != "NewCodexEntry") {showGameHUD();}

		float t = 0;
		while (t < 1.0) {
			t+=4*Time.deltaTime;
			GameObject.Find("GameCanvas").GetComponent<CanvasGroup>().alpha = Mathf.Lerp (0, 1, t);
			if (previous_scene == "NewCodexEntry") {GameObject.Find("InfoCanvas").GetComponent<CanvasGroup>().alpha = Mathf.Lerp (0, 1, t);}
			GameObject.Find("GameCover").GetComponent<SpriteRenderer>().color = Color.Lerp (Color.white,new Color(1f,1f,1f,0f),t);
			yield return 0;
		}

		// If the player is starting a new game, display the "Level 1" splash before revealing elements.
		if (Level == 0) {
			Level++;
			Transform LevelTransform = GameObject.Find("Levels").transform;
			GameObject[] LevelTitle = GameObject.FindGameObjectsWithTag("leveltitle");
			RageSpline LevelNum = GameObject.Find("L" + Level).GetComponent<RageSpline>();
			t = 0;
			while (t < 1.0) {
				t+=3*Time.deltaTime;
				LevelTransform.position = Vector3.Lerp (new Vector3(-480,350,-5),new Vector3(-480,320,-5),Mathf.SmoothStep(0.0f,1.0f,t));
				foreach (GameObject l in LevelTitle) {
					l.GetComponent<RageSpline>().fillColor1 = Color.Lerp (new Color(1.0f,1.0f,1.0f,0.0f),lightLitColor,t);
					l.GetComponent<RageSpline>().RefreshMesh(true,true,true);
				}
				LevelNum.fillColor1 = Color.Lerp (new Color(1.0f,1.0f,1.0f,0.0f),lightLitColor,t);
				LevelNum.RefreshMesh(true,true,true);
				yield return 0;
			}
			yield return new WaitForSeconds(0.5f);
			t = 0;
			while (t < 1.0) {
				t+=2*Time.deltaTime;
				foreach (GameObject l in LevelTitle) {
					l.GetComponent<RageSpline>().fillColor1 = Color.Lerp (lightLitColor,new Color(1.0f,1.0f,1.0f,0.0f),t);
					l.GetComponent<RageSpline>().RefreshMesh(true,true,true);
				}
				LevelNum.fillColor1 = Color.Lerp (lightLitColor,new Color(1.0f,1.0f,1.0f,0.0f),t);
				LevelNum.RefreshMesh(true,true,true);

				yield return 0;
			}
			spawnAtoms ();
		}
	}


	public void spawnAtoms() {

		//first, spawn non-hydrogens. Decide how many to make based on score.
		int nonHydrogens = 0;
		if (gameScore < 500) nonHydrogens = 2;
		else if (gameScore >= 500 && gameScore < 1500) nonHydrogens = 3;
		else if (gameScore >= 1500 && gameScore < 3000) nonHydrogens = 4;
		else if (gameScore >= 3000 && gameScore < 5000) nonHydrogens = 5;
		else if (gameScore >= 5000) nonHydrogens = 6;

		int CarbonCount = 0;
		int NitrogenCount = 0;
		int OxygenCount = 0;

		for (int i=0;i<nonHydrogens;i++) {
			Transform newAtom = null;
			zStack += 2;
			Vector3 endPos = new Vector3((int)(Random.value*900+30)-960,(int)(Random.value*280+170),-1*zStack);
			Vector3 fromCenter = Vector3.Normalize(new Vector3((Screen.width/2)-endPos.x-960,(Screen.height/2)-endPos.y,0f));
			Vector3 startPos = endPos - fromCenter*500;

			bool atomPicked = false;
			while (!atomPicked) {
				float rand = Random.value;
				if (rand < 0.33333 && CarbonCount < 3) {
					atomPicked = true;
					newAtom = (Transform)Instantiate (Carbon);
					CarbonCount++;
					newAtom.GetComponent<AtomBehavior>().valence = 4;
					newAtom.GetComponent<AtomBehavior>().availableValence = 4;
					newAtom.GetComponent<AtomBehavior>().element = "Carbon";
				} else if (rand >= 0.33333 && rand < 0.66667 && NitrogenCount < 2) {
					atomPicked = true;
					NitrogenCount++;
					atomPicked = true;
					newAtom = (Transform)Instantiate (Nitrogen);
					newAtom.GetComponent<AtomBehavior>().valence = 5;
					newAtom.GetComponent<AtomBehavior>().availableValence = 5;
					newAtom.GetComponent<AtomBehavior>().element = "Nitrogen";
				} else if (rand >= 0.66667 && ((gameScore < 3000 && OxygenCount < 2) || (gameScore >= 3000 && OxygenCount < 3)) ) {
					atomPicked = true;
					newAtom = (Transform)Instantiate (Oxygen);
					OxygenCount++;
					newAtom.GetComponent<AtomBehavior>().valence = 6;
					newAtom.GetComponent<AtomBehavior>().availableValence = 6;
					newAtom.GetComponent<AtomBehavior>().element = "Oxygen";
				}
			}
			newAtom.position = startPos;
			StartCoroutine(enterAtom (newAtom,endPos));
		}


		int hydrogens = 4; //4*CarbonCount + 3*NitrogenCount + 2*OxygenCount;
		for (int i=0;i<hydrogens;i++) {
			spawnHydrogen();
		}
	}

	public void spawnHydrogen() {
		zStack += 2;
		Transform newAtom = (Transform)Instantiate (Hydrogen);
		Vector3 endPos = new Vector3((int)(Random.value*900+30)-960,(int)(Random.value*280+170),-1*zStack);
		Vector3 fromCenter = Vector3.Normalize(new Vector3((Screen.width/2)-endPos.x-960,(Screen.height/2)-endPos.y,0f));
		Vector3 startPos = endPos - fromCenter*500;
		newAtom.position = startPos;

		newAtom.GetComponent<AtomBehavior>().valence = 1;
		newAtom.GetComponent<AtomBehavior>().availableValence = 1;
		newAtom.GetComponent<AtomBehavior>().element = "Hydrogen";
		StartCoroutine(enterAtom (newAtom,endPos));
	}

	IEnumerator enterAtom(Transform atom, Vector3 finalPos) {
		Vector3 initPos = atom.position;
		float t = 0;
		while (t < 1.0) {
			t+=2*Time.deltaTime;
			atom.position = Vector3.Lerp (initPos, finalPos,Mathf.SmoothStep(0.0f,1.0f,t));
			yield return 0;
		}
	}

	public void destroyAll () {
		foreach (GameObject a in GameObject.FindGameObjectsWithTag("atom")) {Destroy (a);}
		foreach (GameObject b in GameObject.FindGameObjectsWithTag("bond")) {Destroy (b);}
		foreach (GameObject m in GameObject.FindGameObjectsWithTag("molecule")) {Destroy (m);}
		zStack = 0;
	}

	public void resetBonds() {
		GameObject moleculeObject = GameObject.FindGameObjectWithTag("molecule");
		if (moleculeObject != null) {
			Transform molecule = moleculeObject.transform;
			foreach (Transform a in molecule) {
				if (a.tag == "atom") {
					a.GetComponent<AtomBehavior>().isMolecule = false;
					a.GetComponent<AtomBehavior>().myBondMates = new List<string>();
					a.GetComponent<AtomBehavior>().myBondOrders = new List<int>();
					a.GetComponent<AtomBehavior>().myBondAtoms = new List<Transform>();
					if (a.name == "Hydrogen(Clone)") {
						a.GetComponent<AtomBehavior>().valence = 1;
						a.GetComponent<AtomBehavior>().availableValence = 1;
					} else if (a.name == "Carbon(Clone)") {
						a.GetComponent<AtomBehavior>().valence = 4;
						a.GetComponent<AtomBehavior>().availableValence = 4;
					} else if (a.name == "Nitrogen(Clone)") {
						a.GetComponent<AtomBehavior>().valence = 5;
						a.GetComponent<AtomBehavior>().availableValence = 5;
					} else if (a.name == "Oxygen(Clone)") {
						a.GetComponent<AtomBehavior>().valence = 6;
						a.GetComponent<AtomBehavior>().availableValence = 6;
					}
				} else if (a.tag == "bond") {
					StartCoroutine (shrinkAndDestroy(a.transform));
				}
			}
			molecule.DetachChildren();
			Destroy (molecule.gameObject);
		}
		addScore (-roundScore);
	}

	public void showFlag(Vector3 pos, int score) {
		Transform flag = GameObject.Find ("FlagText").transform;
		CanvasGroup myCanvas = GameObject.Find ("ScoreFlagCanvas").GetComponent<CanvasGroup>();
		flag.position = pos+Vector3.back*100f;
		flag.GetComponent<Text>().text = "+" + score;
		myCanvas.GetComponent<CanvasGroup>().alpha = 1;
		StartCoroutine (animateFlagRoutine(flag,myCanvas));
	}

	IEnumerator animateFlagRoutine(Transform flag, CanvasGroup myCanvas) {
		Vector3 initPos = flag.position;
		float t = 0;
		while (t < 1.0) {
			t+=2*Time.deltaTime;
			flag.position = Vector3.Lerp (initPos, initPos+Vector3.up*30,Mathf.SmoothStep(0.0f,1.0f,t));
			myCanvas.alpha = Mathf.Lerp (1,0,Mathf.SmoothStep(0.0f,1.0f,t));
			yield return 0;
		}
	}

	public void addScore(int score) {
		roundScore += score;
		roundScoreText.text = roundScore.ToString ();
		StartCoroutine(pulseRound());
	}

	IEnumerator pulseRound() {
		Transform roundBox = GameObject.Find ("RoundScoreBox").transform;
		Vector3 popScale = new Vector3(1.1f,1.1f,1.0f);
		roundBox.localScale = popScale;
		float t = 0;
		while (t < 1.0) {
			t+=3*Time.deltaTime;
			roundBox.localScale = Vector3.Lerp (popScale, Vector3.one,Mathf.SmoothStep(0.0f,1.0f,t));
			roundBox.GetComponent<RageSpline>().fillColor1 = Color.Lerp (scoreBoxAddColor, scoreBoxColor, Mathf.SmoothStep(0.0f,1.0f,t));
			roundBox.GetComponent<RageSpline>().RefreshMesh(true,true,true);
			yield return 0;
		}
	}

	public void showGameHUD () {
		StartCoroutine (showGameHUDRoutine());
	}

	IEnumerator showGameHUDRoutine () {
		Vector3 upPos = new Vector3(0,170,0);
		Vector3 downPos = Vector3.zero;
		Vector3 upBarPos = new Vector3(-480,90,400);
		Vector3 downBarPos = new Vector3(-480,-80,400);
		Vector3 gameScorePos = gameScoreText.transform.position;
		Vector3 roundScorePos = roundScoreText.transform.position;
		Vector3 atomTextPos = atomText.transform.position;
		Vector3 valenceTextPos = valenceText.transform.position;
		GameObject.Find("AtomText").GetComponent<Text>().text = "Atom:";
		GameObject.Find("ValenceText").GetComponent<Text>().text = "Valence:";
		float t = 0;
		while (t < 1.0) {
			t+=3*Time.deltaTime;
			GameObject.Find ("GameHUD").transform.position = Vector3.Lerp (upPos, downPos,Mathf.SmoothStep(0.0f,1.0f,t));
			gameScoreText.transform.position = Vector3.Lerp (gameScorePos,gameScorePos-upPos,Mathf.SmoothStep(0.0f,1.0f,t));
			roundScoreText.transform.position = Vector3.Lerp (roundScorePos,roundScorePos-upPos,Mathf.SmoothStep(0.0f,1.0f,t));
			atomText.transform.position = Vector3.Lerp (atomTextPos,atomTextPos-upPos,Mathf.SmoothStep(0.0f,1.0f,t));
			valenceText.transform.position = Vector3.Lerp (valenceTextPos,valenceTextPos-upPos,Mathf.SmoothStep(0.0f,1.0f,t));

			GameObject.Find ("Bar").transform.position = Vector3.Lerp (downBarPos, upBarPos,Mathf.SmoothStep(0.0f,1.0f,t));


			yield return 0;
		}
	}

	IEnumerator showGameBarRoutine () {
		Vector3 upPos = new Vector3(0,170,0);
		Vector3 upBarPos = new Vector3(-480,90,400);
		Vector3 downBarPos = new Vector3(-480,-80,400);
		Vector3 infoBoxPos = GameObject.Find ("AtomInfoBox").transform.position;
		float t = 0;
		while (t < 1.0) {
			t+=3*Time.deltaTime;
			GameObject.Find ("AtomInfoBox").transform.position = Vector3.Lerp (infoBoxPos, infoBoxPos-upPos,Mathf.SmoothStep(0.0f,1.0f,t));
			atomText.color = Color.Lerp (new Color(1.0f,1.0f,1.0f,0.0f),new Color(1.0f,1.0f,1.0f,1.0f),Mathf.SmoothStep(0.0f,1.0f,t));
			valenceText.color = Color.Lerp (new Color(1.0f,1.0f,1.0f,0.0f),new Color(1.0f,1.0f,1.0f,1.0f),Mathf.SmoothStep(0.0f,1.0f,t));
			GameObject.Find ("Bar").transform.position = Vector3.Lerp (downBarPos, upBarPos,Mathf.SmoothStep(0.0f,1.0f,t));


			yield return 0;
		}
	}

	public void hideGameHUD () {
		StartCoroutine (hideGameHUDRoutine());
	}

	IEnumerator hideGameHUDRoutine () {
		Vector3 upPos = new Vector3(0,170,0);
		Vector3 downPos = Vector3.zero;
		Vector3 upBarPos = new Vector3(-480,90,400);
		Vector3 downBarPos = new Vector3(-480,-80,400);
		Vector3 gameScorePos = gameScoreText.transform.position;
		Vector3 roundScorePos = roundScoreText.transform.position;
		Vector3 atomTextPos = atomText.transform.position;
		Vector3 valenceTextPos = valenceText.transform.position;
		float t = 0;
		while (t < 1.0) {
			t+=3*Time.deltaTime;
			GameObject.Find ("GameHUD").transform.position = Vector3.Lerp (downPos, upPos,Mathf.SmoothStep(0.0f,1.0f,t));
			gameScoreText.transform.position = Vector3.Lerp (gameScorePos,gameScorePos+upPos,Mathf.SmoothStep(0.0f,1.0f,t));
			roundScoreText.transform.position = Vector3.Lerp (roundScorePos,roundScorePos+upPos,Mathf.SmoothStep(0.0f,1.0f,t));
			atomText.transform.position = Vector3.Lerp (atomTextPos,atomTextPos+upPos,Mathf.SmoothStep(0.0f,1.0f,t));
			valenceText.transform.position = Vector3.Lerp (valenceTextPos,valenceTextPos+upPos,Mathf.SmoothStep(0.0f,1.0f,t));
			GameObject.Find ("Bar").transform.position = Vector3.Lerp (upBarPos, downBarPos,Mathf.SmoothStep(0.0f,1.0f,t));


			yield return 0;
		}
	}

	IEnumerator hideGameBarRoutine () {
		Vector3 upPos = new Vector3(0,170,0);
		Vector3 upBarPos = new Vector3(-480,90,400);
		Vector3 downBarPos = new Vector3(-480,-80,400);
		Vector3 infoBoxPos = GameObject.Find ("AtomInfoBox").transform.position;
		float t = 0;
		while (t < 1.0) {
			t+=3*Time.deltaTime;
			GameObject.Find ("AtomInfoBox").transform.position = Vector3.Lerp (infoBoxPos, infoBoxPos+upPos,Mathf.SmoothStep(0.0f,1.0f,t));
			atomText.color = Color.Lerp (new Color(1.0f,1.0f,1.0f,1.0f),new Color(1.0f,1.0f,1.0f,0.0f),Mathf.SmoothStep(0.0f,1.0f,t));
			valenceText.color = Color.Lerp (new Color(1.0f,1.0f,1.0f,1.0f),new Color(1.0f,1.0f,1.0f,0.0f),Mathf.SmoothStep(0.0f,1.0f,t));
			GameObject.Find ("Bar").transform.position = Vector3.Lerp (upBarPos, downBarPos,Mathf.SmoothStep(0.0f,1.0f,t));


			yield return 0;
		}
	}

	public void endRound () {
		StartCoroutine (endRoundRoutine());
	}

	IEnumerator endRoundRoutine () {
		locked = true;
		StartCoroutine (hideGameBarRoutine());

		// Send all unbonded atoms away
		GameObject[] atoms = GameObject.FindGameObjectsWithTag("atom");
		foreach (GameObject a in atoms) {
			if (a.transform.parent == null) {
				Vector3 fromCenter = Vector3.Normalize(new Vector3(-480-a.transform.position.x,300-a.transform.position.y,0f));
				Vector3 endPos = a.transform.position - fromCenter*550;
				sendAtom(a.transform,endPos);
			}
		}
		Vector3 worldCenter = new Vector3(-480,300,0);
		GameObject theMolecule = GameObject.FindGameObjectWithTag("molecule");
		Vector3 initPos = theMolecule.transform.position;
		float t = 0;
		while (t < 1.0) {
			t+=2*Time.deltaTime;
			theMolecule.transform.position = Vector3.Slerp (initPos, worldCenter,Mathf.SmoothStep(0.0f,1.0f,t));
			yield return 0;
		}

		// If the structure hasn't been made before, proceed with success animations.
		if (uniqueStructure) {
			//ringCoroutine =	StartCoroutine (HoverRing());
			Transform innerStar = (Transform)Instantiate (starShape);
			Transform outerStar = (Transform)Instantiate (starShape);
			innerStar.position = new Vector3(worldCenter.x, worldCenter.y, 450f);
			outerStar.position = new Vector3(worldCenter.x,worldCenter.y, 451f);
			innerStar.GetComponent<StarSpin>().rotDirection = 1;
			outerStar.GetComponent<StarSpin>().rotDirection = -1;
			innerStar.localScale = Vector3.zero;
			outerStar.localScale = Vector3.zero;
			t = 0;
			while (t < 1.0) {
				t+=5*Time.deltaTime;
				innerStar.localScale = Vector3.Lerp (Vector3.zero,Vector3.one*1.5f,t);
				outerStar.localScale = Vector3.Lerp (Vector3.zero,Vector3.one*1.2f,t);
				yield return 0;
			}
			if (moleculeName != "Unknown") {
				GameObject.Find ("CompoundName").GetComponent<Text>().text = moleculeName;
				GameObject.Find ("Formula").GetComponent<Text>().text = moleculeFormula;
			} else {
				GameObject.Find ("CompoundName").GetComponent<Text>().text = moleculeFormula;
				GameObject.Find ("Formula").GetComponent<Text>().text = "";
			}
			GameObject.Find ("YouBuilt").GetComponent<Text>().color = Color.white;
			GameObject.Find ("CompoundName").GetComponent<Text>().color = Color.white;
			GameObject.Find ("Formula").GetComponent<Text>().color = Color.white;
			StartCoroutine (growFromZero (GameObject.Find ("YouBuilt").transform));
			StartCoroutine (growFromZero (GameObject.Find ("CompoundName").transform));
			StartCoroutine (growFromZero (GameObject.Find ("Formula").transform));
			yield return new WaitForSeconds(0.1f);



			// If the molecule happens to have a Codex entry, proceed with a custom animation sequence.
			if (codexDictionary.ContainsKey(moleculeName)) {
				GameObject.Find("DatabaseInfoText").GetComponent<Text>().text = codexDictionary[moleculeName].Split('.')[0].Replace("\n"," ") + ".";


				t = 0;
				Vector3 youBuiltInit = GameObject.Find ("YouBuilt").transform.position;
				Vector3 compoundInit = GameObject.Find ("CompoundName").transform.position;
				Vector3 formulaInit = GameObject.Find ("Formula").transform.position;
				Vector3 moleculeInit = theMolecule.transform.position;
				while (t < 1.0) {
					t+=1.2f*Time.deltaTime;
					innerStar.position = Vector3.Lerp (worldCenter+Vector3.forward*450f,worldCenter+Vector3.left*250f+Vector3.forward*450f,Mathf.SmoothStep(0.0f,1.0f,t));
					outerStar.position = Vector3.Lerp (worldCenter+Vector3.forward*451f,worldCenter+Vector3.left*250f+Vector3.forward*451f,Mathf.SmoothStep(0.0f,1.0f,t));
					theMolecule.transform.position = Vector3.Lerp (moleculeInit,moleculeInit+Vector3.left*250f,Mathf.SmoothStep(0.0f,1.0f,t));
					GameObject.Find ("YouBuilt").transform.position = Vector3.Lerp (youBuiltInit,youBuiltInit+Vector3.left*250f,Mathf.SmoothStep(0.0f,1.0f,t));
					GameObject.Find ("CompoundName").transform.position = Vector3.Lerp (compoundInit,compoundInit+Vector3.left*250f,Mathf.SmoothStep(0.0f,1.0f,t));
					GameObject.Find ("Formula").transform.position = Vector3.Lerp (formulaInit,formulaInit+Vector3.left*250f,Mathf.SmoothStep(0.0f,1.0f,t));
					yield return 0;
				}
				t = 0;
				Transform icon = GameObject.Find ("DatabaseIcon").transform;
				Transform flash = GameObject.Find ("DatabaseFlash").transform;
				GameObject.Find ("DatabaseInfoTitle").GetComponent<Text>().color = Color.white;
				StartCoroutine (growFromZero(GameObject.Find ("DatabaseInfoTitle").transform));
				while (t < 1.0) {
					t+=3f*Time.deltaTime;
					icon.localScale = Vector3.Lerp (new Vector3(2f,2f,1f),new Vector3(1.5f,1.5f,1f),t);
					foreach (Transform p in icon) {
						p.GetComponent<RageSpline>().fillColor1 = Color.Lerp (new Color(1f,1f,1f,0f),Color.white,t);
						p.GetComponent<RageSpline>().RefreshMesh(true,true,true);
					}
					yield return 0;
				}
				t = 0;
				while (t < 1.0) {
					t+=3f*Time.deltaTime;
					flash.localScale = Vector3.Lerp (new Vector3(1.5f,1.5f,1f),new Vector3(2.5f,2.5f,1f),t);
					foreach (Transform p in flash) {
						p.GetComponent<RageSpline>().fillColor1 = Color.Lerp (Color.white,new Color(1f,1f,1f,0f),t);
						p.GetComponent<RageSpline>().RefreshMesh(true,true,true);
					}
					yield return 0;
				}
				t = 0;
				GameObject.Find("NextRoundButton").GetComponent<Renderer>().enabled = true;
				GameObject.Find("ReadMoreButton").GetComponent<Renderer>().enabled = true;
				GameObject.Find ("NextRoundButton").GetComponent<BoxCollider2D>().enabled = true;
				GameObject.Find ("ReadMoreButton").GetComponent<BoxCollider2D>().enabled = true;
				GameObject.Find("NextRoundLabel").GetComponent<Text>().enabled = true;
				GameObject.Find("ReadMoreLabel").GetComponent<Text>().enabled = true;
				GameObject.Find("DatabaseInfoSprite").GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>(moleculeName);
				GameObject.Find("DatabaseInfoSprite").GetComponent<SpriteRenderer>().enabled = true;
				StartCoroutine (growFromZero(GameObject.Find("NextRoundButton").transform));
				StartCoroutine (growFromZero(GameObject.Find("ReadMoreButton").transform));
				StartCoroutine (growFromZero(GameObject.Find("NextRoundLabel").transform));
				StartCoroutine (growFromZero(GameObject.Find("ReadMoreLabel").transform));
				while (t < 1.0) {
					t+=3f*Time.deltaTime;
					GameObject.Find("DatabaseInfoText").GetComponent<Text>().color = Color.Lerp (new Color(1f,1f,1f,0f),Color.white,t);
					GameObject.Find("DatabaseInfoSprite").GetComponent<SpriteRenderer>().color = Color.Lerp (new Color(1f,1f,1f,0f),Color.white,t);
					GameObject.Find("DatabaseInfoSprite").transform.localPosition = Vector3.Lerp (new Vector3(111,-34,-50),new Vector3(121,-34,-50),t);
					yield return 0;
				}
			} else {

				// If the molecule does not have a Codex entry, proceed with a simpler animation sequence.
				t = 0;
				GameObject.Find("NextRoundButton").GetComponent<Renderer>().enabled = true;
				GameObject.Find ("NextRoundButton").GetComponent<BoxCollider2D>().enabled = true;
				GameObject.Find("NextRoundLabel").GetComponent<Text>().enabled = true;
				StartCoroutine (growFromZero(GameObject.Find("NextRoundButton").transform));
				StartCoroutine (growFromZero(GameObject.Find("NextRoundLabel").transform));

			}

		} else {

			// If the molecule has been made before, proceed with the 'Used' animations.
			t = 0;
			Color clearColor = new Color(1.0f,0.0f,0.0f,0.0f);
			Vector3 popScale = new Vector3(1.5f,1.5f,1.0f);
			Transform usedStamp = GameObject.Find ("Used").transform;
			while (t < 1.0) {
				t+=5*Time.deltaTime;
				foreach (Transform p in usedStamp) {
					if (p.GetComponent<RageSpline>() != null) {
						p.GetComponent<RageSpline>().fillColor1 = Color.Lerp (clearColor,Color.red,t);
						p.GetComponent<RageSpline>().outlineColor1 = Color.Lerp (clearColor,Color.red,t);
						p.GetComponent<RageSpline>().RefreshMesh (true,true,true);
					}
				}
				usedStamp.localScale = Vector3.Lerp (popScale,Vector3.one,t);
				yield return 0;
			}
			Transform roundScoreBox = GameObject.Find ("RoundScoreBox").transform;
			Vector3 beginPos = roundScoreBox.localPosition;
			roundScoreBox.GetComponent<RageSpline>().fillColor1 = scoreBoxSubtractColor;
			roundScoreBox.GetComponent<RageSpline>().RefreshMesh(true,true,true);
			roundScoreText.text = "0";
			roundScore = 0;
			roundScoreBox.Translate(Random.insideUnitCircle.normalized*8);
			yield return new WaitForSeconds(0.02f);
			roundScoreBox.localPosition = beginPos;
			roundScoreBox.Translate(Random.insideUnitCircle.normalized*8);
			yield return new WaitForSeconds(0.02f);
			roundScoreBox.localPosition = beginPos;
			roundScoreBox.Translate(Random.insideUnitCircle.normalized*8);
			yield return new WaitForSeconds(0.02f);
			roundScoreBox.localPosition = beginPos;
			roundScoreBox.Translate(Random.insideUnitCircle.normalized*8);
			yield return new WaitForSeconds(0.02f);
			roundScoreBox.GetComponent<RageSpline>().fillColor1 = scoreBoxColor;
			roundScoreBox.GetComponent<RageSpline>().RefreshMesh(true,true,true);
			roundScoreBox.localPosition = beginPos;
			GameObject.Find("NextRoundButton").GetComponent<Renderer>().enabled = true;
			GameObject.Find ("NextRoundButton").GetComponent<BoxCollider2D>().enabled = true;
			GameObject.Find("NextRoundLabel").GetComponent<Text>().enabled = true;
			StartCoroutine (growFromZero(GameObject.Find("NextRoundButton").transform));
			StartCoroutine (growFromZero(GameObject.Find("NextRoundLabel").transform));

		}

	}

	public void nextRound () {
		StartCoroutine (nextRoundRoutine());
	}

	IEnumerator nextRoundRoutine() {
		bool levelUp = false;
		switch (Level) {
			case 1: if (gameScore < 500 && gameScore+roundScore >= 500) {levelUp = true; Level++;} break;
			case 2: if (gameScore < 1500 && gameScore+roundScore >= 1500) {levelUp = true; Level++;} break;
			case 3: if (gameScore < 3000 && gameScore+roundScore >= 3000) {levelUp = true; Level++;} break;
			case 4: if (gameScore < 5000 && gameScore+roundScore >= 5000) {levelUp = true; Level++;} break;
		}

		foreach (GameObject a in GameObject.FindGameObjectsWithTag("atom")) {
			if (a.transform.parent == null) {Destroy (a);}
		}

		/*if (!uniqueStructure) {
			Color clearColor = new Color(1.0f,1.0f,1.0f,0.0f);
			usedStamp.GetComponent<Renderer>().enabled = true;
			float t=0;
			while (t < 1.0) {
				t+=5*Time.deltaTime;
				usedStamp.GetComponent<tk2dSprite>().color = Color.Lerp (Color.red,clearColor,t);
				yield return 0;
			}
		}*/

		Vector3 initCanvasPos = new Vector3(-960+480,300,0);
		if (uniqueStructure) {

			// If the molecule has not been made before, there are many more elements in play.
			yield return new WaitForSeconds(0.1f);
			StartCoroutine (emptyScoreRoutine());
			StartCoroutine (shrinkToZero(GameObject.Find ("YouBuilt").transform));
			StartCoroutine (shrinkToZero(GameObject.Find ("CompoundName").transform));
			StartCoroutine (shrinkToZero(GameObject.Find ("Formula").transform));
			//StopCoroutine(ringCoroutine);
			Transform icon = GameObject.Find ("DatabaseIcon").transform;
			float t = 0;
			while (t < 1.0) {
				t+=3f*Time.deltaTime;
				GameObject.Find("NextRoundButton").GetComponent<RageSpline>().outlineColor1 = Color.Lerp (purpleUIColor, new Color(purpleUIColor.r,purpleUIColor.g,purpleUIColor.b,0f), t);
				GameObject.Find("ReadMoreButton").GetComponent<RageSpline>().outlineColor1 = Color.Lerp (purpleUIColor, new Color(purpleUIColor.r,purpleUIColor.g,purpleUIColor.b,0f), t);
				GameObject.Find ("InfoCanvas").transform.position = Vector3.Lerp (initCanvasPos,initCanvasPos+Vector3.right*20,t);
				GameObject.Find ("InfoCanvas").GetComponent<CanvasGroup>().alpha = Mathf.Lerp (1,0,Mathf.SmoothStep (0f,1f,t));
				GameObject.Find("DatabaseInfoSprite").GetComponent<SpriteRenderer>().color = Color.Lerp (Color.white,new Color(1f,1f,1f,0f),Mathf.SmoothStep (0f,1f,t));
				if (codexDictionary.ContainsKey(moleculeName)) {
					foreach (Transform p in icon) {
						p.GetComponent<RageSpline>().fillColor1 = Color.Lerp (Color.white,new Color(1f,1f,1f,0f),t);
						p.GetComponent<RageSpline>().RefreshMesh(true,true,true);
					}
				}
				GameObject.Find ("NextRoundButton").GetComponent<RageSpline>().RefreshMesh(true,true,true);
				GameObject.Find ("ReadMoreButton").GetComponent<RageSpline>().RefreshMesh(true,true,true);
				yield return 0;
			}
			GameObject.Find ("DatabaseInfoSprite").GetComponent<SpriteRenderer>().enabled = false;
			GameObject.Find ("NextRoundButton").GetComponent<Renderer>().enabled = false;
			GameObject.Find ("ReadMoreButton").GetComponent<Renderer>().enabled = false;
			GameObject.Find ("NextRoundButton").GetComponent<BoxCollider2D>().enabled = false;
			GameObject.Find ("ReadMoreButton").GetComponent<BoxCollider2D>().enabled = false;
			foreach (RageSpline o in GameObject.FindGameObjectWithTag("molecule").transform.GetComponentsInChildren<RageSpline>()) {
				if (o != null) {
					o.fillColor1 = Color.white;
					o.fillColor2 = Color.white;
					o.outlineColor1 = Color.white;
					o.RefreshMesh(true,true,true);
				}
			}
			t = 0;
			GameObject[] stars = GameObject.FindGameObjectsWithTag("star");
			Vector3 star1scale = stars[0].transform.localScale;
			Vector3 star2scale = stars[1].transform.localScale;
			Color starColor = stars[0].GetComponent<RageSpline>().GetFillColor1();
			while (t < 1.0) {
				t+=2f*Time.deltaTime;
				foreach (RageSpline o in GameObject.FindGameObjectWithTag("molecule").transform.GetComponentsInChildren<RageSpline>()) {
					if (o != null) {
						o.fillColor1 = Color.Lerp (Color.white,new Color(1f,1f,1f,0f),Mathf.SmoothStep(0f,1f,t));
						o.fillColor2 = Color.Lerp (Color.white,new Color(1f,1f,1f,0f),Mathf.SmoothStep(0f,1f,t));
						o.outlineColor1 = Color.Lerp (Color.white,new Color(1f,1f,1f,0f),Mathf.SmoothStep(0f,1f,t));
						o.RefreshMesh(true,true,true);
					}
				}
				stars[0].transform.localScale = Vector3.Lerp (star1scale,star1scale*0.1f,Mathf.SmoothStep(0f,1f,t));
				stars[1].transform.localScale = Vector3.Lerp (star2scale,star2scale*0.1f,Mathf.SmoothStep(0f,1f,t));
				stars[0].GetComponent<RageSpline>().fillColor1 = Color.Lerp (starColor,Color.white,Mathf.SmoothStep(0f,1f,t));
				stars[1].GetComponent<RageSpline>().fillColor1 = Color.Lerp (starColor,Color.white,Mathf.SmoothStep(0f,1f,t));
				stars[0].GetComponent<RageSpline>().RefreshMesh(true,true,true);
				stars[1].GetComponent<RageSpline>().RefreshMesh(true,true,true);
				yield return 0;
			}
			storeMolecule(GameObject.FindGameObjectWithTag("molecule").transform);
			StartCoroutine (showGameBarRoutine());
			t = 0;
			Vector3 p1 = stars[0].transform.position;
			Vector3 p3 = GameObject.Find ("DatabaseBarButton").transform.position+Vector3.up*160; //new Vector3(-555f,90f,450f);
			Vector3 p2 = new Vector3((p1.x+p3.x)/2f,p1.y+30f,450f);
			while (t < 1.0) {
				t+=2f*Time.deltaTime;
				foreach (GameObject s in stars) {
					s.transform.position = LaGrangeVector(Mathf.SmoothStep(0f,1f,t),p1,p2,p3);
				}
				yield return 0;
			}
			Destroy (stars[0]);
			Destroy (stars[1]);
			Vector3 popScale = new Vector3(1.2f,1.2f,1.0f);
			GameObject dataButton = GameObject.Find ("DatabaseBarButton");
			t = 0;
			while (t < 1.0) {
				t+=2*Time.deltaTime;
				dataButton.transform.localScale = Vector3.Lerp (popScale, Vector3.one,Mathf.SmoothStep(0.0f,1.0f,t));
				dataButton.GetComponent<RageSpline>().fillColor1 = Color.Lerp (dataButton.GetComponent<DatabaseButton>().myHighlight, dataButton.GetComponent<DatabaseButton>().myColor, Mathf.SmoothStep(0.0f,1.0f,t));
				dataButton.GetComponent<RageSpline>().RefreshMesh(true,true,true);
				yield return 0;
			}
		} else {

			// If the molecule has been made before, proceed with a shorter cleanup sequence.

			float t = 0;
			while (t < 1.0) {
				t+=3f*Time.deltaTime;
				GameObject.Find("NextRoundButton").GetComponent<RageSpline>().outlineColor1 = Color.Lerp (purpleUIColor, new Color(purpleUIColor.r,purpleUIColor.g,purpleUIColor.b,0f), t);
				GameObject.Find ("InfoCanvas").transform.position = Vector3.Lerp (initCanvasPos,initCanvasPos+Vector3.right*20,t);
				GameObject.Find ("InfoCanvas").GetComponent<CanvasGroup>().alpha = Mathf.Lerp (1,0,Mathf.SmoothStep (0f,1f,t));
				GameObject.Find ("NextRoundButton").GetComponent<RageSpline>().RefreshMesh(true,true,true);
				yield return 0;
			}
			t = 0;
			GameObject.Find ("NextRoundButton").GetComponent<Renderer>().enabled = false;
			GameObject.Find ("NextRoundButton").GetComponent<BoxCollider2D>().enabled = false;
			Color clearColor = new Color(1.0f,0.0f,0.0f,0.0f);
			Transform usedStamp = GameObject.Find ("Used").transform;
			while (t < 1.0) {
				t+=5*Time.deltaTime;
				foreach (Transform p in usedStamp) {
					if (p.GetComponent<RageSpline>() != null) {
						p.GetComponent<RageSpline>().fillColor1 = Color.Lerp (Color.red,clearColor,t);
						p.GetComponent<RageSpline>().outlineColor1 = Color.Lerp (Color.red,clearColor,t);
						p.GetComponent<RageSpline>().RefreshMesh (true,true,true);
					}
				}
				yield return 0;
			}
			t = 0;
			Transform theMolecule = GameObject.FindGameObjectWithTag("molecule").transform;
			Vector3 p1 = theMolecule.position;
			Vector3 p3 = new Vector3(-1000f,0f,p1.z);
			Vector3 p2 = new Vector3((p1.x+p3.x)/2f,p1.y+50f,450f);
			while (t < 1.0) {
				t+=1f*Time.deltaTime;
				theMolecule.transform.position = LaGrangeVector(Mathf.SmoothStep(0f,1f,t),p1,p2,p3);
				theMolecule.Rotate(Vector3.forward,500*Time.deltaTime);
				yield return 0;
			}
			Destroy (theMolecule.gameObject);
			StartCoroutine (showGameBarRoutine());


		}


		// Animation is finished, so reset everything
		GameObject.Find ("InfoCanvas").transform.position = initCanvasPos;
		GameObject.Find ("InfoCanvas").GetComponent<CanvasGroup>().alpha = 1;
		GameObject.Find ("YouBuilt").transform.position = new Vector3(-480,150,-100);
		GameObject.Find ("CompoundName").transform.position = new Vector3(-480,108,-100);
		GameObject.Find ("Formula").transform.position = new Vector3(-480,65,-100);
		GameObject.Find ("NextRoundButton").GetComponent<RageSpline>().outlineColor1 = purpleUIColor;
		GameObject.Find ("ReadMoreButton").GetComponent<RageSpline>().outlineColor1 = purpleUIColor;
		GameObject.Find ("NextRoundButton").GetComponent<RageSpline>().RefreshMesh(true,true,true);
		GameObject.Find ("ReadMoreButton").GetComponent<RageSpline>().RefreshMesh(true,true,true);
		GameObject.Find ("NextRoundLabel").GetComponent<Text>().enabled = false;
		GameObject.Find ("ReadMoreLabel").GetComponent<Text>().enabled = false;
		GameObject.Find ("DatabaseInfoTitle").GetComponent<Text>().color = new Color(1f,1f,1f,0f);
		GameObject.Find ("DatabaseInfoText").GetComponent<Text>().color = new Color(1f,1f,1f,0f);
		zStack = 0;
		roundScore = 0;

		/*if (levelUp) {
			GameObject[] stars = GameObject.FindGameObjectsWithTag("star");
			foreach (GameObject s in stars) {
				GameObject.Destroy(s);
			}
			GameObject[] atoms = GameObject.FindGameObjectsWithTag("atom");
			foreach (GameObject a in atoms) {
				GameObject.Destroy (a);
			}
			GameObject.Destroy(GameObject.FindGameObjectWithTag("molecule"));

			Color clearColor = new Color(1.0f,1.0f,1.0f,0.0f);
			backdrop.color = clearColor;
			scoreBox.GetComponent<tk2dSprite>().color = clearColor;
			roundBox.GetComponent<tk2dSprite>().color = clearColor;
			valenceBox.GetComponent<tk2dSprite>().color = clearColor;
			nameBox.GetComponent<tk2dSprite>().color = clearColor;
			hButton.GetComponent<tk2dSprite>().color = clearColor;
			backdrop.SetSprite("backdrop_" + Level.ToString());
			moleculeText.transform.GetComponent<Renderer>().enabled = false;
			youMadeText.transform.GetComponent<Renderer>().enabled = false;

			yield return new WaitForSeconds(0.5f);
			float t=0;
			while (t < 1.0) {
				t+=Time.deltaTime;
				backdrop.color = Color.Lerp (clearColor,Color.white,t);
				scoreBox.GetComponent<tk2dSprite>().color = Color.Lerp (clearColor,LevelColors[Level-1],t);
				roundBox.GetComponent<tk2dSprite>().color = Color.Lerp (clearColor,LevelColors[Level-1],t);
				valenceBox.GetComponent<tk2dSprite>().color = Color.Lerp (clearColor,LevelColors[Level-1],t);
				nameBox.GetComponent<tk2dSprite>().color = Color.Lerp (clearColor,LevelColors[Level-1],t);
				hButton.GetComponent<tk2dSprite>().color = Color.Lerp (clearColor,LevelColors[Level-1],t);
				yield return 0;
			}

		}*/

		/*if (!levelUp) {
			StartCoroutine(shrinkToZero(moleculeText.transform));
			StartCoroutine(shrinkToZero(youMadeText.transform));
		}*/

		/*float t = 0;
		while (t < 1.0) {
			t+=2*Time.deltaTime;
			foreach (GameObject s in stars) {
				s.GetComponent<tk2dSprite>().color = Color.Lerp (Color.white, new Color(1.0f,1.0f,1.0f,0.0f),Mathf.SmoothStep(0.0f,1.0f,t));
			}
			yield return 0;
		}*/
		/*if (!levelUp) {
			GameObject theMolecule = GameObject.FindGameObjectWithTag("molecule");
			GameObject[] stars = GameObject.FindGameObjectsWithTag("star");
			int rate = 500;
			while (theMolecule.transform.localPosition.y > -300) {
				theMolecule.transform.Translate (Vector3.up*Time.deltaTime*rate,Space.World);
				foreach (GameObject s in stars) {
					s.transform.Translate (Vector3.up*Time.deltaTime*rate,Space.World);
				}
				rate -= 68;
				yield return 0;
			}
			GameObject.Destroy(theMolecule);
			foreach (GameObject s in stars) {
				GameObject.Destroy(s);
			}
			GameObject[] atoms = GameObject.FindGameObjectsWithTag("atom");
			foreach (GameObject a in atoms) {
				GameObject.Destroy (a);
			}
		}*/

		/*usedStamp.GetComponent<Renderer>().enabled = false;
		if (levelUp) {
			StartCoroutine (revealLevel(gameEnding));
		}else{
			beginRound(gameEnding);
		}*/

		// If the player has reached a level threshold, display the splash before revealing elements.
		if (levelUp) {
			Transform LevelTransform = GameObject.Find("Levels").transform;
			GameObject[] LevelTitle = GameObject.FindGameObjectsWithTag("leveltitle");
			RageSpline LevelNum = GameObject.Find("L" + Level).GetComponent<RageSpline>();
			float t = 0;
			while (t < 1.0) {
				t+=3*Time.deltaTime;
				LevelTransform.position = Vector3.Lerp (new Vector3(-480,350,-5),new Vector3(-480,320,-5),Mathf.SmoothStep(0.0f,1.0f,t));
				foreach (GameObject l in LevelTitle) {
					l.GetComponent<RageSpline>().fillColor1 = Color.Lerp (new Color(1.0f,1.0f,1.0f,0.0f),lightLitColor,t);
					l.GetComponent<RageSpline>().RefreshMesh(true,true,true);
				}
				LevelNum.fillColor1 = Color.Lerp (new Color(1.0f,1.0f,1.0f,0.0f),lightLitColor,t);
				LevelNum.RefreshMesh(true,true,true);
				yield return 0;
			}
			yield return new WaitForSeconds(0.5f);
			t = 0;
			while (t < 1.0) {
				t+=2*Time.deltaTime;
				foreach (GameObject l in LevelTitle) {
					l.GetComponent<RageSpline>().fillColor1 = Color.Lerp (lightLitColor,new Color(1.0f,1.0f,1.0f,0.0f),t);
					l.GetComponent<RageSpline>().RefreshMesh(true,true,true);
				}
				LevelNum.fillColor1 = Color.Lerp (lightLitColor,new Color(1.0f,1.0f,1.0f,0.0f),t);
				LevelNum.RefreshMesh(true,true,true);
				yield return 0;
			}

		}
		spawnAtoms();
		locked = false;

	}

	void storeMolecule (Transform theMolecule) {
		theMolecule.tag = "archived";

		// Determine molecule position in the archive array.
		int lastIndex = builtMolecules.Count-1;
		float myX = 2020+180*(lastIndex % 4);
		float myY = 600-180*(lastIndex / 4);
		float myZ = 1000;
		theMolecule.transform.position = new Vector3(myX,myY,myZ);

		// Determine a scale for the molecule that will fit within the array.
		float minX = theMolecule.GetComponentInChildren<Transform>().position.x;
		float maxX = theMolecule.GetComponentInChildren<Transform>().position.x;
		float minY = theMolecule.GetComponentInChildren<Transform>().position.y;
		float maxY = theMolecule.GetComponentInChildren<Transform>().position.y;
		foreach (Transform child in theMolecule) {
			if ((child.transform.position.x - (child.GetComponent<Renderer>().bounds.size.x/2) ) < minX) {minX = child.transform.position.x - (child.GetComponent<Renderer>().bounds.size.x/2);}
			if ((child.transform.position.x + (child.GetComponent<Renderer>().bounds.size.x/2) ) > maxX) {maxX = child.transform.position.x + (child.GetComponent<Renderer>().bounds.size.x/2);}
			if ((child.transform.position.y - (child.GetComponent<Renderer>().bounds.size.y/2) ) < minY) {minY = child.transform.position.y - (child.GetComponent<Renderer>().bounds.size.y/2);}
			if ((child.transform.position.y + (child.GetComponent<Renderer>().bounds.size.y/2) ) > maxY) {maxY = child.transform.position.y + (child.GetComponent<Renderer>().bounds.size.y/2);}
		}
		float moleculeWidth = maxX - minX;
		float moleculeHeight = maxY - minY;
		if (moleculeWidth*0.5f > 120 || moleculeHeight*0.5f > 120) {
			if (moleculeWidth >= moleculeHeight) {
				theMolecule.localScale = new Vector3(120f/moleculeWidth,120f/moleculeWidth,1f);
			} else {
				theMolecule.localScale = new Vector3(120f/moleculeHeight,120f/moleculeHeight,1f);
			}
		} else {
			theMolecule.localScale = new Vector3(0.5f,0.5f,1f);
		}

		// Recolor, and discard extraneous child transforms.
		foreach (Transform o in theMolecule) {
			if (o.GetComponent<RageSpline>() != null) {
				if (o.GetComponent<AtomBehavior>() != null) {
					o.GetComponent<RageSpline>().fillColor1 = o.GetComponent<AtomBehavior>().myColor;
					o.GetComponent<RageSpline>().outlineColor1 = archivedOutlineColor;
					o.GetComponent<RageSpline>().OutlineWidth = 4f;
					o.GetComponent<RageSpline>().SetAntialiasingWidth(3f);
					foreach (Transform c in o) {if (c.parent == o) {Destroy (c.gameObject);}}
				} else {
					o.GetComponent<RageSpline>().fillColor1 = archivedOutlineColor;
					o.GetComponent<RageSpline>().fillColor2 = archivedOutlineColor;
					if (o.localScale.x > 0.7f) {o.GetComponent<RageSpline>().SetAntialiasingWidth(2f);}
					if (o.localScale.x < 0.6f && o.localScale.x > 0.4f) {o.GetComponent<RageSpline>().SetAntialiasingWidth(5f);}
					if (o.localScale.x < 0.4f) {o.GetComponent<RageSpline>().SetAntialiasingWidth(10f);}

				}
				o.GetComponent<RageSpline>().RefreshMesh(true,true,true);
			}
		}

		// If the molecule has a codex entry, place a badge in the lower right corner
		if (codexDictionary.ContainsKey(moleculeName)) {
			Transform newBadge = (Transform)Instantiate(databaseMark);
			Transform newRing = (Transform)Instantiate(ring);
			newBadge.position = theMolecule.position + Vector3.forward*500;
			newRing.position = theMolecule.position + Vector3.forward*500;
		}

	}



	IEnumerator HoverRing (Transform ring) {
		Vector3 baseScale = ring.localScale;
		float t = 0;
		bool hovering = true;
		while (hovering) {
			t = 0;
			while (t < 1.0) {
				t+=0.9f*Time.deltaTime;
				ring.localScale = Vector3.Lerp (baseScale,baseScale*0.8f,Mathf.SmoothStep(0.0f,1.0f,t));
				yield return 0;
			}
			t = 0;
			while (t < 1.0) {
				t+=0.9f*Time.deltaTime;
				ring.localScale = Vector3.Lerp (baseScale*0.8f,baseScale,Mathf.SmoothStep(0.0f,1.0f,t));
				yield return 0;
			}
		}
	}

	public void sendAtom(Transform atom, Vector3 finalPos) {
		StartCoroutine (enterAtom(atom, finalPos));
	}

	IEnumerator emptyScoreRoutine () {
		int initRoundScore = roundScore;
		int initGameScore = gameScore;
		while (roundScore > 0) {
			if (roundScore > 15) {roundScore-=5; gameScore+=5;} else {roundScore-=1; gameScore+=1;}
			roundScoreText.text = roundScore.ToString ();
			gameScoreText.text = gameScore.ToString ();
			yield return new WaitForSeconds(0.01f);
		}
		roundScore = 0;
		gameScore = initGameScore + initRoundScore;
		roundScoreText.text = roundScore.ToString ();
		gameScoreText.text = gameScore.ToString ();

	}

	public void showOptions() {
		GameObject.Find ("OptionsBox").transform.position = new Vector3(-480,320,-200);
		GameObject.Find ("GameCover").GetComponent<SpriteRenderer>().color = new Color(1.0f,1.0f,1.0f,0.8f);
		GameObject.Find ("GameCanvas").GetComponent<CanvasGroup>().alpha = 0.2f;

		// Update UI to reflect current progress
		GameObject.Find("OptionsLevel").GetComponent<Text>().text = "Level " + Level;
		GameObject.Find("OptionsScore").GetComponent<Text>().text = "Your Score: " + gameScore;
		GameObject.Find("OptionsCodex").GetComponent<Text>().text = "Codex: " + foundCodexEntries.Count + "/30";
		RageSpline[] lights = GameObject.Find("OptionsCodexRing").transform.GetComponentsInChildren<RageSpline>();
		for (int i=0; i<50; i++) {
			lights[i].fillColor1 = new Color(1f,1f,1f,0.2f);
			lights[i].RefreshMesh(true,true,true);
		}
		foreach (int i in foundCodexEntries) {
			lights[i].fillColor1 = Color.white;
			lights[i].RefreshMesh(true,true,true);
		}

	}

	public void reboot() {

		// Reset all game variables
		RoundNumber = 0;
		Level = 0;
		gameScore = 0;
		roundScore = 0;
		GameObject.Find ("RoundScoreText").GetComponent<Text>().text = "0";
		GameObject.Find ("GameScoreText").GetComponent<Text>().text = "0";
		builtMolecules = new List<string>();
		foundCodexEntries = new List<int>();
		newCompounds = new List<string>();

		// Destroy all molecule and atom objects
		foreach (GameObject m in GameObject.FindGameObjectsWithTag("molecule")) Destroy (m);
		foreach (GameObject a in GameObject.FindGameObjectsWithTag("atom")) Destroy (a);
		foreach (GameObject b in GameObject.FindGameObjectsWithTag("bond")) Destroy (b);
		foreach (GameObject d in GameObject.FindGameObjectsWithTag("archived")) Destroy (d);
		foreach (GameObject r in GameObject.FindGameObjectsWithTag("ring")) Destroy (r);
		foreach (GameObject k in GameObject.FindGameObjectsWithTag("databasemark")) Destroy (k);

		// Animate transition to title card
		StartCoroutine (rebootRoutine());
	}

	IEnumerator rebootRoutine() {
		hideGameHUD();
		SpriteRenderer gameCover = GameObject.Find("GameCover").GetComponent<SpriteRenderer>();
		Color initColor = gameCover.color;
		float t = 0;
		while (t < 1.0) {
			t+=2*Time.deltaTime;
			gameCover.color = Color.Lerp (initColor, Color.white,Mathf.SmoothStep(0.0f,1.0f,t));
			yield return 0;
		}
		showTitle ();
		inputLock = false;
	}



	public void sendMail (string messageString) {
		StartCoroutine(sendMailRoutine(messageString));

		/*MailMessage mail = new MailMessage();

		mail.From = new MailAddress("valence_client@benbinder.com");
		mail.To.Add("valence@benbinder.com");
		mail.Subject =  "[Valence Client Message] - " + System.DateTime.Now;
		string fullstring = "Player session summary:\n\nLevel " + Level + "\nScore: " + gameScore + "\nCodex: " + foundCodexEntries.Count + "\n\nMessage for developer:\n\n" + messageString;
		mail.Body = fullstring;

		SmtpClient smtpServer = new SmtpClient("mail.benbinder.com");
		smtpServer.Port = 587;
		smtpServer.Credentials = new System.Net.NetworkCredential("valence_client@benbinder.com", "devokan5") as ICredentialsByHost;
		smtpServer.EnableSsl = true;
		ServicePointManager.ServerCertificateValidationCallback =
			delegate(object s, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
		{ return true; };
		smtpServer.Send(mail);

		// Reset options box
		GameObject.Find ("SendingMessage").GetComponent<Text>().color = Color.clear;
		GameObject.Find ("InputField").GetComponent<InputField>().text = "";
		GameObject.Find ("OptionsBox").transform.position = new Vector3(-480,320,-200);
		GameObject.Find ("EmailBox").transform.position = new Vector3(-480,2000,-200);

		// Set visual elements for ok message
		GameObject.Find ("OptionsConfirm").GetComponent<Text>().text = "Message sent!";
		GameObject.Find ("CloseOptions").GetComponent<Text>().text = "OK";
		GameObject.Find ("EndGame").GetComponent<Text>().text = "";
		GameObject.Find ("ResetCodex").GetComponent<Text>().text = "";
		GameObject.Find ("ResetCodexButton").GetComponent<BoxCollider2D>().enabled = false;
		GameObject.Find ("ResetCodexButton").GetComponent<MeshRenderer>().enabled = false;
		GameObject.Find ("EndGameButton").GetComponent<BoxCollider2D>().enabled = false;
		GameObject.Find ("EndGameButton").GetComponent<MeshRenderer>().enabled = false;
		foreach (MeshRenderer m in GameObject.Find ("EmailButton").GetComponentsInChildren<MeshRenderer>()) m.enabled = false;
		GameObject.Find ("EmailButton").GetComponent<BoxCollider2D>().enabled = false;
		foreach (MeshRenderer m in GameObject.Find ("OptionsCodexRing").GetComponentsInChildren<MeshRenderer>()) m.enabled = false;
		GameObject.Find ("OptionsTitle").GetComponent<Text>().color = Color.clear;
		GameObject.Find ("OptionsLevel").GetComponent<Text>().color = Color.clear;
		GameObject.Find ("OptionsScore").GetComponent<Text>().color = Color.clear;
		GameObject.Find ("OptionsCodex").GetComponent<Text>().color = Color.clear;
		GameObject.Find ("OptionsConfirm").GetComponent<Text>().color = Color.white;
		*/


	}

	IEnumerator sendMailRoutine (string messageString) {
		string builtstring = "";
		foreach (string m in builtMolecules) builtstring += m + "\n";
		string fullstring = "Player session summary:\n\nLevel " + Level + "\nScore: " + gameScore + "\nCodex: " + foundCodexEntries.Count + "\n\nMessage for developer:\n\n" + messageString + "\n\nMolecules built:\n\n" + builtstring;

		WWWForm form = new WWWForm();
		form.AddField("myEmail", "valence@benbinder.com");
		form.AddField("mySubject","[Valence Client Message] - " + System.DateTime.Now);
		form.AddField("myMessage", fullstring);
		using(UnityWebRequest www = UnityWebRequest.Post("http://benbinder.com/resources/php/valenceEmail.php", form)) {
			yield return www.Send();

			if(www.isNetworkError) {
				GameObject.Find ("OptionsConfirm").GetComponent<Text>().text = "Message could not be sent.";
				Debug.Log(www.error);
			} else {
				GameObject.Find ("OptionsConfirm").GetComponent<Text>().text = "Message sent!";
				Debug.Log(www.downloadHandler.text);
			}
		}

		// Reset options box
		GameObject.Find ("SendingMessage").GetComponent<Text>().color = Color.clear;
		GameObject.Find ("InputField").GetComponent<InputField>().text = "";
		GameObject.Find ("OptionsBox").transform.position = new Vector3(-480,320,-200);
		GameObject.Find ("EmailBox").transform.position = new Vector3(-480,2000,-200);

		// Set visual elements for ok message
		GameObject.Find ("CloseOptions").GetComponent<Text>().text = "OK";
		GameObject.Find ("EndGame").GetComponent<Text>().text = "";
		GameObject.Find ("ResetCodex").GetComponent<Text>().text = "";
		GameObject.Find ("ResetCodexButton").GetComponent<BoxCollider2D>().enabled = false;
		GameObject.Find ("ResetCodexButton").GetComponent<MeshRenderer>().enabled = false;
		GameObject.Find ("EndGameButton").GetComponent<BoxCollider2D>().enabled = false;
		GameObject.Find ("EndGameButton").GetComponent<MeshRenderer>().enabled = false;
		foreach (MeshRenderer m in GameObject.Find ("EmailButton").GetComponentsInChildren<MeshRenderer>()) m.enabled = false;
		GameObject.Find ("EmailButton").GetComponent<BoxCollider2D>().enabled = false;
		foreach (MeshRenderer m in GameObject.Find ("OptionsCodexRing").GetComponentsInChildren<MeshRenderer>()) m.enabled = false;
		GameObject.Find ("OptionsTitle").GetComponent<Text>().color = Color.clear;
		GameObject.Find ("OptionsLevel").GetComponent<Text>().color = Color.clear;
		GameObject.Find ("OptionsScore").GetComponent<Text>().color = Color.clear;
		GameObject.Find ("OptionsCodex").GetComponent<Text>().color = Color.clear;
		GameObject.Find ("OptionsConfirm").GetComponent<Text>().color = Color.white;

	}



	/* -----------------------------------------
	 * AUXILIARY FUNCTIONS
	 * ----------------------------------------- */


	IEnumerator growFromZero(Transform trans) {
		Vector3 zeroScale = new Vector3(1.0f,0.0f,1.0f);
		trans.localScale = zeroScale;
		//trans.renderer.enabled = true;
		float t = 0;
		while (t < 1.0) {
			t+=4*Time.deltaTime;
			trans.localScale = Vector3.Lerp (zeroScale, Vector3.one,Mathf.SmoothStep(0.0f,1.0f,t));
			yield return 0;
		}
	}

	IEnumerator shrinkToZero(Transform trans) {
		Vector3 zeroScale = new Vector3(1.0f,0.0f,1.0f);
		Vector3 initScale = trans.localScale;
		float t = 0;
		while (t < 1.0) {
			t+=4*Time.deltaTime;
			trans.localScale = Vector3.Lerp (initScale, zeroScale,Mathf.SmoothStep(0.0f,1.0f,t));
			yield return 0;
		}
	}

	IEnumerator shrinkAndDestroy(Transform trans) {
		Vector3 zeroScale = new Vector3(0.0f,1.0f,1.0f);
		Vector3 initScale = trans.localScale;
		float t = 0;
		while (t < 1.0) {
			t+=6*Time.deltaTime;
			trans.localScale = Vector3.Lerp (initScale, zeroScale,Mathf.SmoothStep(0.0f,1.0f,t));
			yield return 0;
		}
		Destroy(trans.gameObject);
	}

	Vector3 LaGrangeVector (float percent, Vector3 p1, Vector3 p2, Vector3 p3) {
		Vector3 output = Vector3.zero;
		float myX = p1.x + (p3.x-p1.x)*percent;
		float myY = ((myX-p2.x)*(myX-p3.x))/((p1.x-p2.x)*(p1.x-p3.x))*p1.y + ((myX-p1.x)*(myX-p3.x))/((p2.x-p1.x)*(p2.x-p3.x))*p2.y + ((myX-p1.x)*(myX-p2.x))/((p3.x-p1.x)*(p3.x-p2.x))*p3.y;
		float myZ = p1.z;
		output = new Vector3(myX,myY,myZ);
		return output;
	}

	IEnumerator textTicker (Text theText, string theString, float speed) {
		int i = 0;
		while (i < theString.Length) {
			if (theString.Length-i > i+20) {
				i+=20;
			} else {
				i+=theString.Length-i;
			}
			theText.text = theString.Substring(0,i);
			yield return new WaitForSeconds(speed);
		}
	}

}
