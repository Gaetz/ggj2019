using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {

	enum EnemyState { Patrol, Taunting, Shoot }

	[SerializeField] float speed;
	[SerializeField] float tauntDistance;
	[SerializeField] float shootDistance;



	bool randomMove;
	PolygonCollider2D collide;
	Rigidbody2D rbody;
	Transform player;
	FollowPath follow;
	Animator animator;


	EnemyState state;
	float counter;

	// Use this for initialization
	void Start () {
		collide = GetComponent<PolygonCollider2D>();
		rbody = GetComponent<Rigidbody2D>();
		player = GameObject.FindGameObjectWithTag("Player").transform;
		state = EnemyState.Patrol;
		follow = GetComponent<FollowPath>();
		animator = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
		counter += Time.deltaTime;
		Vector3 between = player.transform.position - transform.position;
		switch(state) {
			case EnemyState.Patrol:
				if(between.magnitude <= tauntDistance) {
					follow.StopPatrol = true;
					state = EnemyState.Taunting;
				}
			break;

			case EnemyState.Taunting:
				// TODO Launch taunt animation 
				// Animation.Play("")
				if(between.magnitude <= shootDistance) {
					state = EnemyState.Shoot;
				}
			break;

			case EnemyState.Shoot:

			break;
		}
	}
}
