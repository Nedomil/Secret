using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour {

	public Texture2D image;
	public Rect position;

	int slotWidthSize = 10;
	int slotHeightSize = 4;
	public Slot[,] slots;
	public Slot helmet;

	public int slotX;
	public int slotY;
	public int width = 29;
	public int height = 30;

	private Item draggedItem;
	private Item secondDraggedItem;
	private Vector2 selected;
	private Vector2 secondSelected;

	public bool inventoryShown;

	// Use this for initialization
	void Start () {
		setSlots ();
		inventoryShown = false;
	}

	void setSlots() {
		helmet = new Slot (new Rect (131, 3, 59, 59));
		slots = new Slot[slotWidthSize, slotHeightSize];
		for (int i = 0; i < slotWidthSize; i++) {
			for (int j = 0; j < slotHeightSize; j++) {
				slots [i, j] = new Slot (new Rect(slotX + width * i, slotY + height * j, width, height));
			}
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.I) && draggedItem == null && secondDraggedItem == null) {
			inventoryShown = !inventoryShown;
			if (!inventoryShown)
				ClickToMove.guiBusy = false;
		}
	}

	/// <summary>
	/// Stores the given Item in the first non-occupied slot of the inventory. Returns false if the
	/// inventory is already full.
	/// </summary>
	/// <returns><c>true</c>, if item was looted, <c>false</c> otherwise.</returns>
	/// <param name="item">Item to store.</param>
	public bool lootItem(Item item) {
		for (int i = 0; i < slotWidthSize; i++) {
			for (int j = 0; j < slotHeightSize; j++) {
				if (!slots [i, j].occupied) {
					addItem (i, j, item);
					return true;
				}
			}
		}
		return false;
	}

	void OnGUI()
	{
		//throwAwayItem ();

		if (inventoryShown) {
			drawInventory ();
			drawSlots ();
			detectGUIAction ();
			draggingItem ();
		}

	}

	void detectGUIAction() {

		Vector2 mousePosition = getMousePosition ();
		Rect inventoryWindow = new Rect (position.x, position.y, position.width, position.height);
		Rect helmetPosition = new Rect (helmet.position.x, helmet.position.y, helmet.position.width, helmet.position.height);
		//--- What happens, if mouse on Inventory ---
		if (inventoryWindow.Contains (mousePosition)) {
			Rect slotArea = new Rect (position.x + slots[0,0].position.x, position.y + slots[0,0].position.y, 10 * width, 4 * height);
			if (slotArea.Contains (mousePosition)) {
				mouseOverInventoryAction ();
				ClickToMove.guiBusy = true;
				return;
			} else if (helmetPosition.Contains(mousePosition)) {

			} else {
				if (draggedItem != null) {
					returnDraggedItemToLastSlot ();
				}
			}
		} else {
			// --- What happens, if mouse not on Inventory ---
			throwAwayItem ();
		}

		if(draggedItem == null && secondDraggedItem == null)
			ClickToMove.guiBusy = false;
	}

	private void returnDraggedItemToLastSlot() {
		if (Event.current.isMouse && Input.GetMouseButtonUp (0)) {
			addItem (draggedItem.x, draggedItem.y, draggedItem);
			draggedItem = null;
		}
	}

	private void mouseOverInventoryAction() {
		for (int i = 0; i < slotWidthSize; i++) {
			for (int j = 0; j < slotHeightSize; j++) {
				Rect slot = new Rect (position.x + slots[i,j].position.x, position.y + slots[i,j].position.y, width, height);
				if (slot.Contains (getMousePosition())) {
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

	private Vector2 getMousePosition() {
		return new Vector2 (Input.mousePosition.x, Screen.height - Input.mousePosition.y);
	}

	void throwAwayItem() {
		if (draggedItem != null && Input.GetMouseButtonUp(0)) {
			draggedItem = null;
		}
		if (secondDraggedItem != null && Input.GetMouseButtonDown(0)) {
			secondDraggedItem = null;
		}
	}

	void draggingItem() {
		if (draggedItem != null && Input.GetMouseButton (0)) {
			GUI.DrawTexture (new Rect (Input.mousePosition.x, Screen.height - Input.mousePosition.y, width - 8, height - 8), draggedItem.image);
		}
		else if (secondDraggedItem != null) {
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
		helmet.draw (position.x, position.y);
	}

	void addItem(int x, int y, Item item) {
		if (!slots [x, y].occupied) {
			slots [x, y].item = item;
			slots [x, y].occupied = true;
			item.x = x;
			item.y = y;
			draggedItem = null;
		}
	}

	void removeItem(int x, int y, Item item) {
		if (slots [x, y].occupied) {
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
