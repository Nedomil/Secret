﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skeleton : NPC {

	// Use this for initialization
	void Start () {
		startPosition = transform.position;
		health = 150;
		damage = 10;
		exp = 60;
		attackCooldown = 2f;
		speed = 2f;
		aggroRange = 10;
		attackRange = 2.5f;
		impactTime = 0.35;
		chasingTime = 5;
		livingRadius = 10;
		lastChaseTime = -(chasingTime + 1);
		nextWalk = 5;
		enemies = new ArrayList ();
		enemies.Add ("Fraction2");
		enemies.Add("Player");
		lastSpecialAttack = Time.time;
	}	
}
