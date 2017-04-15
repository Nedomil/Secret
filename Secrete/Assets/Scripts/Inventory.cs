using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour {

	private bool open;
	public Item[] inventory;

	// Use this for initialization
	void Start () {
		inventory = new Item[4];
	}

	// Update is called once per frame
	void Update () {
	}

	public bool addItem(Item item) {
		for (int i = 0; i < inventory.Length; i++) {
			if (inventory [i] == null) {
				inventory [i] = item;
				return true;
			}
		}
		return false;
	}

	public void addItem(Item item, int slot) {
		inventory [slot] = item;
	}

	public void deleteItem(int slot) {
		inventory [slot] = null;
	}
}
