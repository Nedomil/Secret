using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Combat : Creature {

	/* --- Objects --- */
	public GameObject player;
	public GameObject chaseTarget;

	/* --- Stats --- */
	private float opponentTime;

	/* --- Bools --- */
	public bool chasing;

	/* --- Attacks --- */
	private Attack mainAttack;

	// Use this for initialization
	void Start () {
		health = 1000;
		damage = 35;
		weaponRange = 2f;
		setUpMainAttack ();
	}

	private void setUpMainAttack() {
		gameObject.AddComponent<NormalMeleeAttack> ();
		mainAttack = GetComponent<NormalMeleeAttack> ();
		mainAttack.defaultCooldown = false;
		mainAttack.coolDownSpecialAttackMax = 1;
		mainAttack.coolDownSpecialAttackMin = 1;
	}

	// Update is called once per frame
	void Update () {
		//Debug.Log(Vector3.Distance(transform.position, opponent.transform.position));
		//Debug.Log (chasing);
		RaycastHit hit;
		if (!isDead) {
			if (!gettingHit) {
				Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
				if (Physics.Raycast (ray, out hit, 1000)) {
					if ((hit.collider.tag == "Fraction1" || hit.collider.tag == "Fraction2") && Input.GetMouseButtonDown (0)) {
						if (!GetComponent<Animation>().IsPlaying(attack.name) && mainAttack.activate()) {
							ClickToMove.isAttacking = true;
						} else {
							chaseTarget = opponent;
							chasing = true;
							chase ();
						}
					}
				}

				if (!GetComponent<Animation> ().IsPlaying (attack.name)) {
					ClickToMove.isAttacking = false;
				}
				resetOpponent ();
			}
			resetGettingHit ();
		} else {
			if (!isCorpse) {
				dieAndBecomeCorpse ();
			}
		}
		chase ();
	}

	private void resetGettingHit() {
		if (!GetComponent<Animation> ().IsPlaying (getHitAnim.name))
			gettingHit = false;
	}

	public override void getHit(int damage, GameObject opponent) {
		GetComponent<Animation> ().CrossFade (getHitAnim.name);
		GetComponent<Animation> () [getHitAnim.name].time = 0.42f;
		gettingHit = true;
		health -= damage;
		if (health < 0) {
			health = 0;
			isDead = true;
		}
	}

	private void dieAndBecomeCorpse() {
		GetComponent<Animation> ().CrossFade (die.name);
		isCorpse = true;
		Destroy (GetComponent<CharacterController>());
		Destroy (GetComponent<ClickToMove> ());
	}

	private void resetOpponent() {
		if (Time.time - opponentTime > 10f)
			opponent = null;
	}

	public void setOpponent(GameObject opponent) {
		opponentTime = Time.time;
		this.opponent = opponent;
	}

	private void chase() {
		if (chasing) {
			transform.LookAt (chaseTarget.transform.position);
			if (!mainAttack.opponentInAttackRange()) {
				GetComponent<CharacterController> ().SimpleMove (transform.forward * GetComponent<ClickToMove> ().speed);
				GetComponent<Animation> ().CrossFade (run.name);
			} else {
				if (mainAttack.activate()) {
					chasing = false;
					GetComponent<ClickToMove> ().position = transform.position;
					ClickToMove.isAttacking = true;
				}
			}
		}
	}
}
