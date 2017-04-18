﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public abstract class Item {

	public string Name;
	public Texture2D image;
	public GameObject prefab;
	public int id;
	public int x;
	public int y;

	public abstract void performAction ();
}
