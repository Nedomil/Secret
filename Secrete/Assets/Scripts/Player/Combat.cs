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
	private Attack attack1;
	private Attack attack2;
	private Attack attack3;

	// Use this for initialization
	void Start () {
		health = 1000;
		damage = 35;
		weaponRange = 2f;
		setUpMainAttack ();
		setUp1 ();
		setUp2 ();
		setUp3 ();
	}

	private void setUpMainAttack() {
		gameObject.AddComponent<NormalMeleeAttack> ();
		mainAttack = GetComponent<NormalMeleeAttack> ();
		mainAttack.defaultCooldown = false;
		mainAttack.coolDownSpecialAttackMax = 1;
		mainAttack.coolDownSpecialAttackMin = 1;
	}

	private void setUp1() {
		gameObject.AddComponent<Fireball> ();
		attack1 = GetComponent<Fireball> ();
		attack1.defaultCooldown = false;
		attack1.coolDownSpecialAttackMax = 6;
		attack1.coolDownSpecialAttackMin = 6;
		attack1.defaultStats = false;
		attack1.damageMultiplicator = 2;
	}

	private void setUp2() {
		gameObject.AddComponent<FireballSlave5> ();
		attack2 = GetComponent<FireballSlave5> ();
		attack2.defaultCooldown = false;
		attack2.coolDownSpecialAttackMax = 10;
		attack2.coolDownSpecialAttackMin = 10;
		attack2.defaultStats = false;
		attack2.damageMultiplicator = 1;
	}

	private void setUp3() {
		gameObject.AddComponent<FireStorm> ();
		attack3 = GetComponent<FireStorm> ();
		attack3.defaultCooldown = false;
		attack3.coolDownSpecialAttackMax = 1;
		attack3.coolDownSpecialAttackMin = 1;
		attack3.defaultStats = false;
		attack3.damageMultiplicator = 0.25f;
	}

	//Update is called once per frame
	void Update () {
		RaycastHit hit;
		if (!isDead) {
			if (!gettingHit) {
				if (Input.GetKey (KeyCode.Alpha1)) {
					attack1.activate ();
				} else if (Input.GetKey (KeyCode.Alpha2)) {
					attack2.activate ();
				} else if (Input.GetKey (KeyCode.Alpha3)) {
					attack3.activate ();
				}
				else {
					Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
					if (Physics.Raycast (ray, out hit, 1000)) {
						if ((hit.collider.tag == "Fraction1" || hit.collider.tag == "Fraction2") && Input.GetMouseButtonDown (0)) {
							if (!GetComponent<Animation> ().IsPlaying (attack.name) && mainAttack.activate ()) {
								//do nothing. Why?
							} else {
								chaseTarget = opponent;
								chasing = true;
								chase ();
							}
						}
					}
					resetOpponent ();
				}
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

	/// <summary> test </summary>
	public override void getHit(int damage, GameObject opponent) {
		if (!isDead) {
			GetComponent<Animation> ().CrossFade (getHitAnim.name);
			GetComponent<Animation> () [getHitAnim.name].time = 0.42f;
			gettingHit = true;
			health -= damage;
			if (health < 0) {
				health = 0;
				isDead = true;
			}
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
				}
			}
		}
	}
}
