using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof(Controller2D))]
public class Player : MonoBehaviour
{
	public float moveSpeed = 6;
	public float maxJumpHeight = 5;
	public float minJumpHeight = 1;
	public float timeToJumpApex = .4f;
	public float accelerationTimeAirborne = .2f;
	public float accelerationTimeGrounded = .1f;
	public float wallSlideSpeedMax = 3;
	public float wallStickTime = .25f;

	public Vector2 wallJumpClimb;
	public Vector2 wallJumpOff;
	public Vector2 wallLeap;

	private float gravity;
	private float maxJumpVelocity;
	private float minJumpVelocity;
	private float timeToWallUnstick;
	private Vector3 velocity;
	private float velocityXSmoothing;

	private Controller2D controller;

	private void Awake()
	{
		controller = GetComponent<Controller2D>();
	}

	void Start()
	{
		controller = GetComponent<Controller2D>();

		gravity = -(2 * maxJumpHeight) / Mathf.Pow(timeToJumpApex, 2);
		maxJumpVelocity = Mathf.Abs(gravity) * timeToJumpApex;
		minJumpVelocity = Mathf.Sqrt(2 * Mathf.Abs(gravity) * minJumpHeight);
		print("Gravity: " + gravity + "  Jump Velocity: " + maxJumpVelocity);
	}

	void Update()
	{
		Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
		int wallDirX = (controller.collisions.left) ? -1 : 1;

		float targetVelocityX = input.x * moveSpeed;
		velocity.x = Mathf.SmoothDamp(velocity.x, targetVelocityX, ref velocityXSmoothing, (controller.collisions.below) ? accelerationTimeGrounded : accelerationTimeAirborne);

		bool wallSliding = false;
		if ((controller.collisions.left || controller.collisions.right) && !controller.collisions.below && velocity.y < 0)
		{
			wallSliding = true;

			if (velocity.y < -wallSlideSpeedMax)
			{
				velocity.y = -wallSlideSpeedMax;
			}

			if (timeToWallUnstick > 0)
			{
				velocityXSmoothing = 0;
				velocity.x = 0;

				if (input.x != wallDirX && input.x != 0)
				{
					timeToWallUnstick -= Time.deltaTime;
				}
				else
				{
					timeToWallUnstick = wallStickTime;
				}
			}
			else
			{
				timeToWallUnstick = wallStickTime;
			}

		}

		if (Input.GetButtonDown("Jump"))
		{
			if (wallSliding)
			{
				if (wallDirX == input.x)
				{
					velocity.x = -wallDirX * wallJumpClimb.x;
					velocity.y = wallJumpClimb.y;
				}
				else if (input.x == 0)
				{
					velocity.x = -wallDirX * wallJumpOff.x;
					velocity.y = wallJumpOff.y;
				}
				else
				{
					velocity.x = -wallDirX * wallLeap.x;
					velocity.y = wallLeap.y;
				}
			}
			if (controller.collisions.below)
			{
				velocity.y = maxJumpVelocity;
			}
		}
		if (Input.GetButtonUp("Jump"))
		{
			if (velocity.y > minJumpVelocity)
			{
				velocity.y = minJumpVelocity;
			}
		}


		velocity.y += gravity * Time.deltaTime;
		controller.Move(velocity * Time.deltaTime, input);

		if (controller.collisions.above || controller.collisions.below)
		{
			velocity.y = 0;
		}

	}
}
