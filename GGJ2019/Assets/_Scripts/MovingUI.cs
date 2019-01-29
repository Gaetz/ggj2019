using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingUI : MonoBehaviour {

	[SerializeField] Vector2 direction;
	[SerializeField] float speed;
	[SerializeField] bool forthAndBack;
	[SerializeField] float forthAndBackDuration;

	bool forth;
	float timer;
	Vector2 move;

	// Use this for initialization
	void Start () {
		timer = 0;
		forth = true;
		Vector2 move = Vector2.zero;
	}
	
	// Update is called once per frame
	void Update () {
		move = direction * speed * (forth ? 1 : -1) * Time.deltaTime;
		if(forthAndBack) {
			timer += Time.deltaTime;
			if(timer > forthAndBackDuration) {
				forth = !forth;
				timer -= forthAndBackDuration;
			}
		}
		transform.position += new Vector3(move.x, move.y, 0);
	}
}
