using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireball : CastAttack {

	// Use this for initialization
	void Start () {
		creature = GetComponent<Creature> ();		
		lastSpecialAttack = Time.time;
		coolDownSpecialAttackMin = 2;
		coolDownSpecialAttackMax = 2;
		speed = 0.1f;
		rangeBeforeDespawn = 20;
		effectivelyAttackRange = 10;
		attackRange = effectivelyAttackRange;
		damageMultiplicator = 10;
		damage = (int) damageMultiplicator * creature.damage;
	}

	public override bool activate() {
		if (opponentInAttackRange() && attackReady && !creature.GetComponent<Animation> ().IsPlaying (creature.attack.name)) {
			creature.attacking = true;
			creature.GetComponent<Animation> ().CrossFade (creature.attack.name);
			startPosition = creature.transform.position;
			endPosition = creature.opponent.transform.position;
			make ("Fireball");
			return true;
		}
		return false;
	}
}
