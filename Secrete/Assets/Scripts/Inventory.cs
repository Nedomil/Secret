using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour {

	public Texture2D image;
	public Rect position;

	public List<Item> items = new List<Item> ();
	int slotWidthSize = 10;
	int slotHeightSize = 4;
	public Slot[,] slots;

	public int slotX;
	public int slotY;
	public int width = 29;
	public int height = 30;

	private Item draggedItem;
	private Item secondDraggedItem;
	private Vector2 selected;
	private Vector2 secondSelected;
	bool tested = false;

	// Use this for initialization
	void Start () {
		setSlots ();	
	}

	void setSlots() {
		slots = new Slot[slotWidthSize, slotHeightSize];
		for (int i = 0; i < slotWidthSize; i++) {
			for (int j = 0; j < slotHeightSize; j++) {
				slots [i, j] = new Slot (new Rect(slotX + width * i, slotY + height * j, width, height));
			}
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (!tested) {
			tested = true;
			addItem (0, 0, Items.getArmor (0));
			addItem (0, 1, Items.getArmor (1));
		}
	}

	void OnGUI()
	{
		drawInventory ();
		drawSlots ();
		drawItems ();
		detectGUIAction ();
		draggingItem ();
	}

	void detectGUIAction() {
		if (Input.mousePosition.x > position.x && Input.mousePosition.x < position.x + position.width) {
			if (Screen.height - Input.mousePosition.y > position.y && Screen.height - Input.mousePosition.y < position.y + position.height) {
				detectMouseAction ();
				ClickToMove.guiBusy = true;
				return;
			}
		}
		ClickToMove.guiBusy = false;
	}

	void detectMouseAction() {
		for (int i = 0; i < slotWidthSize; i++) {
			for (int j = 0; j < slotHeightSize; j++) {
				Rect slot = new Rect (position.x + slots[i,j].position.x, position.y + slots[i,j].position.y, width, height);
				if (slot.Contains (new Vector2 (Input.mousePosition.x, Screen.height - Input.mousePosition.y))) {
					if (Event.current.isMouse && Input.GetMouseButtonDown (0)) {
						if (secondDraggedItem == null) {
							selected.x = i;
							selected.y = j;
							if (slots [i, j].item != null) {
								draggedItem = slots [i, j].item;
								slots [i, j].item = null;
								removeItem (i, j, slots [i, j].item);
							}
						} else {
							Item itemTemp = null;
							if (slots [i, j] != null) {
								itemTemp = slots [i, j].item;
								removeItem (i, j, itemTemp);
							}
							addItem (i, j, secondDraggedItem);
							secondDraggedItem = itemTemp;
						}
					} else if (draggedItem != null && Event.current.isMouse && Input.GetMouseButtonUp (0)) {
						secondSelected.x = i;
						secondSelected.y = j;
						if (slots [i, j] != null) {
							secondDraggedItem = slots [i, j].item;
							removeItem (i, j, secondDraggedItem);
						}
						addItem ((int)secondSelected.x, (int)secondSelected.y, draggedItem);
					}
					return;
				}
			}
		}
	}

	void draggingItem() {
		if (draggedItem != null && Input.GetMouseButton (0)) {
			GUI.DrawTexture (new Rect (Input.mousePosition.x, Screen.height - Input.mousePosition.y, width - 8, height - 8), draggedItem.image);
		}
		if (secondDraggedItem != null) {
			GUI.DrawTexture (new Rect (Input.mousePosition.x, Screen.height - Input.mousePosition.y, width - 8, height - 8), secondDraggedItem.image);
		}
	}

	void drawSlots()
	{
		for (int i = 0; i < slotWidthSize; i++) {
			for (int j = 0; j < slotHeightSize; j++) {
				slots [i, j].draw (position.x, position.y);
			}
		}
	}

	void drawItems() {
		for (int i = 0; i < items.Count; i++) {
			if(items[i] != draggedItem && items[i] != secondDraggedItem)
				GUI.DrawTexture (new Rect (4 + position.x + slotX + items[i].x * width, 4 + position.y + slotY + items[i].y * height, width - 8, height - 8), items [i].image);
		}
	}

	void addItem(int x, int y, Item item) {
		if (!slots [x, y].occupied) {
			items.Add (item);
			slots [x, y].item = item;
			slots [x, y].occupied = true;
			item.x = x;
			item.y = y;
			draggedItem = null;
		}
	}

	void removeItem(int x, int y, Item item) {
		if (slots [x, y].occupied) {
			items.Remove (item);
			slots [x, y].item = null;
			slots [x, y].occupied = false;
		}
	}

	void drawInventory() {
		position.x = Screen.width - position.width;
		position.y = Screen.height - position.height - Screen.height * 0.2f;
		GUI.DrawTexture (position, image);
	}
}
