using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zombie : NPC {

	// Use this for initialization
	void Start () {
		startPosition = transform.position;
		health = 100;
		damage = 8;
		exp = 60;
		attackCooldown = 1.5f;
		speed = 2.5f;
		aggroRange = 10;
		weaponRange = 2.5f;
		impactTime = 0.35;
		chasingTime = 5;
		livingRadius = 10;
		lastChaseTime = -(chasingTime + 1);
		nextWalk = 20;
		enemies = new ArrayList ();
		enemies.Add("Fraction1");
		enemies.Add("Player");
		gameObject .AddComponent<JumpBack>();
		gameObject.AddComponent<NormalMeleeAttack> ();
		mainAttack = GetComponent<NormalMeleeAttack> ();
		lastSpecialAttack = Time.time;
	}
}
