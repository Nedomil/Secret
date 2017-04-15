using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour {

	public Item item;
	public Inventory inventory;
	public int slotNumber;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		if (inventory.inventory [slotNumber] != null) {
			item = inventory.inventory [slotNumber];
			Button button = GetComponent<Button> ();
			button.image.overrideSprite = Resources.Load <Sprite> (item.iconPath);
		}
	}

	public void click() {
		if (item != null)
			Debug.Log (item.name);
		else
			Debug.Log ("nothing");
	}
}
