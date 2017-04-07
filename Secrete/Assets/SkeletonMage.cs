using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonMage : NPC {

	// Use this for initialization
	void Start () {
		startPosition = transform.position;
		health = 80;
		damage = 10;
		exp = 60;
		attackCooldown = 2f;
		speed = 2f;
		aggroRange = 10;
		weaponRange = 2f;
		impactTime = 0.7;
		chasingTime = 10;
		livingRadius = 10;
		lastChaseTime = -(chasingTime + 1);
		nextWalk = 5;
		enemies = new ArrayList ();
		enemies.Add ("Fraction1");
		enemies.Add("Player");
		gameObject.AddComponent<Fireball> ();
		mainAttack = GetComponent<Fireball> ();
		lastSpecialAttack = Time.time;
	}	
}
