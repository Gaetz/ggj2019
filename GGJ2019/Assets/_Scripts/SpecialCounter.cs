using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpecialCounter : MonoBehaviour {

	SpecialAttackState player;
	Slider slider;

	void Start() {
		player = GameObject.FindGameObjectWithTag("Player").GetComponent<SpecialAttackState>();
		slider = GetComponent<Slider>();
		slider.maxValue = player.MaxSpecial;
		slider.value = 0;
	}

	void Update () {
		if(player.CurrentSpecial != slider.value) {
			slider.value = player.CurrentSpecial;
		}
	}
}
