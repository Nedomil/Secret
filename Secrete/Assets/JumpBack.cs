﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpBack : Attack {

	private int jumpSize = 5;
	private int jumpRadius = 1;
	private bool isJumping = false;
	private float speed = 1;
	private Vector3 position;
	private Vector3 destination;

	// Use this for initialization
	void Start () {
		lastSpecialAttack = Time.time;
		coolDownSpecialAttackMin = 10;
		coolDownSpecialAttackMax = 20;
	}
	
	// Update is called once per frame
	void Update () {
		if (isJumping && position != destination && creature != null && GetComponent<Creature> ().underAttack) {
			transform.position = Vector3.MoveTowards (transform.position, destination, speed);
			if (transform.position == destination)
				isJumping = false;
		} else
			readyCheck ();
	}

	public override void activate() {
		base.activate ();
		position = creature.transform.position;
		destination = calculateDestination();
		isJumping = true;
	}

	private Vector3 calculateDestination() {
		Vector3 destTemp = position - jumpSize * creature.transform.forward;
		return (Vector3)Random.onUnitSphere * jumpRadius + destTemp;
	}

	public void setJumpSize(int size) {
		this.jumpSize = size;
	}

	public int getJumpSize() {
		return jumpSize;
	}
}