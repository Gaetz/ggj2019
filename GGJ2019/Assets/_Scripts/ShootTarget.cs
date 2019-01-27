using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ShootTarget : MonoBehaviour {

	[SerializeField] int maxHp;
	[SerializeField] float invincibilityDuration;
	int hp;
	SpriteRenderer sprite;

	bool isInvicible;
	float invincibilityCounter;

	public int MaxHp { get { return maxHp; } set { maxHp = value; } }
	public int CurrentHp { get { return hp; } set { hp = value; } }


	void Start() {
		sprite = GetComponent<SpriteRenderer>();
		hp = MaxHp;
		invincibilityCounter = 0;
	}

	public void Damage(int damage) {
		/* if(gameObject.tag == "Player") {
			damage = 1;
		}*/
		/* if (invincibilityDuration > 0) {
			Color color = sprite.color;
			if(!isInvicible) {
				hp -= damage;
				isInvicible = true;
				sprite.color = new Color(color.r, color.g, color.b, 100);
			} else {
				invincibilityCounter += Time.deltaTime;
				if(invincibilityCounter > invincibilityDuration) {
					invincibilityCounter = 0;
					isInvicible = false;
					sprite.color = new Color(color.r, color.g, color.b, 255);
				}
			}
		} else {*/
		hp -= damage;
		//}
		Bleed();
		if (hp < 0) {
			if(gameObject.tag == "Player") {
				DataAccess.Instance.RemoveLive();
				SceneManager.LoadScene(SceneManager.GetActiveScene().name);
			} else {
				Destroy();
			}
		}
	}

	void Bleed() {

	}

	void Destroy() {
		Destroy(gameObject);
	}
}
