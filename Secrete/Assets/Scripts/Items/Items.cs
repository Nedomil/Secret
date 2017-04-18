using System.Collections.Generic;
using UnityEngine;

public class Items : MonoBehaviour {

	public List<Armor> armorInspector;
	private static List<Armor> armor;

	void Start() {
		armor = armorInspector;
	}

	public static Armor getArmor(int id) {
		Armor armor = new Armor ();
		armor.image = Items.armor [id].image;
		armor.Name = Items.armor [id].Name;
		armor.prefab = Items.armor [id].prefab;
		armor.kind = Items.armor [id].kind;
		armor.id = id;

		return armor;
	}
}
