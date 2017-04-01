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

	/* --- Stats --- */
	public int health;
	protected int damage;

	/* --- Bools --- */
	public bool isDead;
	protected bool isCorpse;
	public bool underAttack;
	private float lastTimeUnderAttack = 0;
	private int underAttackCoolDown = 7;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public abstract void getHit(int dmg, GameObject opponent);

	protected void checkIfUnderAttack(bool attack) {
		if (attack) {
			lastTimeUnderAttack = Time.time;
			underAttack = true;
		}
		else if (lastTimeUnderAttack + underAttackCoolDown < Time.time) {
			underAttack = false;
		}
	}
}
