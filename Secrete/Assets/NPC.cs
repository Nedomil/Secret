using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour {

	/* --- AnimationClips --- */
	public AnimationClip run;
	public AnimationClip idle;
	public AnimationClip die;
	public AnimationClip attack;
	public AnimationClip getHitAnim;
	public AnimationClip waitingForFight;

	/* --- Objects --- */
	public CharacterController charController;
	public Transform player;
	public GameObject opponent;

	/* --- Stats --- */
	protected int health;
	protected int damage;
	protected int exp;
	protected float attackCooldown;
	protected float lastAttack;
	protected float speed;
	protected float aggroRange;
	protected float attackRange;
	protected float livingRadius;
	protected float chasingTime;
	protected float lastChaseTime;
	protected int lastWalkTime = 0; 				//Last time, the NPC walked
	protected int nextWalk; 					//Next walk
	private Vector3 currentGoal;
	public Vector3 startPosition;
	private int rotationSpeed = 10;				//Time to rotate
	public double impactTime;
	private bool impacted;


	/* --- Bools --- */
	public bool walking;
	protected bool gettingHit;
	protected bool isDead;
	protected bool isCorpse;


	// Use this for initialization
	void Start () {
	}

	/* ------------------------------ Fight ------------------------------ */

	/*
	* Handles what happens if NPC get hit.
	* @damage is incomming
	* @opponent which is dealing damage
	*/
	public void getHit(int damage, GameObject opponent) {
		GetComponent<Animation> ().CrossFade (getHitAnim.name);
		GetComponent<Animation> () [getHitAnim.name].time = 0.42f;
		lastAttack += 0.5f;
		gettingHit = true;
		health -= damage;
		if (health < 0) {
			health = 0;
			isDead = true;
			if (opponent.name == "Player")
				opponent.GetComponent<Level> ().increaseExp (exp);
		}
	}

	// Update is called once per frame
	void Update () {
		if (!isDead) {
			if (!gettingHit) {
				if (playerInAttackRange () && !player.GetComponent<Combat> ().isDead) {
					//so the NPC don't attack imediatly after the fight ist starting
					if (Time.time - lastAttack > 3) {
						lastAttack = Time.time;
						GetComponent<Animation> ().CrossFade (waitingForFight.name);
					}
					transform.LookAt (player.position);
					//Only attacks if cooldow is over
					if (Time.time > attackCooldown + lastAttack) {
						GetComponent<Animation> ().CrossFade (attack.name);
						lastAttack = Time.time;
					}
				} else if ((playerInRange () || npcInChasingTime ()) && !player.GetComponent<Combat> ().isDead)					
					chase ();
				else
					live ();

				impact ();
			}
			resetGettingHit ();

		} else {
			if (!isCorpse) {
				dieAndBecomeCorpse ();
			}
		}
	}

	void OnMouseOver() {
		player.GetComponent<Combat> ().setOpponent(gameObject);
	}

	/*
	 * Checks if the player is in Range
	*/
	protected bool playerInRange() {
		return Vector3.Distance (transform.position, player.position) < aggroRange;
	}

	/*
	 * Checks if the NPC is chasing something.
	*/
	protected bool npcInChasingTime () {
		return chasingTime > Time.time - lastChaseTime;
	}

	/*
	 * Follows the @chasingTarget
	*/
	protected void chase() {
		// if player isn't in range, the chasingTime will stop updating
		if (playerInRange ())
			lastChaseTime = Time.time;
		transform.LookAt (player.position);
		if (Vector3.Distance (transform.position, player.transform.position) > 2) {
			charController.SimpleMove (transform.forward * speed);
			GetComponent<Animation> ().CrossFade (run.name);
		} else {
			lastAttack = Time.time;
		}
	}

	protected bool playerInAttackRange() {
		return Vector3.Distance (transform.position, player.position) < attackRange;
	}

	protected void resetGettingHit() {
		if (!GetComponent<Animation> ().IsPlaying (getHitAnim.name))
			gettingHit = false;
	}

	protected void impact() {
		if (!gettingHit) {
			if (GetComponent<Animation> () [attack.name].time < GetComponent<Animation> () [attack.name].length * 0.1) {
				impacted = false;
			}
			if (playerInAttackRange () && !impacted && GetComponent<Animation> ().IsPlaying (attack.name)) {
				if (GetComponent<Animation> () [attack.name].time > GetComponent<Animation> () [attack.name].length * impactTime) {
					player.GetComponent<Combat> ().getHit (damage);
					impacted = true;
				}
			}
		}
	}

	protected void dieAndBecomeCorpse() {
		GetComponent<Animation> ().CrossFade (die.name);
		isCorpse = true;
		Destroy (charController);
	}


	/* ------------------------------ Live and walk ------------------------------ */
	protected void live () {
		if (walking)
			goToPosition (currentGoal);
		else if (Vector3.Distance (startPosition, transform.position) > livingRadius) {
			randomNextWalkIn ();
			currentGoal = startPosition;
			goToPosition (currentGoal);
		} else {
			if (nextWalk + lastWalkTime < Time.time) {
				randomNextWalkIn ();
				currentGoal = randomPlaceInLivingRadius ();
				currentGoal.y = startPosition.y; // else the circle-bug will happen
				goToPosition (currentGoal);
			}
		}
	}

	private void goToPosition (Vector3 v) {
		if (Vector2.Distance (transform.position, v) > 1) {
			lookAtObject (v);
			charController.SimpleMove (transform.forward * speed);
			GetComponent<Animation> ().CrossFade (run.name);
			GetComponent<Animation> () [run.name].speed = relationSpeedToNormalSpeed ();
			walking = true;
		} else {
			walking = false;
			GetComponent<Animation> ().CrossFade (idle.name);
		}
	}

	private void lookAtObject(Vector3 position) {
		Quaternion newRotation = Quaternion.LookRotation (position - transform.position);
		newRotation.x = 0f;
		newRotation.z = 0f;
		transform.rotation = Quaternion.Slerp (transform.rotation, newRotation, Time.deltaTime * rotationSpeed);
	}

	private Vector3 randomPlaceInLivingRadius() {
		return (Vector3)Random.onUnitSphere * livingRadius + startPosition;
	}

	private void randomNextWalkIn () {
		nextWalk = (int)Random.value * 10 + 5;
		lastWalkTime = (int)Time.time;
	}

	private float relationSpeedToNormalSpeed() {
		return speed / 3;
	}
}
