using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaOnMouseAttack : Attack {

	public Vector3 spawnPosition;
	public float radius;
	public float spawnHeight;
	public float time;
	public float speed;
	public float numberOfProjectiles;
	public string spellPath;

	protected bool isSpawning;

	private int counter;
	private float lastSpawn;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		readyCheck ();
		stopAttackAnimation ();
		if (isSpawning) {
			if (counter <= numberOfProjectiles) {
				if (spawnReady ()) {
					counter++;
					spawnProjectile (spellPath);
					lastSpawn = Time.time;
				}
			} else {
				isSpawning = false;
			}
		}
	}



	public override bool activate() {
		spawnPosition = getMousePosition ();
		if(attackReady && targetInRange() && !GetComponent<Animation> ().IsPlaying (creature.attack.name)) {
			setUpActivate ();
			creature.attacking = true;
			creature.GetComponent<Animation> ().CrossFade (creature.attack.name);
			transform.LookAt (new Vector3(spawnPosition.x, transform.position.y, spawnPosition.z));
			isSpawning = true;
			counter = 0;
			return true;
		}
		return false;
	}

	public void spawnProjectile(string path) {
		GameObject projectileGo = (GameObject)Instantiate (Resources.Load (path));
		Projectile projectile = projectileGo.GetComponent<Projectile> ();
		projectile.startPosition = randomPlaceInSpawnRadius();
		projectile.startPosition.y += spawnHeight;
		projectile.endPosition = randomPlaceInSpawnRadius();
		projectile.horizontalize = false;
		projectile.speed = speed;
		projectile.damage = damage;
		projectile.caster = gameObject;
		projectile.rangeBeforeDespawn = 100;
	}

	public bool targetInRange() {
		return Vector3.Distance (transform.position, spawnPosition) < attackRange;
	}

	private bool spawnReady() {
		return Time.time > lastSpawn + timeUntilNextSpawn ();
	}

	private Vector3 randomPlaceInSpawnRadius() {
		return (Vector3)Random.onUnitSphere * radius + spawnPosition;
	}

	private float timeUntilNextSpawn() {
		return time / numberOfProjectiles;
	}
}
