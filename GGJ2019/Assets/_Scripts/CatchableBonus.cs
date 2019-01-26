using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatchableBonus : MonoBehaviour {

	[SerializeField] int bonusValue;
	
	public void OnTriggerExit2D(Collider2D other) {
		if(other.gameObject.tag == "Player") {
			other.gameObject.GetComponent<SpecialAttackState>().AddSpecial(bonusValue);
			Destroy(gameObject);
		}
	}
}
