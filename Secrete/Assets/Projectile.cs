using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour {

	public GameObject caster;
	public Vector3 startPosition;
	public Vector3 endPosition;
	public float speed;
	public float rangeBeforeDespawn;
	public int damage;
	public float height = 0.5f;

	// Use this for initialization
	void Start () {
		startPosition.y += height;
		endPosition.y += height;
		transform.position = startPosition;
		transform.LookAt (endPosition);
		//transform.position += transform.forward;  //becouse selfhit.
	}
	
	// Update is called once per frame
	void Update () {
		if (!rangeBeforeDespawnReached ()) {
			transform.position += transform.forward * speed;
		} else {
			despawn ();
		}	
	}

	void OnTriggerEnter (Collider col)
	{
		if(col.gameObject != caster && (col.gameObject.tag == "Fraction1" || col.gameObject.tag == "Fraction2" || col.gameObject.tag == "Player"))
		{
			col.gameObject.GetComponent<Creature> ().getHit (damage, caster);
			Destroy(gameObject);
		}
	}

	private void spawn(string path) {
		transform.position = startPosition;
		transform.LookAt (endPosition);
	}

	private void despawn() {
		Destroy (gameObject);
	}

	private bool rangeBeforeDespawnReached() {
		return Vector3.Distance (transform.position, startPosition) > rangeBeforeDespawn;
	}
}
