﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour {

	enum JumpState {
		None = 0, Holding
	}

	[SerializeField] MoveData moveData;

	Vector2 velocity;
	Vector2 move;

	const float parallelInsetLen = 0.05f;
	const float perpendicularInsetLen = 0.05f;
	const float groundMargin = 0.05f;

	RaycasterVerticalDirection raycastDown;
	RaycasterHorizontalDirection raycastLeft;
	RaycasterHorizontalDirection raycastRight;
	RaycasterVerticalDirection raycastUp;

	RaycasterCheckTouch groundDown;

	float jumpStartTimer;
	float jumpHoldTimer;
	bool jumpInputDown;
	JumpState jumpState;

	bool lastGrounded;

	Animator animator;
	SpriteRenderer spriteRenderer;

	public BoxCollider2D Collider { get { return boxCollider; } private set {} }
	BoxCollider2D boxCollider;
	Rect box;

	public ControllerState State { get; set; }
	ControllerState state;

	[SerializeField] AudioSource jumpSfx, landSfx, startMoveSfx;

	float inputX, inputY;
	public Vector2 PlayerInput { get { return new Vector2(inputX, inputY); } }

	void Awake() {
		animator = GetComponent<Animator>();
		spriteRenderer = GetComponent<SpriteRenderer>();
		boxCollider = GetComponent<BoxCollider2D>();
		state = GetComponent<ControllerState>();
	}

	void Start () {
		raycastDown = new RaycasterVerticalDirection(moveData.platformMask, Vector2.down, DataAccess.Instance.EngineMoveData.margin, state);
		raycastLeft = new RaycasterHorizontalDirection(moveData.platformMask, Vector2.left, DataAccess.Instance.EngineMoveData.margin, state);
		raycastRight = new RaycasterHorizontalDirection(moveData.platformMask, Vector2.right, DataAccess.Instance.EngineMoveData.margin, state);
		raycastUp = new RaycasterVerticalDirection(moveData.platformMask, Vector2.up, DataAccess.Instance.EngineMoveData.margin, state);

		groundDown = new RaycasterCheckTouch(moveData.platformMask, Vector2.down, groundMargin, state);

	}

	void Update() {
		jumpStartTimer -= Time.deltaTime;
		bool jumpButton = Input.GetButton("Jump");
		if(jumpButton && !jumpInputDown) {
			jumpStartTimer = moveData.jumpInputLeewayPeriod;
		}
		jumpInputDown = jumpButton;
	}
	
	void FixedUpdate() {
		Reset();
		CheckGround(box);
		HandleJumpInput();

		// Input
		inputX = Input.GetAxisRaw("Horizontal");
		inputY = Input.GetAxisRaw("Vertical");
		int wantedDirectionX = GetSign(inputX);
		int velocityDirection = GetSign(velocity.x);

		HandleVerticalAim(GetSign(inputY));
		HandleHorizontalVelocity(wantedDirectionX, velocityDirection);
		HandleGravity();
		HandleMovingPlatform();

		// Move
		move = Vector2.zero;
		Vector2 wantedMove = velocity * Time.deltaTime;
		HandleMove(wantedMove);

		CancelVelocity(ref velocity, move, wantedMove);
		HandleAnimationAndDirection(wantedDirectionX);
	}

	void Reset() {
		box = GetColliderRect();
		Vector2 offset = GetColliderOffset();
		raycastDown.UpdateOrigin(box, offset);
		raycastUp.UpdateOrigin(box, offset);
		raycastLeft.UpdateOrigin(box, offset);
		raycastRight.UpdateOrigin(box, offset);
		groundDown.UpdateOrigin(box, offset);
		state.MovingPlatform = null;
	}

	void CheckGround(Rect box) {
		// Jump and moving platform
		state.Grounded = groundDown.Cast(transform.position, box) <= DataAccess.Instance.EngineMoveData.throughFloorMargin * 2;
		if (state.Grounded && !lastGrounded) {
			landSfx.Play();
			state.Falling = false;
		}
		lastGrounded = state.Grounded;
	}

	void HandleJumpInput() {
		switch(jumpState) {
			case JumpState.None:
				if (state.Grounded && jumpStartTimer > 0) {
					jumpStartTimer = 0;
					jumpHoldTimer = 0;
					jumpState = JumpState.Holding;
					velocity.y = moveData.jumpStartSpeed;
					jumpSfx.Play();
				}
				break;
			case JumpState.Holding:
				jumpHoldTimer += Time.deltaTime;
				if(!jumpInputDown || jumpHoldTimer >= moveData.jumpMaxHoldPeriod) {
					jumpState = JumpState.None;
					velocity.y = Mathf.Lerp(moveData.jumpMinSpeed, moveData.jumpStartSpeed, jumpHoldTimer / moveData.jumpMaxHoldPeriod);
				}
				break;
		}
	}

	void HandleHorizontalVelocity(int wantedDirectionX, int velocityDirection) {
		// Handle direction change
		if (wantedDirectionX != 0) {
			if (wantedDirectionX != velocityDirection) {
				velocity.x = moveData.xSnapSpeed * wantedDirectionX;
				startMoveSfx.Play();
			}	else {
				velocity.x = Mathf.MoveTowards(velocity.x, moveData.xMaxSpeed * wantedDirectionX, moveData.xAccel * Time.deltaTime);
			}
		} else {
			velocity.x = Mathf.MoveTowards(velocity.x, 0, moveData.xDecel * Time.deltaTime);
		}
	}

	void HandleGravity() {
		if(jumpState == JumpState.None) {
			velocity.y = Mathf.Max(velocity.y - moveData.gravity, -moveData.maxFallSpeed); // Negative, so we use max
		}
		if (velocity.y < 0  && !state.Grounded) {
			state.Falling = true;
		}
	}

	void HandleMovingPlatform() {
		if (state.MovingPlatform != null) {
			transform.Translate(state.MovingPlatform.Velocity);
		}
	}

	void HandleVerticalAim(int vDir) {
		switch(vDir) {
			case -1:
				if(!state.Grounded) {
					state.VerticalDirection = VerticalDirection.Down;
				} else {
					state.VerticalDirection = VerticalDirection.Normal;
				}
				break;
			case 0:
				state.VerticalDirection = VerticalDirection.Normal;
				break;
			case 1:
				state.VerticalDirection = VerticalDirection.Up;
				break;
		}
	}

	void HandleMove(Vector2 wantedMove) {
		if (velocity.y > 0) {
			raycastUp.Cast(transform.position, wantedMove.y, ref move);
		} else if (velocity.y < 0) {
			raycastDown.Cast(transform.position, -wantedMove.y, ref move, wantedMove.x);
		}

		if (velocity.x > 0) {
			raycastRight.Cast(transform.position, wantedMove.x, ref move);
		} else if (velocity.x < 0) {
			raycastLeft.Cast(transform.position, -wantedMove.x, ref move);
		}

		if(move.y >= 0) {
			state.Descending = false;
			state.Falling = false;
		}
	}

	void CancelVelocity(ref Vector2 velocity, Vector2 move, Vector2 wantedMove) {
		if (!Mathf.Approximately(wantedMove.x, move.x)) {
			velocity.x = 0;
		}
		if (!Mathf.Approximately(wantedMove.y, move.y)) {
			velocity.y = 0;
		}
	}

	void HandleAnimationAndDirection(float wantedDirectionX) {
		// Animations 
		if(jumpState == JumpState.Holding) {
			animator.Play("Jump");
		} else {
			if (state.Grounded) {
				if(state.Shooting) {
					animator.Play("Player shoot");
				} else {
					if(wantedDirectionX == 0) {
						animator.Play("Idle");
					} else {
						animator.Play("Move");
					}
				}
			} else {
				if (velocity.y < 0) {
					animator.Play("Fall");
				} else {
					animator.Play("Jump");
				}
			}
		}
		if (wantedDirectionX != 0) {
			spriteRenderer.flipX = wantedDirectionX < 0;
			state.horizontalDirection = wantedDirectionX < 0 ? HorizontalDirection.Left : HorizontalDirection.Right;
		}
	}

	int GetSign(float v) {
		if (Mathf.Approximately(v, 0)) {
			return 0;
		} else if (v > 0) {
			return 1;
		} else {
			return -1;
		}
	}

	void LateUpdate() {
		transform.Translate(move);
		FixPosition();
	}

	void FixPosition() {
		box = GetColliderRect();
		Vector2 offset = GetColliderOffset();
		groundDown.UpdateOrigin(box, offset);
		float floorDistance = groundDown.Cast(transform.position, box);
		if(floorDistance < 0 && !state.Climbing && !state.Descending) {
			transform.Translate(new Vector2(0, -floorDistance + DataAccess.Instance.EngineMoveData.throughFloorMargin));
		}
	}

	Rect GetColliderRect() {
		return new Rect(
			boxCollider.bounds.min.x,
			boxCollider.bounds.min.y,
			boxCollider.bounds.size.x,
			boxCollider.bounds.size.y
		);
	}

	Vector2 GetColliderOffset() {
		return boxCollider.offset;
	}
}
