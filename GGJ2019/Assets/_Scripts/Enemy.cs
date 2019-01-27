using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {

	enum EnemyState { Patrol, Taunting, Shoot, ShootReload }

	[SerializeField] float tauntDistance;
	[SerializeField] float shootDistance;
	[SerializeField] float shootDuration;
	[SerializeField] float reloadDuration;



	bool randomMove;
	PolygonCollider2D collide;
	Rigidbody2D rbody;
	Transform player;
	FollowPath follow;
	Animator animator;
	WeaponShoot weaponShoot;
	ControllerState controllerState;
	SpriteRenderer sprite;

	EnemyState state;
	float shootCounter;
	bool hasShot;


	// Use this for initialization
	void Start () {
		collide = GetComponent<PolygonCollider2D>();
		rbody = GetComponent<Rigidbody2D>();
		player = GameObject.FindGameObjectWithTag("Player").transform;
		state = EnemyState.Patrol;
		follow = GetComponent<FollowPath>();
		animator = GetComponent<Animator>();
		controllerState = GetComponent<ControllerState>();
		hasShot = false;
		weaponShoot = GetComponent<WeaponShoot>();
		sprite = GetComponent<SpriteRenderer>();
	}
	
	void Update () {
		// State
		if(follow.Velocity.x < 0) {
			controllerState.horizontalDirection = HorizontalDirection.Left;
		} else if(follow.Velocity.x > 0) {
			controllerState.horizontalDirection = HorizontalDirection.Right;
		}

		// Behaviour
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
				TurnTowardPlayer();
				if(between.magnitude <= shootDistance) {
					state = EnemyState.Shoot;
				} else if (between.magnitude > tauntDistance) {
					state = EnemyState.Patrol;
				}
			break;

			case EnemyState.Shoot:
				shootCounter += Time.deltaTime;
				TurnTowardPlayer();
				if(!hasShot) {
					float targetAngle = Vector2.Angle(Vector2.right, player.position - transform.position);
					weaponShoot.AutoShoot(targetAngle);
					hasShot = true;
				}
				if (shootCounter >= shootDuration) {
					weaponShoot.StopAutoShoot();
					state = EnemyState.ShootReload;
					shootCounter = 0;
				}
				if (between.magnitude > tauntDistance) {
					BackToPatrol();
				}
			break;

			case EnemyState.ShootReload:
				shootCounter += Time.deltaTime;
				if (shootCounter >= reloadDuration) 
				{
					hasShot = false;
					state = EnemyState.Shoot;
					shootCounter = 0;
				}
				if (between.magnitude > tauntDistance) {
					BackToPatrol();
				}
			break; 
		}
	}
	
	void BackToPatrol() {
		state = EnemyState.Patrol;
		shootCounter = 0;
		hasShot = false;
	}

	void TurnTowardPlayer() {
		bool isPlayerLeft = player.position.x < transform.position.x;
		if(isPlayerLeft) {
			sprite.flipX = false;
		} else {
			sprite.flipX = true;
		}
	}
}
