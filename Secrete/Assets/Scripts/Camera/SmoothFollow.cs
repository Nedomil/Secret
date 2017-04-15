using UnityEngine;
using System.Collections;

// Place the script in the Camera-Control group in the component menu
[AddComponentMenu("Camera-Control/Smooth Follow")]

public class SmoothFollow : MonoBehaviour
{
	/*
     This camera smoothes out rotation around the y-axis and height.
     Horizontal Distance to the target is always fixed.
     
     There are many different ways to smooth the rotation but doing it this way gives you a lot of control over how the camera behaves.
     
     For every of those smoothed values we calculate the wanted value and the current value.
     Then we smooth it using the Lerp function.
     Then we apply the smoothed values to the transform's position.
     */

	public int minScroll = 1;
	public int maxScroll = 10;
	// The scrollSpeed
	public float scrollSpeed = 5f;
	// The target we are following
	public Transform target;
	// The distance in the x-z plane to the target
	public float distance = 10f;

	void  LateUpdate ()
	{
		// Early out if we don't have a target
		if (!target)
			return;

		// Set the position of the camera on the x-z plane to:
		// distance meters behind the target
		transform.position = target.position;
		// Set the height of the camera
		transform.position = new Vector3(transform.position.x, transform.position.y + distance, transform.position.z - distance);

		// Always look at the target
		transform.LookAt (target);

		scroll ();

	}

	private void scroll () {
		float d = Input.GetAxis ("Mouse ScrollWheel");
		float newDistance = distance - d * scrollSpeed;
		if (newDistance > minScroll && newDistance < maxScroll)
			distance = newDistance;
	}
}