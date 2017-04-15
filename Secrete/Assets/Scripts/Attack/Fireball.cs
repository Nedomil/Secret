using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireball : CastAttack {

	// Use this for initialization
	void Start () {
		creature = GetComponent<Creature> ();		
		lastSpecialAttack = Time.time;
		if (defaultCooldown) {
			coolDownSpecialAttackMin = 2;
			coolDownSpecialAttackMax = 2;
		}
		speed = 1f;
		rangeBeforeDespawn = 20;
		effectivelyAttackRange = 10;
		attackRange = effectivelyAttackRange;
		if (defaultStats)
			damageMultiplicator = 5;
		damage = (int) damageMultiplicator * creature.damage;
	}

	public override bool activate() {
		if(attackReady && !GetComponent<Animation> ().IsPlaying (creature.attack.name)) {
			if (creature.name == "Player") {
				setUpActivate ();
				creature.attacking = true;
				creature.GetComponent<Animation> ().CrossFade (creature.attack.name);
				startPosition = transform.position;
				endPosition = getMousePosition ();
				transform.LookAt (new Vector3(endPosition.x, transform.position.y, endPosition.z));
				make ("Fireball");
				return true;
			}
			if (opponentInAttackRange ()) {
				setUpActivate ();
				creature.attacking = true;
				creature.GetComponent<Animation> ().CrossFade (creature.attack.name);
				startPosition = creature.transform.position;
				endPosition = creature.opponent.transform.position;
				make ("Fireball");
				return true;
			}
		}
		return false;
	}
}
