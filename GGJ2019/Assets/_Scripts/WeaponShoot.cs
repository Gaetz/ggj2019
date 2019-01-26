using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Shooter { Player, Enemy };

public class WeaponShoot : MonoBehaviour {

	[SerializeField] Projectile projectile;
  	[SerializeField] WeaponData weaponData;
	[SerializeField] Shooter shooter;

	ControllerState state;

	void Start() {
		state = GetComponent<ControllerState>();
	}

	float cooldownCounter;
	
	// Update is called once per frame
	void Update () {
		cooldownCounter += Time.deltaTime;
		if(Input.GetButton("Fire1") && cooldownCounter > weaponData.cooldown) {
			HorizontalDirection hDirection = state.horizontalDirection;
			VerticalDirection vDirection = state.VerticalDirection;
			Vector2 projectilePosition = transform.position;
			bool isOrientedLeft = hDirection == HorizontalDirection.Left;
			projectilePosition += GetPositionModifier(hDirection, vDirection, isOrientedLeft);
			for(int i = 0; i < weaponData.projectileNumber; i++) {
				projectilePosition.y += Random.Range(-weaponData.verticalSpread, weaponData.verticalSpread);
				Projectile p = Instantiate(projectile, projectilePosition, Quaternion.identity);
				float directionAngle = GetAngleWithDirection(hDirection, vDirection, isOrientedLeft);
				float angle = directionAngle + weaponData.defaultAngle + Random.Range(-weaponData.angularSpread, weaponData.angularSpread);
				p.Setup(angle, weaponData.projectileSpeed, weaponData.projectileLifetime, weaponData.projectileDamage, weaponData.gravityScale, weaponData.projectileMass, shooter, isOrientedLeft);
			}
			cooldownCounter = 0;
		}
	}

	int GetAngleWithDirection(HorizontalDirection hDirection, VerticalDirection vDirection, bool isOrientedLeft) {
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
