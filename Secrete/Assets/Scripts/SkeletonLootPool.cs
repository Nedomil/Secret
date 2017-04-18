using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonLootPool : LootPool {

	public SkeletonLootPool () {
		minLootedItems = 0;
		maxLootedItems = 2;
		items = new Item[4];
		items [0] = Items.getArmor (0);
		items [1] = Items.getArmor (1);
		items [2] = Items.getArmor (2);
		items [3] = Items.getArmor (3);
	}

}
