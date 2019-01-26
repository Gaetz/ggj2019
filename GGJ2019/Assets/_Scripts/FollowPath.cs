using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPath : MonoBehaviour {

	[SerializeField] float speed;
	[SerializeField] Transform[] waypoints;
	[SerializeField] bool flip;
	SpriteRenderer sprite;

	public bool StopPatrol {get;set;}

	public Vector2 Velocity { get { return velocity; } private set {} }
	Vector2 velocity;

	int currentWp;
	int NextWp {
		get {
			int next = currentWp + 1;
			return next >= waypoints.Length ? 0 : next;
		}
	}

	void Start () {
		currentWp = 0;
		sprite = GetComponent<SpriteRenderer>();
		StopPatrol = false;
	}
	
	void FixedUpdate () {
		if(!StopPatrol)
		{
			velocity = (waypoints[currentWp].position - transform.position).normalized * speed * Time.deltaTime;
			// Reach next node
			if((transform.position - waypoints[currentWp].position).magnitude <= speed * Time.deltaTime) {
				velocity = Vector2.ClampMagnitude(velocity, (waypoints[currentWp].position - transform.position).magnitude);
				currentWp = NextWp;
			}
			// Flip
			if(flip) {
				if(velocity.x < 0) {
					sprite.flipX = false;
				} else {
					sprite.flipX = true;
				}
			}
			transform.Translate(velocity);
		}
	}
}
