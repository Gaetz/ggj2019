using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialAttackState : MonoBehaviour {

	[SerializeField] int MaxSpecial;
	int currentSpecial;

	public void AddSpecial(int bonus) {
		currentSpecial = Mathf.Min(currentSpecial + bonus, MaxSpecial);
	}
}
