using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalMeleeAttack : Attack {

	private bool impacted;
	public bool isAttacking;

	// Use this for initialization
	void Start () {
		creature = GetComponent<Creature> ();
		lastSpecialAttack = Time.time;
		if (defaultCooldown) {
			coolDownSpecialAttackMin = 3;
			coolDownSpecialAttackMax = 3;
		}
		effectivelyAttackRange = 1;
		attackRange = effectivelyAttackRange * creature.weaponRange;
		damageMultiplicator = 1;
		damage = (int) damageMultiplicator * creature.damage;
	}

	// Update is called once per frame
	void Update () {
		readyCheck ();
		impact ();
		stopAttackAnimation ();
	}

	public override bool activate() {
		if(opponentInAttackRange() && attackReady && !creature.GetComponent<Animation>().IsPlaying(creature.attack.name)) {
			setUpActivate ();
			isAttacking = true;
			transform.LookAt (creature.opponent.transform.position);
			GetComponent<Animation> ().CrossFade (creature.attack.name);
			return true;
		}
		return false;
	}

	private void stopAttackAnimation() {
		if (isAttacking && creature.GetComponent<Animation> ().IsPlaying (creature.attack.name) &&
			GetComponent<Animation> () [creature.attack.name].time > GetComponent<Animation> () [creature.attack.name].length * 0.8) {
			GetComponent<Animation> ().CrossFade (creature.waitingForFight.name);
			isAttacking = false;
		}
	}

	private void impact() {
		if (!creature.gettingHit && isAttacking) {
			if (GetComponent<Animation> () [creature.attack.name].time < GetComponent<Animation> () [creature.attack.name].length * 0.1) {
				impacted = false;
			}
			if (opponentInAttackRange () && !impacted && GetComponent<Animation> ().IsPlaying (creature.attack.name)) {
				if (GetComponent<Animation> () [creature.attack.name].time > GetComponent<Animation> () [creature.attack.name].length * creature.impactTime && opponentInAttackRange()) {
					creature.opponent.GetComponent<Creature> ().getHit (damage, this.gameObject);
					impacted = true;
				}
			}
		}
	}
}
