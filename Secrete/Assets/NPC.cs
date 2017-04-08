using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : Creature {

	/* --- Objects --- */
	public CharacterController charController;
	public Transform player;
	public GameObject opponent;

	/* --- Stats --- */
	protected int exp;
	protected float attackCooldown;
	protected float lastAttack;
	protected Attack mainAttack;
	protected float speed;
	protected float aggroRange;
	public float weaponRange;
	protected float livingRadius;
	protected float chasingTime;
	protected float lastChaseTime;
	protected int lastWalkTime = 0; 				//Last time, the NPC walked
	protected int nextWalk; 					//Next walk
	private Vector3 currentGoal;
	public Vector3 startPosition;
	private int rotationSpeed = 10;				//Time to rotate
	public double impactTime;
	private AggroMeter aggroMeter = new AggroMeter ();

	protected float lastSpecialAttack;
	private int coolDownSpecialAttackMin = 2;
	private int coolDownSpecialAttackMax = 5;

	/* --- Social --- */
	protected ArrayList enemies;


	/* --- Bools --- */
	public bool walking;
	public bool gettingHit;
	public bool attacking;

	// Use this for initialization
	void Start () {
	}

	// Update is called once per frame
	void Update () {
		//Debug.Log (!specialAttack ());
		if (!isDead) {
			checkRadiusForOpponents ();
			handleAggro ();
			checkIfUnderAttack (false);
			if (!gettingHit) {
				if (hasOpponent ()) {
					if (!opponent.GetComponent<Creature> ().isDead && !specialAttack () && npcInChasingTime ()) {
						chase ();
					} else if (Vector3.Distance (transform.position, opponent.transform.position) > 2)
						lastChaseTime = Time.time;
				} else {
					live ();
				}
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
		checkIfUnderAttack (true);
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
		if (opponentInAggroRange())
			lastChaseTime = Time.time;
		transform.LookAt (opponent.transform.position);
		if (Vector3.Distance (transform.position, opponent.transform.position) > 2) {
			if (!attacking && !isInAttackRange ()) {
				charController.SimpleMove (transform.forward * speed);
				GetComponent<Animation> ().CrossFade (run.name);
			}
		} else {
			lastAttack = Time.time;
			if(GetComponent<Animation>().IsPlaying(run.name))			//else NPC would permanently be waiting for fight if he's near enaugh
				GetComponent<Animation> ().CrossFade (waitingForFight.name);
		}
	}

	protected void resetGettingHit() {
		if (!GetComponent<Animation> ().IsPlaying (getHitAnim.name))
			gettingHit = false;
	}

	/*
	 * Handles death of NPC and destroyes the CharacterController.
	 */
	protected void dieAndBecomeCorpse() {
		GetComponent<Animation> ().CrossFade (die.name);
		isCorpse = true;
		gameObject.tag = "Corpse";
		Destroy (charController);
	}

	public bool specialAttack() {
		if(GetComponent<Animation>().IsPlaying(run.name))			//else NPC would permanently be waiting for fight if he's near enaugh
			GetComponent<Animation> ().CrossFade (waitingForFight.name);
		ArrayList specialAttacks = allReadySpecialAttacks ();
		int countSpecialAttacks = specialAttacks.Count;
		if (countSpecialAttacks != 0) {
			int index = (int)Random.value * (countSpecialAttacks - 1);
			if (Time.time > lastSpecialAttack + coolDownSpecialAttack ()) {
				lastSpecialAttack = Time.time;
				Attack attackToDo = (Attack)specialAttacks [index];
				return attackToDo.activate ();
			}
		} else {
			return mainAttack.activate ();
		}
		return false;
	}

	private ArrayList allReadySpecialAttacks () {
		Attack[] allAttacks = GetComponents<Attack> ();
		ArrayList readyAttacks = new ArrayList ();
		if (allAttacks != null) {
			foreach (Attack attack in allAttacks) {
				if (attack.attackReady)
					readyAttacks.Add (attack);
			}
			return readyAttacks;
		}

		return readyAttacks;
	}

	private int coolDownSpecialAttack() {
		int temp = (int) (Random.value * (coolDownSpecialAttackMax - coolDownSpecialAttackMin));
		return temp + coolDownSpecialAttackMin;
	}

	private void resetAttackCooldowns() {
		lastSpecialAttack = Time.time;
	}

	private bool opponentInAggroRange() {
		return Vector3.Distance(transform.position, opponent.transform.position) < aggroRange;
	}

	private bool isInAttackRange() {
		float attackRange = mainAttack.attackRange;
		return attackRange > Vector3.Distance (opponent.transform.position, transform.position);
	}


	/* ------------------------------ Live and walk ------------------------------ */
	protected void live () {
		opponent = null;
		aggroMeter = new AggroMeter ();
		resetAttackCooldowns ();
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
		if (opponent != null && opponent.GetComponent<Creature> ().isDead)
			opponent = null;
		return opponent != null;
	}

	/*
	 * Checks the aggroRange for enemies and chose the next one. Only works if there isn't
	 * an opponent declared as @opponent.
	 */
	private void checkRadiusForOpponents() {
		if (!hasOpponent () || opponent.GetComponent<Creature>().isDead) {
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
		} else if (Vector3.Distance (opponent.transform.position, transform.position) > aggroRange && !npcInChasingTime()) {
			//aggroMeter.deleteAggro (opponent);
			aggroMeter = new AggroMeter();
			opponent = null;
		}
	}
}
