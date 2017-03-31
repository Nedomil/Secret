using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : Creature {

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
	private AggroMeter aggroMeter = new AggroMeter ();

	/* --- Social --- */
	protected ArrayList enemies;


	/* --- Bools --- */
	public bool walking;
	protected bool gettingHit;


	// Use this for initialization
	void Start () {
	}

	// Update is called once per frame
	void Update () {
		checkRadiusForOpponents ();
		if (!isDead) {
			handleAggro ();
			if (!gettingHit) {
				if (opponentInAttackRange () && !opponent.GetComponent<Creature> ().isDead) {
					//so the NPC don't attack imediatly after the fight ist starting
					if (Time.time - lastAttack > 3) {
						lastAttack = Time.time;
						GetComponent<Animation> ().CrossFade (waitingForFight.name);
					}
					transform.LookAt (opponent.transform.position);
					//Only attacks if cooldow is over
					if (Time.time > attackCooldown + lastAttack) {
						GetComponent<Animation> ().CrossFade (attack.name);
						lastAttack = Time.time;
					}
				} else if ((hasOpponent () && npcInChasingTime ()) && !opponent.GetComponent<Creature> ().isDead)					
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

	/* ------------------------------ Fight ------------------------------ */

	/*
	* Handles what happens if NPC get hit.
	* @damage is incomming
	* @opponent which is dealing damage
	*/
		public override void getHit(int damage, GameObject opponent) {
		if (this.name == "SkeletonBoss") {
			Debug.Log (aggroMeter.getOpponentAndAggro (0));
			Debug.Log (aggroMeter.getOpponentAndAggro (1));
			Debug.Log (aggroMeter.getOpponentAndAggro (2));
		}
		handleAggro (opponent, damage);
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


	/*
	 * Handles @aggroMeter. Adds new oppoennt and changes opponent if a new one
	 * has the most aggro.
	 * @opponent which has to bi add
	 * @dmg of the opponent
	 */
	private void handleAggro(GameObject opponent, int dmg) {
		aggroMeter.addAggro (opponent, dmg);
		handleAggro ();
	}

	/*
	 * Helps handleAggro(GameObject, int) to find the new opponent with the
	 * highest aggro.
	 */
	private void handleAggro() {
		GameObject highestAggroOpponent = aggroMeter.getHighestAggro ();

		while (highestAggroOpponent != null && highestAggroOpponent.GetComponent<Creature> ().isDead) {
			aggroMeter.deleteAggro (highestAggroOpponent);
			highestAggroOpponent = aggroMeter.getHighestAggro ();
		}

		if (highestAggroOpponent != null)
			this.opponent = highestAggroOpponent;
	}

	void OnMouseOver() {
		player.GetComponent<Combat> ().setOpponent(gameObject);
	}

	/*
	 * Checks if the NPC is chasing something.
	*/
	protected bool npcInChasingTime () {
		return Time.time < chasingTime + lastChaseTime;
	}

	/*
	 * Follows the @chasingTarget
	*/
	protected void chase() {
		// if player isn't in range, the chasingTime will stop updating
		if (hasOpponent ())
			lastChaseTime = Time.time;
		transform.LookAt (opponent.transform.position);
		if (Vector3.Distance (transform.position, opponent.transform.position) > 2) {
			charController.SimpleMove (transform.forward * speed);
			GetComponent<Animation> ().CrossFade (run.name);
		} else {
			lastAttack = Time.time;
		}
	}

	protected bool opponentInAttackRange() {
		if (opponent == null)
			return false;
		return Vector3.Distance (transform.position, opponent.transform.position) < attackRange;
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
			if (opponentInAttackRange () && !impacted && GetComponent<Animation> ().IsPlaying (attack.name)) {
				if (GetComponent<Animation> () [attack.name].time > GetComponent<Animation> () [attack.name].length * impactTime) {
					opponent.GetComponent<Creature> ().getHit (damage, this.gameObject);
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
		opponent = null;
		aggroMeter = new AggroMeter ();
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

	private bool hasOpponent() {
		return opponent != null;
	}

	private void checkRadiusForOpponents() {
		if (!hasOpponent ()) {
			float npcDistance = 99999999;
			foreach (string enemy in enemies) {
				GameObject[] npcs = GameObject.FindGameObjectsWithTag (enemy);
				foreach (GameObject npc in npcs) {
					float distance = Vector3.Distance (npc.transform.position, transform.position);
					if (distance != 0 && distance < aggroRange) {
						if (opponent == null || distance < npcDistance) {
							opponent = npc;
							npcDistance = distance;
							lastChaseTime = Time.time;
						}
					}
				}
			}
		} else if (Vector3.Distance (opponent.transform.position, transform.position) > aggroRange) {
			//aggroMeter.deleteAggro (opponent);
			aggroMeter = new AggroMeter();
			opponent = null;
		}
	}
}
