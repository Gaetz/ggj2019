using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ShootTarget : MonoBehaviour {

	[SerializeField] int maxHp;
	[SerializeField] float invincibilityDuration;
	[SerializeField] AudioClip dieSound;
	[SerializeField] AudioClip hitSound;
    [SerializeField] ParticleSystem blood;
    public Enemy Enmy;

    int hp;
	SpriteRenderer sprite;

	bool isInvicible;
	float invincibilityCounter;

	public int MaxHp { get { return maxHp; } set { maxHp = value; } }
	public int CurrentHp { get { return hp; } set { hp = value; } }


	void Start() {
		sprite = GetComponent<SpriteRenderer>();
        Enmy = GetComponent<Enemy>();
        hp = MaxHp;
		invincibilityCounter = 0;
		if(hitSound) {
			hitSound.LoadAudioData();
		}
		if(dieSound) {
			dieSound.LoadAudioData();
		}
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
		
		if (hp < 0) {
			if(gameObject.tag == "Player") {
				DataAccess.Instance.RemoveLive();
				if(DataAccess.Instance.Lives <= 0) {
					SceneManager.LoadScene("Gameover");
				} else {
					SceneManager.LoadScene(SceneManager.GetActiveScene().name);   //Comment to get 9 lives in a row
				}
			} else {
                Bleed();
                Animator animator = GetComponent<Animator>();
                Enmy.Die();
                Destroy();
            }
			if(dieSound != null) AudioSource.PlayClipAtPoint(dieSound, transform.position);
		} else {
			if(hitSound != null) AudioSource.PlayClipAtPoint(hitSound, transform.position);
		}
	}

	void Bleed() {
        if (blood) { Instantiate(blood, transform.position, Quaternion.identity); }
    }

	void Destroy() {
		Destroy(gameObject, 0.75f);
	}
}
