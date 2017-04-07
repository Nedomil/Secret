using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalMeleeAttack : Attack {

	private bool impacted;
	public bool isAttacking;

	// Use this for initialization
	void Start () {
		npc = GetComponent<NPC> ();
		lastSpecialAttack = Time.time;
		coolDownSpecialAttackMin = 3;
		coolDownSpecialAttackMax = 3;
		effectivelyAttackRange = 1;
		attackRange = effectivelyAttackRange * npc.weaponRange;
		damageMultiplicator = 1;
		damage = (int) damageMultiplicator * npc.damage;
	}

	// Update is called once per frame
	void Update () {
		readyCheck ();
		impact ();
		stopAttackAnimation ();
	}

	public override bool activate() {
		if(opponentInAttackRange() && attackReady && !npc.GetComponent<Animation>().IsPlaying(npc.attack.name)) {
			setUpActivate ();
			isAttacking = true;
			transform.LookAt (npc.opponent.transform.position);
			GetComponent<Animation> ().CrossFade (npc.attack.name);
			return true;
		}
		return false;
	}

	private void stopAttackAnimation() {
		if (npc.GetComponent<Animation> ().IsPlaying (npc.attack.name) &&
		   GetComponent<Animation> () [npc.attack.name].time > GetComponent<Animation> () [npc.attack.name].length * 0.95) {
			GetComponent<Animation> ().CrossFade (npc.waitingForFight.name);
			isAttacking = false;
		}
	}

	protected void impact() {
		if (!npc.gettingHit) {
			if (GetComponent<Animation> () [npc.attack.name].time < GetComponent<Animation> () [npc.attack.name].length * 0.1) {
				impacted = false;
			}
			if (opponentInAttackRange () && !impacted && GetComponent<Animation> ().IsPlaying (npc.attack.name)) {
				if (GetComponent<Animation> () [npc.attack.name].time > GetComponent<Animation> () [npc.attack.name].length * npc.impactTime && opponentInAttackRange()) {
					npc.opponent.GetComponent<Creature> ().getHit (damage, this.gameObject);
					impacted = true;
				}
			}
		}
	}
}
