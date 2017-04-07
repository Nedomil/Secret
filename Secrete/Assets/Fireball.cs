using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireball : CastAttack {

	// Use this for initialization
	void Start () {
		npc = GetComponent<NPC> ();		
		lastSpecialAttack = Time.time;
		coolDownSpecialAttackMin = 2;
		coolDownSpecialAttackMax = 2;
		speed = 0.1f;
		rangeBeforeDespawn = 20;
		effectivelyAttackRange = 10;
		attackRange = effectivelyAttackRange;
		damageMultiplicator = 10;
		damage = (int) damageMultiplicator * npc.damage;
	}

	public override bool activate() {
		if (opponentInAttackRange() && attackReady && !npc.GetComponent<Animation> ().IsPlaying (npc.attack.name)) {
			npc.attacking = true;
			npc.GetComponent<Animation> ().CrossFade (npc.attack.name);
			startPosition = npc.transform.position;
			endPosition = npc.opponent.transform.position;
			make ("Fireball2");
			return true;
		}
		return false;
	}
}
