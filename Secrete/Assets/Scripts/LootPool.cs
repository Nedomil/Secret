using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class LootPool {

	public Item[] items;
	public int minLootedItems;
	public int maxLootedItems;

	public float dropRadius = 1;

	public LootPool() {

	}

	public void loot(float x, float y) {
		int number = (int) Mathf.Round(Random.value * (maxLootedItems - minLootedItems)) + minLootedItems;
		for (int i = 1; i <= number; i++) {
			spawn (x, y, items [(int)(Random.value * items.Length)]);
		}
	}

	private void spawn(float x, float y, Item item) {
		GameObject itemToSpawn = item.prefab;
		itemToSpawn.transform.position = randomLocationInDropRadius(x, 0.17f, y);
		itemToSpawn.SetActive (true);
		Object.Instantiate (itemToSpawn);
	}

	private Vector3 randomLocationInDropRadius(float x, float y, float z) {
		return (Vector3)Random.onUnitSphere * dropRadius + new Vector3(x, y, z);
	}

}
