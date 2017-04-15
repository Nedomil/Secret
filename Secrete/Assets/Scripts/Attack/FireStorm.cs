using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireStorm : AreaOnMouseAttack {

	// Use this for initialization
	void Start () {
		creature = GetComponent<Creature> ();
		coolDownSpecialAttackMax = 1;
		coolDownSpecialAttackMin = 1;
		if (defaultStats)
			damageMultiplicator = 0.25f;
		damage = (int) (damageMultiplicator * creature.damage);
		time = 5;
		speed = 0.5f;
		numberOfProjectiles = 30;
		radius = 2.5f;
		spawnHeight = 10;
		spellPath = "Fireball";
		effectivelyAttackRange = 15;
		attackRange = effectivelyAttackRange;
	}
}
