using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventorySystem : MonoBehaviour {

	public GameObject inventoryWindow;
	public Inventory inventory;
	private bool open;
	public InventorySlot slot1;
	public InventorySlot slot2;
	public InventorySlot slot3;
	public InventorySlot slot4;

	// Use this for initialization
	void Start () {
		open = false;
	}

	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.I)) {
			openOrCloseInventory();
		}
	}

	private void openOrCloseInventory(){
		if (open)
			closeInventory ();
		else
			openInventory ();
	}

	private void closeInventory() {
		open = false;
		inventoryWindow.SetActive (false);
	}

	private void openInventory() {
		open = true;
		inventoryWindow.SetActive (true);
		setUpInventoryItems ();
	}

	private void setUpInventoryItems() {
		/*Item item;
		GameObject icon;
		if(inventory.inventory[0] != null) {
			item = inventory.inventory [0];
			icon = (GameObject) Instantiate(Resources.Load (item.iconPath));
			icon.transform.position = slot1.transform.position;
			icon.transform.position += new Vector3 (0, 0, 1);
			icon.transform.localScale = new Vector3 (50, 50, 1);
		}*/
	}
}
