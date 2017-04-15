using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class Attack : MonoBehaviour {

	protected Creature creature;
	protected float lastSpecialAttack;
	public int coolDownSpecialAttackMin;
	public int coolDownSpecialAttackMax;
	public bool attackReady = false;
	public float effectivelyAttackRange;
	public float attackRange;
	public float damageMultiplicator;
	public int damage;


	public bool isAttacking;
	public bool defaultCooldown = true;
	public bool defaultStats = true;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public virtual bool activate () {
		setUpActivate ();
		return false;
	}

	protected void setUpActivate() {
		lastSpecialAttack = Time.time;
		attackReady = false;
		isAttacking = true;
	}

	protected void readyCheck() {
		if (Time.time > lastSpecialAttack + coolDownSpecialAttack()) {
			attackReady = true;
		}
	}

	private int coolDownSpecialAttack() {
		int temp = (int) (Random.value * (coolDownSpecialAttackMax - coolDownSpecialAttackMin));
		return temp + coolDownSpecialAttackMin;
	}

	protected void stopAttackAnimation() {
		if (isAttacking && creature.GetComponent<Animation> ().IsPlaying (creature.attack.name) &&
			GetComponent<Animation> () [creature.attack.name].time > GetComponent<Animation> () [creature.attack.name].length * 0.8) {
			GetComponent<Animation> ().CrossFade (creature.waitingForFight.name);
			isAttacking = false;
		}
		if (!GetComponent<Animation> ().IsPlaying (creature.attack.name))
			isAttacking = false;
	}

	/*
	 * Returns, if opponent is in attack Range.
	 */
	public virtual bool opponentInAttackRange() {
		GameObject opponent = creature.opponent;
		if (opponent == null)
			return false;
		return Vector3.Distance (transform.position, opponent.transform.position) < attackRange;
	}

	protected Vector3 getMousePosition() {
		Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
		RaycastHit hit;
		Physics.Raycast (ray, out hit, 1000);
		return hit.point;
	}
}
