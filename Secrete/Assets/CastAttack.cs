using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CastAttack : Attack {

	protected Vector3 startPosition;
	protected Vector3 endPosition;
	protected float speed;
	protected float rangeBeforeDespawn;

	private bool impacted;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame§
	void Update () {
		readyCheck ();
		if (GetComponent<Animation> () [creature.attack.name].time > GetComponent<Animation> () [creature.attack.name].length * 0.95) {
			creature.attacking = false;
		}
	}

	public override bool activate() {
		return false;
	}

	public void make(string path) {
		GameObject projectileGo = (GameObject) Instantiate (Resources.Load (path));
		Projectile projectile = projectileGo.GetComponent<Projectile> ();
		projectile.startPosition = startPosition;
		projectile.endPosition = endPosition;
		projectile.speed = speed;
		projectile.rangeBeforeDespawn = rangeBeforeDespawn;
		projectile.damage = damage;
		projectile.caster = gameObject;
	}
}
