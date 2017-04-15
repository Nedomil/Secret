using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * Simple aggro meter which add opponents with them aggro calculated on
 * them damage. Saves GameObjects of the opponents. First hit on a Crature
 * will do more Aggro (@see AGGRO_MULTIPLICATOR_FOR_FIRST_HIT).
 **/
public class AggroMeter {

	private List<int> aggro;
	private List<GameObject> opponents;
	private static readonly int AGGRO_MULITIPLICATOR_FOR_FIRST_HIT = 5;

	public AggroMeter() {
		aggro = new List<int> ();
		opponents = new List<GameObject> ();
	}

	public void addAggro(GameObject opponent, int dmg) {
		bool opponentAlreadyInList = false;
		foreach (GameObject go in opponents) {
			if ((GameObject) go == opponent) {
				aggro[opponents.IndexOf(go)] += dmg;
				opponentAlreadyInList = true;
			}
		}
		if (!opponentAlreadyInList) {
			int aggroToAdd;
			if (opponents.Count == 0)
				aggroToAdd = AGGRO_MULITIPLICATOR_FOR_FIRST_HIT * dmg;
			else 
				aggroToAdd = dmg;
			opponents.Add (opponent);
			aggro.Add (aggroToAdd);
		}
	}

	public GameObject getHighestAggro() {
		GameObject highestOpponent = null;
		int highestAggro = 0;
		foreach (int i in aggro) {
			if (i > highestAggro) {
				highestOpponent = opponents [aggro.IndexOf (i)];
				highestAggro = i;
			}
		}
		return highestOpponent;
	}

	public void deleteAggro(GameObject opponent) {
		int index = opponents.IndexOf (opponent);
		aggro.RemoveAt (index);
		opponents.RemoveAt (index);
	}

	/*
	 * Returns the opponent with his aggro on index i. Only needed
	 * to debug.
	 * @i index of opponent;
	 * @return arraylist with 1. opponent, 2. aggro.
	 */
	public ArrayList getOpponentAndAggro(int i) {
		if (i >= opponents.Count)
			return null;
		ArrayList result = new ArrayList ();
		result.Add (opponents [i]);
		result.Add (aggro [i]);
		return result;
	}
}
