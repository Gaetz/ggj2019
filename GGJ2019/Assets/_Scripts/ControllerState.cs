using UnityEngine;

public enum HorizontalDirection { Left, Right }
public enum VerticalDirection { Normal, Up, Down }

public class ControllerState : MonoBehaviour {

	void Start() {
		climbing = false;
		descending = false;
		grounded = false;
		falling = false;
		hDirection = HorizontalDirection.Right;
	}

	public HorizontalDirection horizontalDirection { get {return hDirection;} set { hDirection = value; } }
	HorizontalDirection hDirection;

	public VerticalDirection VerticalDirection { get {return vDirection;} set { vDirection = value; } }
	VerticalDirection vDirection;


	public bool Climbing { get { return climbing; } set { climbing = value; } }
	bool climbing;

	public bool Descending { get { return descending; } set { descending = value; } }
	bool descending;

	public bool Grounded { get { return grounded; } set { grounded = value; } }
	bool grounded;

	public bool Falling { get { return falling; } set { falling = value; } }
	bool falling;

	public MovingPlatform MovingPlatform { get { return movingPlatform; } set { movingPlatform = value; } }
	MovingPlatform movingPlatform;

}
