using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootTarget : MonoBehaviour {

	[SerializeField] int maxHp;
	int hp;

	public int MaxHp { get { return maxHp; } set { maxHp = value; } }
	public int CurrentHp { get { return hp; } set { hp = value; } }


	public void Damage(int damage) {
		if(gameObject.tag == "Player") damage = 1;
		hp -= damage;
		Bleed();
		if (hp < 0) {
			Destroy();
		}
	}

	void Bleed() {

	}

	void Destroy() {
		Destroy(gameObject);
	}
}
