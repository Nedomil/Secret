using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Combat : MonoBehaviour {

	/* --- AnimationClips --- */
	public AnimationClip attack;
	public AnimationClip die;
	public AnimationClip getHitAnim;
	public AnimationClip run;

	/* --- Objects --- */
	private GameObject opponent;
	public GameObject player;
	public GameObject chaseTarget;

	/* --- Stats --- */
	public int health = 100;
	public int damage = 35;
	private float opponentTime;
	private float attackRange = 2f;
	public double impactTime;
	private bool impacted;

	/* --- Bools --- */
	public bool gettingHit;
	public bool chasing;
	public  bool isDead;
	private bool isCorpse;





	// Use this for initialization
	void Start () {
		
	}

	// Update is called once per frame
	void Update () {
		RaycastHit hit;
		if (!isDead) {
			if (!gettingHit) {
				Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
				if (Physics.Raycast (ray, out hit, 1000)) {
					if (hit.collider.tag == "Enemy" && Input.GetMouseButtonDown (0)) {
						if (Vector3.Distance (transform.position, opponent.transform.position) < attackRange) {
							transform.LookAt (opponent.transform.position);
							GetComponent<Animation> ().CrossFade (attack.name);
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
					impacted = false;
				}
				resetOpponent ();

				impact ();
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

	private void impact() {
		if (opponent != null && !impacted && GetComponent<Animation>().IsPlaying (attack.name)) {
			if (GetComponent<Animation> () [attack.name].time > GetComponent<Animation> () [attack.name].length * impactTime) {
				opponent.GetComponent<NPC> ().getHit (damage, player);
				impacted = true;
			}
		}
	}

	public void getHit(int damage) {
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
			if (Vector3.Distance (transform.position, chaseTarget.transform.position) > 2) {
				GetComponent<CharacterController> ().SimpleMove (transform.forward * GetComponent<ClickToMove> ().speed);
				GetComponent<Animation> ().CrossFade (run.name);
			} else {
				chasing = false;
				GetComponent<ClickToMove>().position = transform.position;
				GetComponent<Animation> ().CrossFade (attack.name);
				ClickToMove.isAttacking = true;
			}
		}
	}
}
