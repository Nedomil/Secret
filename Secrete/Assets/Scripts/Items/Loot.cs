using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Loot : MonoBehaviour {

	public int itemId;
	private Item item;
	private bool looted;

	// Use this for initialization
	void Start () {
		looted = false;
	}
	
	// Update is called once per frame
	void Update () {
		if(item == null)
			item = Items.getArmor (itemId);
		
		if (looted)
			Destroy (gameObject);
	}

	public Item loot() {
		looted = true;
		gameObject.tag = "Looted";
		return item;
	}
}
