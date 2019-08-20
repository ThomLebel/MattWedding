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
	public float bounceTakeOff = 7f;

	public Vector2 wallJumpClimb;
	public Vector2 wallJumpOff;
	public Vector2 wallLeap;

	private float gravity;
	private float maxJumpVelocity;
	private float minJumpVelocity;
	private float timeToWallUnstick;
	private Vector3 velocity;
	private float velocityXSmoothing;
	private bool wallSliding;
	private int wallDirX;
	private bool facingRight = true;

	[SerializeField]
	private bool canBounce = false;
	[SerializeField]
	private bool bouncing = false;
	[SerializeField]
	private float bounceTimerWindow = 0.2f;
	private IEnumerator bounceOnMonster;

	private Vector2 directionalInput;
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
		CalculateVelocity();
		HandleWallSliding();

		controller.Move(velocity * Time.deltaTime, directionalInput);

		//facingRight = (controller.collisions.faceDir == 1);

		if (controller.collisions.above || controller.collisions.below)
		{
			if (controller.collisions.slidingDownMaxSlope)
			{
				velocity.y += controller.collisions.slopeNormal.y * -gravity * Time.deltaTime;
			}
			else
			{
				velocity.y = 0;
			}
		}
		if (velocity.y <= 0 && bouncing)
		{
			bouncing = false;
		}

		if (!facingRight && directionalInput.x > 0)
		{
			Flip();
		}
		else if (facingRight && directionalInput.x < 0)
		{
			Flip();
		}
		
	}

	public void SetDirectionalInput(Vector2 input)
	{
		directionalInput = input;
	}

	public void OnJumpInputDown()
	{
		if (wallSliding)
		{
			if (wallDirX == directionalInput.x)
			{
				velocity.x = -wallDirX * wallJumpClimb.x;
				velocity.y = wallJumpClimb.y;
			}
			else if (directionalInput.x == 0)
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
			if (controller.collisions.slidingDownMaxSlope)
			{
				if (directionalInput.x != -Mathf.Sign(controller.collisions.slopeNormal.x)) // not jumping against max slope
				{
					velocity.y = maxJumpVelocity * controller.collisions.slopeNormal.y;
					velocity.x = maxJumpVelocity * controller.collisions.slopeNormal.x;
				}
			}
			else
			{
				velocity.y = maxJumpVelocity;
			}
		}
		if (canBounce)
		{
			bouncing = true;
			canBounce = false;
			velocity.y = maxJumpVelocity + bounceTakeOff;
		}
	}

	public void OnJumpInputUp()
	{
		if (velocity.y > minJumpVelocity && !bouncing)
		{
			velocity.y = minJumpVelocity;
		}
	}

	void CalculateVelocity()
	{
		float targetVelocityX = directionalInput.x * moveSpeed;
		velocity.x = Mathf.SmoothDamp(velocity.x, targetVelocityX, ref velocityXSmoothing, (controller.collisions.below) ? accelerationTimeGrounded : accelerationTimeAirborne);
		velocity.y += gravity * Time.deltaTime;
	}

	void HandleWallSliding()
	{
		wallDirX = (controller.collisions.left) ? -1 : 1;
		wallSliding = false;
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

				if (directionalInput.x != wallDirX && directionalInput.x != 0)
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
	}

	private void Flip()
	{
		// Switch the way the player is labelled as facing.
		facingRight = !facingRight;

		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;
	}

	public void Bounce()
	{
		canBounce = true;

		bounceOnMonster = BounceOnMonster(bounceTimerWindow);
		StartCoroutine(bounceOnMonster);

		velocity.y = bounceTakeOff;
	}

	private IEnumerator BounceOnMonster(float time)
	{
		yield return new WaitForSeconds(time);
		canBounce = false;
		StopCoroutine(bounceOnMonster);
	}
}
