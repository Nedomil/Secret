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
	public Vector3 position;
	private int rotationSpeed = 10;	//Time to rotate

	/* --- Bools --- */
	public static bool isAttacking;



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
		if(!GetComponent<Animation>().IsPlaying(getHitAnim.name))
			moveToPosition();
	}

	private void locateMousePosition() {
		RaycastHit hit;
		Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);

		//ray, returns hit, length of ray
		if(Physics.Raycast(ray, out hit, 1000) && hit.collider.tag != "Enemy") {
			GetComponent<Combat> ().chasing = false;
			position = new Vector3 (hit.point.x, hit.point.y, hit.point.z);
		}
	}

	private void moveToPosition() {
		if (Vector3.Distance (transform.position, position) > 2.5) {
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
			if(!isAttacking && !GetComponent<Combat>().gettingHit)
				GetComponent<Animation> ().CrossFade (idle.name);
		}
	}
}
