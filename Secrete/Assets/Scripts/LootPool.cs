using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Loot pool contains objects which can be looted. It randomly chose one or multiple
/// of them and spawn them around a chosen location.
/// </summary>
public abstract class LootPool {

	public Item[] items;
	public int minLootedItems;
	public int maxLootedItems;

	public float dropRadius = 1;

	public LootPool() {

	}


	/// <summary>
	/// Loot a random Item from the LootPool and spawns it randomly around the given x and y.
	/// </summary>
	/// <param name="x">The x coordinate.</param>
	/// <param name="y">The y coordinate.</param>
	public void loot(float x, float y) {
		int number = (int) Mathf.Round(Random.value * (maxLootedItems - minLootedItems)) + minLootedItems;
		for (int i = 1; i <= number; i++) {
			spawn (x, y, items [(int)(Random.value * items.Length)]);
		}
	}

	private void spawn(float x, float y, Item item) {
		GameObject itemToSpawn = item.prefab;
		itemToSpawn.transform.position = randomLocationInDropRadius(x, 0.17f, y);
		itemToSpawn.tag = "Loot";
		itemToSpawn.GetComponent<Loot> ().itemId = item.id;
		Object.Instantiate (itemToSpawn);
	}

	private Vector3 randomLocationInDropRadius(float x, float y, float z) {
		return (Vector3)Random.onUnitSphere * dropRadius + new Vector3(x, y, z);
	}

}
