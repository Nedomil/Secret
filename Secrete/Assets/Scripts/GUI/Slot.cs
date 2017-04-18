using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Slot {

	public Item item;
	public bool occupied;
	public Rect position;

	public Slot(Rect position) {
		this.position = position;
	}

	public void draw(float frameX, float frameY) {
		if(item != null)
			GUI.DrawTexture (new Rect(4 + frameX + position.x, 4 + frameY + position.y, position.width - 8, position.height - 8), item.image);
	}
}
