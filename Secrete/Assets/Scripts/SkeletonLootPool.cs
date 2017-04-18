using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonLootPool : LootPool {

	public SkeletonLootPool () {
		minLootedItems = 0;
		maxLootedItems = 1;
		items = new Item[1];
		items [0] = Items.getArmor (0);
	}

}
