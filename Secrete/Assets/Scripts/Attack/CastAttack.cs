using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CastAttack : Attack {

	protected Vector3 startPosition;
	protected Vector3 endPosition;
	protected float speed;
	protected float rangeBeforeDespawn;
	public float attackAngle = 50f / 180f * Mathf.PI;
	public int numberOfProjectiles = 1;

	private bool impacted;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame§
	void Update () {
		readyCheck ();
		stopAttackAnimation ();
		if (isAttacking && GetComponent<Animation> () [creature.attack.name].time > GetComponent<Animation> () [creature.attack.name].length * 0.95) {
			creature.attacking = false;
			isAttacking = false;
		}
	}

	public override bool activate() {
		return false;
	}

	public void make(string path) {
		if (numberOfProjectiles == 1) {
			GameObject projectileGo = (GameObject)Instantiate (Resources.Load (path));
			Projectile projectile = projectileGo.GetComponent<Projectile> ();
			projectile.startPosition = startPosition;
			projectile.endPosition = endPosition;
			projectile.horizontalize = true;
			projectile.speed = speed;
			projectile.rangeBeforeDespawn = rangeBeforeDespawn;
			projectile.damage = damage;
			projectile.caster = gameObject;
		} else {
			float rangeStart = attackAngle / 2;
			float rangeEnd = -rangeStart;
			float r = radius ();
			float a = angle ();

			for (int i = 1; i <= numberOfProjectiles; i++) {
				GameObject projectileGo = (GameObject)Instantiate (Resources.Load (path));
				Projectile projectile = projectileGo.GetComponent<Projectile> ();
				projectile.startPosition = startPosition;
				float effectiveAngle = a + rangeEnd + (i - 1) * attackAngle / (numberOfProjectiles - 1);
				Vector3 v = new Vector3 (startPosition.x + r * Mathf.Cos (effectiveAngle), endPosition.y, startPosition.z + r * Mathf.Sin (effectiveAngle));
				projectile.horizontalize = true;
				projectile.endPosition = v;
				projectile.speed = speed;
				projectile.rangeBeforeDespawn = rangeBeforeDespawn;
				projectile.damage = damage;
				projectile.caster = gameObject;
			}
		}
	}

	private float angle() {
		if (endPosition.x > startPosition.x)
			return Mathf.Atan ((endPosition.z - startPosition.z) / (endPosition.x - startPosition.x));
		else
			return Mathf.PI + Mathf.Atan ((endPosition.z - startPosition.z) / (endPosition.x - startPosition.x));
	}

	private float radius() {
		return Vector3.Distance (startPosition, endPosition);
	}
	
}
