using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour {

		[SerializeField] float rotationSpeed;
		[SerializeField] ParticleSystem trail;

		float angle;
		float speed;
		float lifetime;
		int damage;
		Shooter shooter;

		float lifeCounter;

		SpriteRenderer spriteRenderer;
		ParticleSystem particles;
		ParticleSystem runningTrail;
		Rigidbody2D rb;
		bool stopMove;

	void Awake () {
		spriteRenderer = GetComponent<SpriteRenderer>();
		particles = GetComponent<ParticleSystem>();
		rb = GetComponent<Rigidbody2D>();
		particles.Stop();
		stopMove = false;
		runningTrail = Instantiate(trail);
	}

	public void Setup(float angle, float speed, float lifetime, int damage, float gravityScale, float mass, Shooter shooter, bool flipY) {
		this.angle = angle;
		this.speed = speed;
		this.lifetime = lifetime;
		this.damage = damage;
		this.shooter = shooter;
		spriteRenderer.flipY = flipY;
		if(gravityScale > 0) {
			rb.gravityScale = gravityScale;
			rb.mass = mass;
			rb.AddForce(new Vector2(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad)) * speed);
		}
		if(lifetime > 0) {
			Destroy(gameObject, lifetime);
			Destroy(runningTrail, lifetime);
			Invoke("DestroyTrail", lifetime - 0.02f);
		}
		runningTrail.Play();
	}

	void Update() {
		runningTrail.transform.position = transform.position;
		if(rb.gravityScale == 0) {
			rb.velocity = new Vector2(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad)) * speed * Time.deltaTime;
		}
		if(stopMove) {
			rb.velocity = Vector2.zero;
		} else {
			transform.RotateAround(transform.position, Vector3.forward, rotationSpeed * Time.deltaTime);
		}
	}

	void DestroyTrail() {
			runningTrail.Stop();
			runningTrail.Clear();
	}

	void OnTriggerEnter2D(Collider2D other) {
		if(shooter == Shooter.Player && other.gameObject.tag == "Player") {
			return;
		}
		if(shooter == Shooter.Enemy && other.gameObject.tag == "Enemy") {
			return;
		}
		if(other.gameObject.layer != gameObject.layer) {
			runningTrail.Stop();
			runningTrail.Clear();
			particles.Play();
			stopMove = true;
			spriteRenderer.enabled = false;
			ShootTarget target = other.gameObject.GetComponent<ShootTarget>();
			if(target) {
				target.Damage(damage);
			}
			Destroy(gameObject, 0.15f);
		}
	}
}
