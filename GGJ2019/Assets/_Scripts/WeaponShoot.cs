using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Shooter { Player, Enemy };

public class WeaponShoot : MonoBehaviour {

	[SerializeField] Projectile projectile;
	[SerializeField] WeaponData weaponData;
	[SerializeField] Shooter shooter;

	Animator animator;
	ControllerState state;
	bool isAutoshooting;
	bool isPlayer;
	Transform player;
	float autoTargetAngle;
	float cooldownCounter;

	public void AutoShoot(float angle) {
		isAutoshooting = true;
		autoTargetAngle = angle;
	}

	public void StopAutoShoot() {
		isAutoshooting = false;
	}

	void Start() {
		state = GetComponent<ControllerState>();
		animator = GetComponent<Animator>();
		isAutoshooting = false;
		isPlayer = shooter == Shooter.Player;
		player = GameObject.FindGameObjectWithTag("Player").transform;
		autoTargetAngle = 0;
	}
	
	// Update is called once per frame
	void Update () {
		if(isPlayer) {
			UpdatePlayer();
		} else {
			UpdateEnemy();
		}
	}

	public void UpdatePlayer() {
		cooldownCounter += Time.deltaTime;
		if (cooldownCounter > weaponData.cooldown) {
			state.Shooting = false;
		}
		if(Input.GetButton("Fire1") && cooldownCounter > weaponData.cooldown) {
			state.Shooting = true;
			ShootOnce();
		}
	}

	public void UpdateEnemy() {
		cooldownCounter += Time.deltaTime;
		if(isAutoshooting && cooldownCounter > weaponData.cooldown) {
			ShootOnce(autoTargetAngle);
		}
	}

	void ShootOnce(float targetAngle = float.PositiveInfinity) {
		HorizontalDirection hDirection = state.horizontalDirection;
		VerticalDirection vDirection = state.VerticalDirection;
		Vector2 projectilePosition = transform.position;
		bool isOrientedLeft = hDirection == HorizontalDirection.Left;
		projectilePosition += GetPositionModifier(hDirection, vDirection, isOrientedLeft);
		for(int i = 0; i < weaponData.projectileNumber; i++) {
			projectilePosition.y += Random.Range(-weaponData.verticalSpread, weaponData.verticalSpread);
			Projectile p = Instantiate(projectile, projectilePosition, Quaternion.identity);
			float directionAngle = 0;
			if(targetAngle == float.PositiveInfinity) {
				directionAngle = GetAngleWithDirection(hDirection, vDirection, isOrientedLeft);
			} else {
				directionAngle = targetAngle;
			}
			float angle = directionAngle + weaponData.defaultAngle + Random.Range(-weaponData.angularSpread, weaponData.angularSpread);
			p.Setup(angle, weaponData.projectileSpeed, weaponData.projectileLifetime, weaponData.projectileDamage, weaponData.gravityScale, weaponData.projectileMass, shooter, isOrientedLeft);
		}
		cooldownCounter = 0;
	}

	public int GetAngleWithDirection(HorizontalDirection hDirection, VerticalDirection vDirection, bool isOrientedLeft) {
		int angle = 0;
		bool isAimingDown = vDirection == VerticalDirection.Down;
		bool isAimingUp = vDirection == VerticalDirection.Up;
		if(isAimingUp) angle = 90;
		else if(isAimingDown) angle = -90;
		else {
			if(isOrientedLeft) {
				angle = 180;
			} else {
				angle = 0;
			}
		}
		return angle;
	}

	Vector2 GetPositionModifier(HorizontalDirection hDirection, VerticalDirection vDirection, bool isOrientedLeft) {
		Vector2 result = Vector2.zero;
		bool isAimingDown = vDirection == VerticalDirection.Down;
		bool isAimingUp = vDirection == VerticalDirection.Up;
		if(isAimingUp) {
			result.y += weaponData.horizontalOffset;
			if(isOrientedLeft) {
				result.x += weaponData.verticalOffset;
			} else {
				result.x -= weaponData.verticalOffset;
			}
		} else if (isAimingDown) {
			result.y -= weaponData.horizontalOffset;
			if(isOrientedLeft) {
				result.x -= weaponData.verticalOffset;
			} else {
				result.x += weaponData.verticalOffset;
			}
		} else {
			result.x += (isOrientedLeft ? -weaponData.horizontalOffset : weaponData.horizontalOffset);
			result.y += weaponData.verticalOffset;
		}
		return result;
	}
}
