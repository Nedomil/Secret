using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class Attack : MonoBehaviour {

	public GameObject creature;
	protected float lastSpecialAttack;
	protected int coolDownSpecialAttackMin;
	protected int coolDownSpecialAttackMax;
	public bool attackReady = false;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public virtual void activate () {
		lastSpecialAttack = Time.time;
		attackReady = false;
	}

	public void setCreature(GameObject creature) {
		this.creature = creature;
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
}
