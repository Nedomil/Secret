﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class Attack : MonoBehaviour {

	protected NPC npc;
	protected float lastSpecialAttack;
	protected int coolDownSpecialAttackMin;
	protected int coolDownSpecialAttackMax;
	public bool attackReady = false;
	public float effectivelyAttackRange;
	public float attackRange;
	protected float damageMultiplicator;
	protected int damage;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public abstract bool activate ();

	protected void setUpActivate() {
		lastSpecialAttack = Time.time;
		attackReady = false;
	}

	protected void readyCheck() {
		if (Time.time > lastSpecialAttack + coolDownSpecialAttack()) {
			attackReady = true;
		}
	}

	private int coolDownSpecialAttack() {
		int temp = (int) (Random.value * (coolDownSpecialAttackMax - coolDownSpecialAttackMin));
		return temp + coolDownSpecialAttackMin;
	}



	/*
	 * Returns, if opponent is in attack Range.
	 */
	public virtual bool opponentInAttackRange() {
		GameObject opponent = npc.opponent;
		if (opponent == null)
			return false;
		return Vector3.Distance (transform.position, opponent.transform.position) < attackRange;
	}
}
