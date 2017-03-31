using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zombie : NPC {

	// Use this for initialization
	void Start () {
		startPosition = transform.position;
		health = 100;
		damage = 10;
		exp = 60;
		attackCooldown = 2f;
		speed = 1f;
		aggroRange = 10;
		attackRange = 2.5f;
		impactTime = 0.35;
		chasingTime = 20;
		livingRadius = 10;
		lastChaseTime = -(chasingTime + 1);
		nextWalk = 20;
		enemies = new ArrayList ();
		enemies.Add("Fraction1");
		enemies.Add("Player");
	}	
}
