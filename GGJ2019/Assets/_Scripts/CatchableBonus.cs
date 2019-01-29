using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatchableBonus : MonoBehaviour {

    [SerializeField] AudioClip bonusSound;
	[SerializeField] int bonusValue;
    public GameObject prefab;

    public void OnTriggerExit2D(Collider2D other) {
		if(other.gameObject.tag == "Player") {
			other.gameObject.GetComponent<SpecialAttackState>().AddSpecial(bonusValue);
            AudioSource.PlayClipAtPoint(bonusSound, transform.position);
            if (prefab != null)
            {
                Instantiate(prefab, this.transform.position, Quaternion.identity);
            }
            Destroy(gameObject);
		}
	}
}
