using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialAttackState : MonoBehaviour {

	[SerializeField] int maxSpecial;
	public int MaxSpecial { get { return maxSpecial; } private set{} }

	public int CurrentSpecial { get { return currentSpecial; } private set{} }
	int currentSpecial;

	public void AddSpecial(int bonus) {
		currentSpecial = Mathf.Min(currentSpecial + bonus, maxSpecial);
	}
}
