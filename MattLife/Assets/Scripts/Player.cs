using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof(Controller2D))]
public class Player : MonoBehaviour
{
	public float moveSpeed = 6;
	public float jumpHeight = 4;
	public float timeToJumpApex = .4f;
	public float accelerationTimeAirborne = .2f;
	public float accelerationTimeGrounded = .1f;

	private float jumpVelocity;
	private float gravity;

	private Vector3 velocity;
	private float velocityXSmoothing;

	[SerializeField]
	private float bounceTakeOff;
	private bool canBounce = false;
	[SerializeField]
	private float bounceTimerWindow = 0.2f;
	private IEnumerator bounceOnMonster;

	private Controller2D controller;
	//private Rigidbody2D rb2d;

	private void Awake()
	{
		controller = GetComponent<Controller2D>();
		//rb2d = GetComponent<Rigidbody2D>();
	}

	// Start is called before the first frame update
	void Start()
    {
		gravity = -(2 * jumpHeight) / Mathf.Pow(timeToJumpApex, 2);
		jumpVelocity = Mathf.Abs(gravity * timeToJumpApex);
		print("Gravity "+gravity+" JumpVelocity " + jumpVelocity);
    }

    // Update is called once per frame
    void Update()
    {
		if (controller.collisions.above || controller.collisions.below)
		{
			velocity.y = 0;
		}
		Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

		if (Input.GetButtonDown("Jump"))
		{
			if (canBounce)
			{
				velocity.y = jumpVelocity;
			}
			else if (controller.collisions.below)
			{
				velocity.y = jumpVelocity;
			}
		}

		float targetVelocityX = input.x * moveSpeed;
		velocity.x = Mathf.SmoothDamp(velocity.x, targetVelocityX, ref velocityXSmoothing, (controller.collisions.below)?accelerationTimeGrounded:accelerationTimeAirborne);
		velocity.y += gravity * Time.deltaTime;

		controller.Move(velocity * Time.deltaTime);
    }

	public void Bounce()
	{
		canBounce = true;

		bounceOnMonster = BounceOnMonster(bounceTimerWindow);
		StartCoroutine(bounceOnMonster);

		velocity.y = jumpVelocity;

		//rb2d.AddForce(new Vector2(0f, bounceTakeOff));
	}

	private IEnumerator BounceOnMonster(float time)
	{
		yield return new WaitForSeconds(time);
		canBounce = false;
		StopCoroutine(bounceOnMonster);
	}
}
