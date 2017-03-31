using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class Creature : MonoBehaviour  {

	/* --- AnimationClips --- */
	public AnimationClip run;
	public AnimationClip idle;
	public AnimationClip die;
	public AnimationClip attack;
	public AnimationClip getHitAnim;
	public AnimationClip waitingForFight;

	/* --- Bools --- */
	public bool isDead;
	protected bool isCorpse;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public abstract void getHit(int dmg, GameObject opponent);
}
