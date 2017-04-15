using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickToMove : MonoBehaviour {

	/* --- AnimationClips --- */
	public AnimationClip run;
	public AnimationClip idle;
	public AnimationClip getHitAnim;

	/* --- Objects --- */
	public CharacterController charController;

	/* --- Stats --- */
	public float speed;
	public float lootRange = 2f;
	public Vector3 position;
	private int rotationSpeed = 10;	//Time to rotate
	private GameObject objectToLoot;


	// Use this for initialization
	void Start () {
		position = transform.position;
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetMouseButton(0)) {
			//Locate where the player clicked on the terrain
			locateMousePosition();
		}
		//Move the player to the position if not getting hit.
		if (!GetComponent<Combat> ().chasing && !GetComponent<Animation> ().IsPlaying (getHitAnim.name) && !attackGoingOn ())
			moveToPosition ();
		else
			position = transform.position;

		lootObjectIfInRange ();
	}

	private void locateMousePosition() {
		RaycastHit hit;
		Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
		int layerMask = 1 << 8;
		layerMask = ~layerMask;
		bool hitted = Physics.Raycast (ray, out hit, 1000, layerMask);

		//ray, returns hit, length of ray
		if (hitted && hit.collider.tag != "Fraction1" && hit.collider.tag != "Fraction2" && hit.collider.tag != "Player") {
			GetComponent<Combat> ().chasing = false;
			position = new Vector3 (hit.point.x, hit.point.y, hit.point.z);
			objectToLoot = null;
		}
		if (hitted && hit.collider.tag == "Loot") {
			objectToLoot = hit.transform.gameObject;
		}
	}

	private void moveToPosition() {
		if (Vector3.Distance (transform.position, position) > 0.25f) {
			//look in the right direction
			Quaternion newRotation = Quaternion.LookRotation (position - transform.position);
			newRotation.x = 0f;
			newRotation.z = 0f;

			transform.rotation = Quaternion.Slerp (transform.rotation, newRotation, Time.deltaTime * rotationSpeed);

			//move player
			charController.SimpleMove (transform.forward * speed);
			GetComponent<Animation> ().CrossFade (run.name);
		}
		//Player is not moving
		else {
			if(!attackGoingOn() && !GetComponent<Combat>().gettingHit)
				GetComponent<Animation> ().CrossFade (idle.name);
		}
	}

	private void lootObjectIfInRange() {
		if (objectToLoot != null && inLootRange (objectToLoot)) {
			GetComponent<Inventory> ().addItem (objectToLoot.GetComponent<Item> ());
			Destroy(objectToLoot); //<-----------------------------------------------------
			objectToLoot = null;
			position = transform.position;
		}
	}

	private bool inLootRange(GameObject loot) {
		return Vector3.Distance(transform.position, loot.transform.position) < lootRange;
	}

	private bool attackGoingOn () {
		Attack[] attacks = GetComponents<Attack> ();
		foreach (Attack attack in attacks) {
			if (attack.isAttacking)
				return true;
		}
		return false;
	}
}
