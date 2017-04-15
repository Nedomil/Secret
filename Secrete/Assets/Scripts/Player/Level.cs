using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour {

	public int level;
	public int exp;
	private ArrayList expNeeded;

	// Use this for initialization
	void Start () {
		level = 1;
		expNeeded = new ArrayList ();
		expNeeded.Add (100);
		expNeeded.Add (250);
		expNeeded.Add (450);
		expNeeded.Add (600);
		expNeeded.Add (999999999);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void increaseExp (int exp) {
		Debug.Log ("" + exp + " exp");
		this.exp += exp;
		checkIfLevelUp ();
	}

	private void checkIfLevelUp() {
		if (exp >= (int)expNeeded [level - 1]) {
			level++;
			Debug.Log ("You reached level " + level + "!");
		}
	}
}
